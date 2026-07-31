import tkinter as tk
from tkinter import filedialog, messagebox

# Exact 4-bit HEX Color Mapping
COLOR_PALETTE = {
    "0": ("#000000", "Black"),
    "1": ("#800000", "Red"),
    "2": ("#008000", "Green"),
    "3": ("#808000", "Yellow"),
    "4": ("#000080", "Blue"),
    "5": ("#800080", "Purple"),
    "6": ("#008080", "Cyan"),
    "7": ("#C0C0C0", "Dim White"),
    "8": ("#808080", "Gray"),
    "9": ("#FF0000", "Light Red"),
    "A": ("#00FF00", "Light Green"),
    "B": ("#FFFF00", "Light Yellow"),
    "C": ("#0000FF", "Light Blue"),
    "D": ("#FF00FF", "Light Purple"),
    "E": ("#00FFFF", "Light Cyan"),
    "F": ("#FFFFFF", "White"),
}

GRID_SIZE = 16
PIXEL_SIZE = 24


class PixelArtEditor:

    def __init__(self, root):
        self.root = root
        self.root.title("16x16 4-Bit Pixel Art Editor")

        # Default selected color key ('0' = Black)
        self.current_color_key = "0"

        # 16x16 grid storage initialized to '0' (Black)
        self.grid_data = [["0" for _ in range(GRID_SIZE)] for _ in range(GRID_SIZE)]

        self.setup_ui()

    def setup_ui(self):
        # --- Left Panel: Palette ---
        palette_frame = tk.Frame(self.root, bd=2, relief=tk.SUNKEN)
        palette_frame.pack(side=tk.LEFT, fill=tk.Y, padx=5, pady=5)

        tk.Label(palette_frame, text="Palette", font=("Arial", 12, "bold")).pack(
            pady=5
        )

        self.palette_buttons = {}
        for key, (hex_code, name) in COLOR_PALETTE.items():
            # Determine text color for contrast
            fg_color = "#FFFFFF" if key in ["0", "1", "2", "4", "5", "6", "8"] else "#000000"

            btn = tk.Button(
                palette_frame,
                text=f"{key} - {name}",
                bg=hex_code,
                fg=fg_color,
                anchor="w",
                width=15,
                command=lambda k=key: self.select_color(k),
            )
            btn.pack(fill=tk.X, padx=2, pady=1)
            self.palette_buttons[key] = btn

        # Highlight default selected color
        self.select_color("0")

        # --- Right Panel: Canvas & Actions ---
        right_frame = tk.Frame(self.root)
        right_frame.pack(side=tk.RIGHT, padx=5, pady=5)

        # Drawing Canvas
        canvas_width = GRID_SIZE * PIXEL_SIZE
        canvas_height = GRID_SIZE * PIXEL_SIZE
        self.canvas = tk.Canvas(
            right_frame,
            width=canvas_width,
            height=canvas_height,
            bg="#333333",
            highlightthickness=1,
            highlightbackground="#888888",
        )
        self.canvas.pack(pady=5)

        # Canvas Mouse Binds for drawing/dragging
        self.canvas.bind("<Button-1>", self.paint_pixel)
        self.canvas.bind("<B1-Motion>", self.paint_pixel)

        # Save Button
        save_btn = tk.Button(
            right_frame,
            text="Save to .txt",
            font=("Arial", 10, "bold"),
            bg="#e1e1e1",
            command=self.save_file,
        )
        save_btn.pack(fill=tk.X, pady=5)

        self.draw_grid()

    def select_color(self, key):
        self.current_color_key = key
        # Reset border styles for palette buttons
        for k, btn in self.palette_buttons.items():
            btn.config(relief=tk.RAISED, bd=2)
        # Highlight selected button
        self.palette_buttons[key].config(relief=tk.SOLID, bd=3)

    def draw_grid(self):
        self.canvas.delete("all")
        for r in range(GRID_SIZE):
            for c in range(GRID_SIZE):
                x1 = c * PIXEL_SIZE
                y1 = r * PIXEL_SIZE
                x2 = x1 + PIXEL_SIZE
                y2 = y1 + PIXEL_SIZE

                color_key = self.grid_data[r][c]
                hex_color = COLOR_PALETTE[color_key][0]

                self.canvas.create_rectangle(
                    x1, y1, x2, y2, fill=hex_color, outline="#444444"
                )

    def paint_pixel(self, event):
        col = event.x // PIXEL_SIZE
        row = event.y // PIXEL_SIZE

        # Ensure clicks stay within bounds
        if 0 <= row < GRID_SIZE and 0 <= col < GRID_SIZE:
            if self.grid_data[row][col] != self.current_color_key:
                self.grid_data[row][col] = self.current_color_key

                # Redraw only the modified pixel for better performance
                x1 = col * PIXEL_SIZE
                y1 = row * PIXEL_SIZE
                x2 = x1 + PIXEL_SIZE
                y2 = y1 + PIXEL_SIZE
                hex_color = COLOR_PALETTE[self.current_color_key][0]

                self.canvas.create_rectangle(
                    x1, y1, x2, y2, fill=hex_color, outline="#444444"
                )

    def save_file(self):
        file_path = filedialog.asksaveasfilename(
            defaultextension=".txt",
            filetypes=[("Text Files", "*.txt"), ("All Files", "*.*")],
        )
        if not file_path:
            return

        try:
            with open(file_path, "w", encoding="utf-8") as f:
                for row in self.grid_data:
                    line = "".join(row) + "\n"
                    f.write(line)
            messagebox.showinfo("Success", f"File saved successfully to:\n{file_path}")
        except Exception as e:
            messagebox.showerror("Error", f"Failed to save file:\n{e}")


if __name__ == "__main__":
    root = tk.Tk()
    app = PixelArtEditor(root)
    root.mainloop()
