#!/usr/bin/env python3
"""Terminal pixel art editor for 16x16 hex-encoded images.

The file format is identical to the Tkinter version (generate_image.py):
16 lines, each 16 characters long, using the 4-bit hex palette 0-F.
"""

import argparse
import os
from collections import deque
import select
import sys
import termios

PALETTE = {
    "0": (0x00, 0x00, 0x00),  # Black
    "1": (0x80, 0x00, 0x00),  # Red
    "2": (0x00, 0x80, 0x00),  # Green
    "3": (0x80, 0x80, 0x00),  # Yellow
    "4": (0x00, 0x00, 0x80),  # Blue
    "5": (0x80, 0x00, 0x80),  # Purple
    "6": (0x00, 0x80, 0x80),  # Cyan
    "7": (0xC0, 0xC0, 0xC0),  # Dim White
    "8": (0x80, 0x80, 0x80),  # Gray
    "9": (0xFF, 0x00, 0x00),  # Light Red
    "A": (0x00, 0xFF, 0x00),  # Light Green
    "B": (0xFF, 0xFF, 0x00),  # Light Yellow
    "C": (0x00, 0x00, 0xFF),  # Light Blue
    "D": (0xFF, 0x00, 0xFF),  # Light Purple
    "E": (0x00, 0xFF, 0xFF),  # Light Cyan
    "F": (0xFF, 0xFF, 0xFF),  # White
}

COLOR_NAMES = {
    "0": "Black", "1": "Red", "2": "Green", "3": "Yellow",
    "4": "Blue", "5": "Purple", "6": "Cyan", "7": "Dim White",
    "8": "Gray", "9": "Light Red", "A": "Light Green", "B": "Light Yellow",
    "C": "Light Blue", "D": "Light Purple", "E": "Light Cyan", "F": "White",
}

# Colors dark enough to need light text on them (mirrors the Tkinter editor).
DARK_KEYS = "0124568"
GRID_SIZE = 16
GRID_WIDTH = GRID_SIZE * 2
CELL = "  "
BORDER_TL, BORDER_TR = "\u250c", "\u2510"  # ┌ ┐
BORDER_BL, BORDER_BR = "\u2514", "\u2518"  # └ ┘
BORDER_H, BORDER_V = "\u2500", "\u2502"    # ─ │


def get_rgb_color(hex_char):
    return PALETTE[hex_char.upper()]


def render_cursor_cell(key):
    """The cursor cell: [] shown with the pixel's color as background."""
    r, g, b = get_rgb_color(key)
    fg = "255;255;255" if key in DARK_KEYS else "0;0;0"
    return f"\x1b[48;2;{r};{g};{b}m\x1b[38;2;{fg}m[]\x1b[0m"


def parse_image(contents):
    lines = [line.strip() for line in contents.splitlines() if line.strip()]
    if len(lines) != GRID_SIZE:
        raise ValueError(f"Expected {GRID_SIZE} rows, got {len(lines)}")
    grid = []
    for line in lines:
        if len(line) != GRID_SIZE:
            raise ValueError(f"Row has {len(line)} characters, expected {GRID_SIZE}")
        row = []
        for ch in line:
            if ch.upper() not in PALETTE:
                raise ValueError(f"Invalid hex character: '{ch}'")
            row.append(ch.upper())
        grid.append(row)
    return grid


