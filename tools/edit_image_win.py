#!/usr/bin/env python3
"""Terminal pixel art editor for 16x16 hex-encoded images (Windows port).

Windows-native port of edit_image.py. Rendering uses the same ANSI escape
sequences (24-bit color) via ENABLE_VIRTUAL_TERMINAL_PROCESSING; keyboard and
mouse input come from the Win32 console input buffer (ReadConsoleInputW)
instead of termios/select, so no Unix-only modules are required.
"""

import argparse
import ctypes
import os
import shutil
import sys
import time
from collections import deque
from ctypes import wintypes

STD_INPUT_HANDLE = -10
STD_OUTPUT_HANDLE = -11

ENABLE_PROCESSED_INPUT = 0x0001
ENABLE_LINE_INPUT = 0x0002
ENABLE_ECHO_INPUT = 0x0004
ENABLE_WINDOW_INPUT = 0x0008
ENABLE_MOUSE_INPUT = 0x0010
ENABLE_QUICK_EDIT_MODE = 0x0040
ENABLE_EXTENDED_FLAGS = 0x0080
ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004

KEY_EVENT = 0x0001
MOUSE_EVENT = 0x0002
MOUSE_BUTTON_LEFT = 0x0001
MOUSE_BUTTON_RIGHT = 0x0002

VK_UP = 0x26
VK_DOWN = 0x28
VK_LEFT = 0x25
VK_RIGHT = 0x27


class COORD(ctypes.Structure):
    _fields_ = [("X", wintypes.SHORT), ("Y", wintypes.SHORT)]


class KEY_EVENT_RECORD(ctypes.Structure):
    _fields_ = [
        ("bKeyDown", wintypes.BOOL),
        ("wRepeatCount", wintypes.WORD),
        ("wVirtualKeyCode", wintypes.WORD),
        ("wVirtualScanCode", wintypes.WORD),
        ("UnicodeChar", wintypes.WCHAR),
        ("dwControlKeyState", wintypes.DWORD),
    ]


class MOUSE_EVENT_RECORD(ctypes.Structure):
    _fields_ = [
        ("dwMousePosition", COORD),
        ("dwButtonState", wintypes.DWORD),
        ("dwControlKeyState", wintypes.DWORD),
        ("dwEventFlags", wintypes.DWORD),
    ]


class _EVENT_UNION(ctypes.Union):
    _fields_ = [
        ("KeyEvent", KEY_EVENT_RECORD),
        ("MouseEvent", MOUSE_EVENT_RECORD),
    ]


class INPUT_RECORD(ctypes.Structure):
    _fields_ = [("EventType", wintypes.WORD), ("Event", _EVENT_UNION)]


kernel32 = ctypes.windll.kernel32
kernel32.GetStdHandle.argtypes = [wintypes.DWORD]
kernel32.GetStdHandle.restype = wintypes.HANDLE
kernel32.GetConsoleMode.argtypes = [wintypes.HANDLE, ctypes.POINTER(wintypes.DWORD)]
kernel32.GetConsoleMode.restype = wintypes.BOOL
kernel32.SetConsoleMode.argtypes = [wintypes.HANDLE, wintypes.DWORD]
kernel32.SetConsoleMode.restype = wintypes.BOOL
kernel32.GetNumberOfConsoleInputEvents.argtypes = [
    wintypes.HANDLE,
    ctypes.POINTER(wintypes.DWORD),
]
kernel32.GetNumberOfConsoleInputEvents.restype = wintypes.BOOL
kernel32.ReadConsoleInputW.argtypes = [
    wintypes.HANDLE,
    ctypes.POINTER(INPUT_RECORD),
    wintypes.DWORD,
    ctypes.POINTER(wintypes.DWORD),
]
kernel32.ReadConsoleInputW.restype = wintypes.BOOL

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
    "0": "Black",
    "1": "Red",
    "2": "Green",
    "3": "Yellow",
    "4": "Blue",
    "5": "Purple",
    "6": "Cyan",
    "7": "Dim White",
    "8": "Gray",
    "9": "Light Red",
    "A": "Light Green",
    "B": "Light Yellow",
    "C": "Light Blue",
    "D": "Light Purple",
    "E": "Light Cyan",
    "F": "White",
}

# Colors dark enough to need light text on them (mirrors the Tkinter editor).
DARK_KEYS = "0124568"
GRID_SIZE = 16
GRID_WIDTH = GRID_SIZE * 2
CELL = "  "
BORDER_TL, BORDER_TR = "\u250c", "\u2510"  # ┌ ┐
BORDER_BL, BORDER_BR = "\u2514", "\u2518"  # └ ┘
BORDER_H, BORDER_V = "\u2500", "\u2502"  # ─ │


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
        self.in_handle = None
        self.out_handle = None
        self.old_in_mode = None
        self.old_out_mode = None
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
            "[Arrows] move  [Space/L-click] draw  [R-click] erase  "
            "[B] fill  [E] erase  [Tab] color"
        )
        buf.append("[Z] undo  [R] redo  [S] save  [Ctrl+A] save as  [O] open  [Q] quit")
        sys.stdout.write("\r\n".join(buf) + "\n")
        sys.stdout.flush()

    def set_raw(self):
        self.in_handle = kernel32.GetStdHandle(STD_INPUT_HANDLE)
        self.out_handle = kernel32.GetStdHandle(STD_OUTPUT_HANDLE)
        mode = wintypes.DWORD()
        if not kernel32.GetConsoleMode(self.in_handle, ctypes.byref(mode)):
            return False
        self.old_in_mode = mode.value
        new_mode = (
            mode.value
            & ~(
                ENABLE_PROCESSED_INPUT
                | ENABLE_LINE_INPUT
                | ENABLE_ECHO_INPUT
                | ENABLE_QUICK_EDIT_MODE
            )
            | ENABLE_MOUSE_INPUT
            | ENABLE_WINDOW_INPUT
            | ENABLE_EXTENDED_FLAGS
        )
        kernel32.SetConsoleMode(self.in_handle, new_mode)
        mode = wintypes.DWORD()
        if not kernel32.GetConsoleMode(self.out_handle, ctypes.byref(mode)):
            return False
        self.old_out_mode = mode.value
        kernel32.SetConsoleMode(
            self.out_handle, mode.value | ENABLE_VIRTUAL_TERMINAL_PROCESSING
        )
        return True

    def restore_terminal(self):
        if self.in_handle is not None and self.old_in_mode is not None:
            kernel32.SetConsoleMode(self.in_handle, self.old_in_mode)
        if self.out_handle is not None and self.old_out_mode is not None:
            kernel32.SetConsoleMode(self.out_handle, self.old_out_mode)

    def hide_cursor(self):
        sys.stdout.write("\x1b[?25l")
        sys.stdout.flush()

    def show_cursor(self):
        sys.stdout.write("\x1b[?25h")
        sys.stdout.flush()

    def read_record(self):
        record = INPUT_RECORD()
        count = wintypes.DWORD()
        if not kernel32.ReadConsoleInputW(
            self.in_handle, ctypes.byref(record), 1, ctypes.byref(count)
        ):
            return None
        return record

    def read_key(self, timeout=0.05):
        deadline = None if timeout is None else time.monotonic() + timeout
        while True:
            pending = wintypes.DWORD()
            if not kernel32.GetNumberOfConsoleInputEvents(
                self.in_handle, ctypes.byref(pending)
            ):
                return None
            if pending.value > 0:
                record = self.read_record()
                if record is None:
                    return None
                if record.EventType == KEY_EVENT:
                    if record.Event.KeyEvent.bKeyDown:
                        key = self.key_from_event(record.Event.KeyEvent)
                        if key is not None:
                            return key
                elif record.EventType == MOUSE_EVENT:
                    return self.mouse_from_event(record.Event.MouseEvent)
                if deadline is not None and time.monotonic() >= deadline:
                    return None
                continue
            if deadline is not None and time.monotonic() >= deadline:
                return None
            time.sleep(0.005)

    def key_from_event(self, event):
        ch = event.UnicodeChar
        if ch and ch != "\x00":
            return ch
        return {
            VK_UP: "up",
            VK_DOWN: "down",
            VK_LEFT: "left",
            VK_RIGHT: "right",
        }.get(event.wVirtualKeyCode)

    def mouse_from_event(self, event):
        # Coordinates are 0-based console buffer rows/columns; handle_mouse
        # expects the same convention.
        x = event.dwMousePosition.X
        y = event.dwMousePosition.Y
        if event.dwButtonState & MOUSE_BUTTON_LEFT:
            return ("mouse", 0, x, y, True)
        if event.dwButtonState & MOUSE_BUTTON_RIGHT:
            return ("mouse", 2, x, y, True)
        return ("mouse", 3, x, y, False)

    def handle_mouse(self, button, x, y, press):
        base = button & 3
        # Palette row: "Palette: " (9 chars) then 16 swatches of 2 columns.
        if y == GRID_SIZE + 4:
            if press and base == 0:
                index = (x - 9) // 2
                if 0 <= index < GRID_SIZE:
                    self.color = "0123456789ABCDEF"[index]
            return
        # Grid occupies buffer rows 2..17, cells start after the 1-char border.
        if 2 <= y <= GRID_SIZE + 1:
            gx = (x - 1) // 2
            gy = y - 2
            if 0 <= gx < GRID_SIZE and 0 <= gy < GRID_SIZE:
                self.cx, self.cy = gx, gy
                if press and base in (0, 2):
                    self.paint("0" if base == 2 else self.color)

    def prompt(self, message):
        self.show_cursor()
        value = self.read_line(message)
        self.hide_cursor()
        return value

    def complete(self, text):
        """Tab completion. Returns (new text, lines occupied by any match list)."""
        if os.altsep and os.altsep in text:
            text = text.replace(os.altsep, os.sep)
        head, tail = os.path.split(text)
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
                name += os.sep
            newtext = os.path.join(head, name) if head else name
            return newtext, 0
        common = os.path.commonprefix(matches)
        if common != tail:
            newtext = os.path.join(head, common) if head else common
            return newtext, 0
        return text, self._show_matches(matches)

    def _show_matches(self, matches):
        text = "  ".join(matches)
        width = max(1, shutil.get_terminal_size().columns)
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
                    line = line[: cursor - 1] + line[cursor:]
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
        if not self.set_raw():
            print("This editor requires a Windows console.", file=sys.stderr)
            return
        self.hide_cursor()
        try:
            while True:
                self.draw()
                key = self.read_key()
                if key is None:
                    continue
                if isinstance(key, tuple):
                    if key[0] == "mouse":
                        _, button, mx, my, press = key
                        self.handle_mouse(button, mx, my, press)
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