class Editor:
    def __init__(self, filename=None):
        self.fd = sys.stdin.fileno()
        self.old_termios = None
        self.grid = [["0" for _ in range(GRID_SIZE)] for _ in range(GRID_SIZE)]
        self.cx = 0
        self.cy = 0
        self.color = "0"
        self.filename = filename
        self.status = ""
        self.undo_stack = deque(maxlen=100)
        self.redo_stack = deque(maxlen=100)

    def load_file(self, path):
        with open(path, "r", encoding="utf-8") as f:
            self.grid = parse_image(f.read())
        self.filename = path
        self.cx = 0
        self.cy = 0
        self.undo_stack.clear()
        self.redo_stack.clear()
        self.status = f"Opened {path}"

    def paint(self, color):
        if self.grid[self.cy][self.cx] == color:
            return
        self.undo_stack.append([row[:] for row in self.grid])
        self.redo_stack.clear()
        self.grid[self.cy][self.cx] = color

    def bucket_fill(self):
        target = self.grid[self.cy][self.cx]
        if target == self.color:
            return
        self.undo_stack.append([row[:] for row in self.grid])
        self.redo_stack.clear()
        stack = [(self.cy, self.cx)]
        while stack:
            y, x = stack.pop()
            if self.grid[y][x] != target:
                continue
            self.grid[y][x] = self.color
            for ny, nx in ((y - 1, x), (y + 1, x), (y, x - 1), (y, x + 1)):
                if 0 <= ny < GRID_SIZE and 0 <= nx < GRID_SIZE:
                    stack.append((ny, nx))
        self.status = f"Filled with {self.color}"

    def undo(self):
        if not self.undo_stack:
            self.status = "Nothing to undo"
            return
        self.redo_stack.append([row[:] for row in self.grid])
        self.grid = self.undo_stack.pop()
        self.status = "Undone"

    def redo(self):
        if not self.redo_stack:
            self.status = "Nothing to redo"
            return
        self.undo_stack.append([row[:] for row in self.grid])
        self.grid = self.redo_stack.pop()
        self.status = "Redone"

    def render_row(self, y):
        row_str = BORDER_V
        for x in range(GRID_SIZE):
            key = self.grid[y][x]
            if (x, y) == (self.cx, self.cy):
                row_str += render_cursor_cell(key)
            else:
                r, g, b = get_rgb_color(key)
                row_str += f"\x1b[48;2;{r};{g};{b}m{CELL}\x1b[0m"
        return row_str + BORDER_V

    def render_palette(self):
        palette_str = "Palette: "
        for key in "0123456789ABCDEF":
            r, g, b = get_rgb_color(key)
            fg = "255;255;255" if key in DARK_KEYS else "0;0;0"
            palette_str += f"\x1b[48;2;{r};{g};{b}m\x1b[38;2;{fg}m{key} \x1b[0m"
        return palette_str

    def render_selection_marker(self):
        keys = list(PALETTE)
        index = keys.index(self.color)
        return "         " + "  " * index + "\u25b2"

    def info_line(self):
        name = self.filename or "untitled.txt"
        base = (
            f"File: {name}  Cursor: ({self.cx},{self.cy})  "
            f"Color: {self.color} {COLOR_NAMES[self.color]}"
        )
        if self.status:
            return base + f"  |  {self.status}"
        return base

    def draw(self):
        buf = ["\x1b[2J\x1b[H"]
        buf.append(BORDER_TL + BORDER_H * GRID_WIDTH + BORDER_TR)
        for y in range(GRID_SIZE):
            buf.append(self.render_row(y))
        buf.append(BORDER_BL + BORDER_H * GRID_WIDTH + BORDER_BR)
        buf.append("")
        buf.append(self.render_palette())
        buf.append(self.render_selection_marker())
        buf.append(self.info_line())
        buf.append(
            "[Arrows] move  [Space] draw  [B] fill  [E] erase  [Tab] color  "
            "[Z] undo  [R] redo  [S] save  [Ctrl+A] save as  "
            "[O] open  [Q] quit"
        )
        sys.stdout.write("\r\n".join(buf) + "\n")
        sys.stdout.flush()

    def set_raw(self):
        self.old_termios = termios.tcgetattr(self.fd)
        mode = termios.tcgetattr(self.fd)
        mode[0] = mode[0] & ~(termios.BRKINT | termios.ICRNL | termios.INPCK
                              | termios.ISTRIP | termios.IXON)
        mode[1] = mode[1] & ~termios.OPOST
        mode[2] = mode[2] & ~(termios.CSIZE | termios.PARENB)
        mode[2] = mode[2] | termios.CS8
        mode[3] = mode[3] & ~(termios.ECHO | termios.ICANON | termios.IEXTEN
                              | termios.ISIG)
        mode[6][termios.VMIN] = 1
        mode[6][termios.VTIME] = 0
        termios.tcsetattr(self.fd, termios.TCSANOW, mode)

    def restore_terminal(self):
        termios.tcsetattr(self.fd, termios.TCSADRAIN, self.old_termios)

    def hide_cursor(self):
        sys.stdout.write("\x1b[?25l")
        sys.stdout.flush()

    def show_cursor(self):
        sys.stdout.write("\x1b[?25h")
        sys.stdout.flush()

    def read_byte(self, timeout=0.05):
        if select.select([self.fd], [], [], timeout)[0]:
            try:
                data = os.read(self.fd, 1)
            except OSError:
                return None
            if data:
                return data
        return None

    def read_key(self):
        first = self.read_byte(timeout=None)
        if first is None:
            return None
        ch = first.decode("latin-1", errors="replace")
        if ch != "\x1b":
            return ch
        b1 = self.read_byte()
        if b1 is None:
            return "\x1b"
        if b1 == b"[":
            seq = b"["
            while True:
                b = self.read_byte()
                if b is None:
                    return "\x1b"
                seq += b
                if 0x40 <= b[0] <= 0x7E:
                    break
            return {
                b"[A": "up", b"[B": "down",
                b"[C": "right", b"[D": "left",
            }.get(seq)
        if b1 == b"O":
            seq = b1 + (self.read_byte() or b"")
            return {
                b"OA": "up", b"OB": "down",
                b"OC": "right", b"OD": "left",
            }.get(seq)
        return "alt_" + b1.decode("latin-1", errors="replace")

    def prompt(self, message):
        self.show_cursor()
        value = self.read_line(message)
        self.hide_cursor()
        return value

    def complete(self, text):
        """Tab completion. Returns (new text, lines occupied by any match list)."""
        if "/" in text:
            head, tail = os.path.split(text)
        else:
            head, tail = "", text
        if head.startswith("~"):
            head = os.path.expanduser(head)
        dirpath = head if head else "."
        try:
            entries = sorted(os.listdir(dirpath))
        except OSError:
            return text, 0
        matches = [entry for entry in entries if entry.startswith(tail)]
        if not matches:
            return text, 0
        if len(matches) == 1:
            name = matches[0]
            if os.path.isdir(os.path.join(dirpath, name)):
                name += "/"
            newtext = os.path.join(head, name) if head else name
            return newtext, 0
        common = os.path.commonprefix(matches)
        if common != tail:
            newtext = os.path.join(head, common) if head else common
            return newtext, 0
        return text, self._show_matches(matches)

    def _show_matches(self, matches):
        text = "  ".join(matches)
        width = max(1, os.get_terminal_size(self.fd).columns)
        lines = 1 + (len(text) + width - 1) // width
        sys.stdout.write("\r\n" + text + "\r\n")
        sys.stdout.flush()
        return lines

    def _redraw_line(self, message, line, cursor):
        sys.stdout.write("\r" + message + " " + line + "\x1b[K")
        back = len(line) - cursor
        if back > 0:
            sys.stdout.write(f"\x1b[{back}D")
        sys.stdout.flush()

    def read_line(self, message):
        line = ""
        cursor = 0
        sys.stdout.write("\r" + message + " ")
        sys.stdout.flush()
        while True:
            key = self.read_key()
            if key in (None, "up", "down"):
                continue
            if key == "left":
                if cursor > 0:
                    cursor -= 1
                    sys.stdout.write("\x1b[D")
                    sys.stdout.flush()
            elif key == "right":
                if cursor < len(line):
                    cursor += 1
                    sys.stdout.write("\x1b[C")
                    sys.stdout.flush()
            elif key == "\t":
                newline, up = self.complete(line)
                if up:
                    sys.stdout.write(f"\x1b[{up}A")
                if newline != line:
                    line = newline
                    cursor = len(line)
                self._redraw_line(message, line, cursor)
            elif key in ("\r", "\n"):
                sys.stdout.write("\r\n")
                sys.stdout.flush()
                return line
            elif key in ("\x7f", "\x08"):
                if cursor > 0:
                    line = line[:cursor - 1] + line[cursor:]
                    cursor -= 1
                    self._redraw_line(message, line, cursor)
            elif key in ("\x1b", "\x03", "\x04"):
                sys.stdout.write("\r\n")
                sys.stdout.flush()
                return ""
            elif len(key) == 1 and key.isprintable():
                line = line[:cursor] + key + line[cursor:]
                cursor += 1
                self._redraw_line(message, line, cursor)

    def write_file(self, path):
        try:
            with open(path, "w", encoding="utf-8") as f:
                for row in self.grid:
                    f.write("".join(row) + "\n")
            self.filename = path
            self.status = f"Saved {path}"
        except OSError as e:
            self.status = f"Save failed: {e}"

    def save(self):
        if not self.filename:
            self.save_as()
        else:
            self.write_file(self.filename)

    def save_as(self):
        path = self.prompt("Save as:")
        if not path:
            self.status = "Save cancelled"
            return
        self.write_file(path)

    def open_file(self):
        path = self.prompt("Open file:")
        if not path:
            self.status = "Open cancelled"
            return
        try:
            self.load_file(path)
        except (OSError, ValueError) as e:
            self.status = f"Open failed: {e}"

    def cycle_color(self):
        keys = list(PALETTE)
        self.color = keys[(keys.index(self.color) + 1) % len(keys)]

    def run(self):
        self.set_raw()
        self.hide_cursor()
        try:
            while True:
                self.draw()
                key = self.read_key()
                if key is None:
                    continue
                if key == "up":
                    self.cy = max(0, self.cy - 1)
                elif key == "down":
                    self.cy = min(GRID_SIZE - 1, self.cy + 1)
                elif key == "left":
                    self.cx = max(0, self.cx - 1)
                elif key == "right":
                    self.cx = min(GRID_SIZE - 1, self.cx + 1)
                elif key == " ":
                    self.paint(self.color)
                elif key in "eE":
                    self.paint("0")
                elif key == "\t":
                    self.cycle_color()
                elif key in "zZ":
                    self.undo()
                elif key in "rR":
                    self.redo()
                elif key in "sS":
                    self.save()
                elif key == "\x01":
                    self.save_as()
                elif key in "bB":
                    self.bucket_fill()
                elif key in "oO":
                    self.open_file()
                elif key in "qQ":
                    break
        except KeyboardInterrupt:
            pass
        finally:
            self.show_cursor()
            self.restore_terminal()
            sys.stdout.write("\n")
            sys.stdout.flush()


def main():
    parser = argparse.ArgumentParser(
        description="Edit 16x16 hex-encoded images (format of the Tkinter editor)."
    )
    parser.add_argument(
        "file_path",
        nargs="?",
        type=str,
        help="Path to a hex image (.txt) to open on startup.",
    )
    args = parser.parse_args()

    editor = Editor()
    if args.file_path:
        try:
            editor.load_file(args.file_path)
        except (OSError, ValueError) as e:
            print(f"Error: {e}", file=sys.stderr)
            sys.exit(1)
    editor.run()


if __name__ == "__main__":
    main()
