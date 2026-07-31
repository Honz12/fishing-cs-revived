import argparse
import sys

# Palette mapping hex characters (0-F) directly to RGB tuples
PALETTE = {
    '0': (0x00, 0x00, 0x00),  # Black
    '1': (0x80, 0x00, 0x00),  # Red
    '2': (0x00, 0x80, 0x00),  # Green
    '3': (0x80, 0x80, 0x00),  # Yellow
    '4': (0x00, 0x00, 0x80),  # Blue
    '5': (0x80, 0x00, 0x80),  # Purple
    '6': (0x00, 0x80, 0x80),  # Cyan
    '7': (0xC0, 0xC0, 0xC0),  # Dim White
    '8': (0x80, 0x80, 0x80),  # Gray
    '9': (0xFF, 0x00, 0x00),  # Light Red
    'A': (0x00, 0xFF, 0x00),  # Light Green
    'B': (0xFF, 0xFF, 0x00),  # Light Yellow
    'C': (0x00, 0x00, 0xFF),  # Light Blue
    'D': (0xFF, 0x00, 0xFF),  # Light Purple
    'E': (0x00, 0xFF, 0xFF),  # Light Cyan
    'F': (0xFF, 0xFF, 0xFF),  # White
}


def get_rgb_color(hex_char):
    """Retrieve RGB tuple for a given hex character."""
    char_upper = hex_char.upper()
    if char_upper not in PALETTE:
        raise ValueError(f"Invalid hex character: '{hex_char}'")
    return PALETTE[char_upper]


def render_half_block_image(image_str):
    lines = [line.strip() for line in image_str.strip().splitlines() if line.strip()]
    if not lines:
        return

    # Pad with an empty row (index '0') if the height is odd
    if len(lines) % 2 != 0:
        lines.append('0' * len(lines[0]))

    output = []
    
    # Process two rows at a time
    for y in range(0, len(lines), 2):
        top_row = lines[y]
        bottom_row = lines[y + 1]
        
        row_str = ""
        for x in range(len(top_row)):
            top_r, top_g, top_b = get_rgb_color(top_row[x])
            bot_r, bot_g, bot_b = get_rgb_color(bottom_row[x])
            
            # 48;2;R;G;Bm sets the background (top pixel)
            # 38;2;R;G;Bm sets the foreground (bottom pixel)
            row_str += f"\033[48;2;{top_r};{top_g};{top_b}m\033[38;2;{bot_r};{bot_g};{bot_b}m▄"
            
        row_str += "\033[0m"  # Reset formatting at end of line
        output.append(row_str)

    print("\n".join(output))


def main():
    parser = argparse.ArgumentParser(
        description="View custom hex-encoded images using TrueColor half-blocks."
    )
    parser.add_argument(
        "file_path",
        type=str,
        help="Path to the text file containing hex image data."
    )

    args = parser.parse_args()

    try:
        with open(args.file_path, "r", encoding="utf-8") as f:
            image_data = f.read()
        render_half_block_image(image_data)
    except FileNotFoundError:
        print(f"Error: File not found at '{args.file_path}'", file=sys.stderr)
        sys.exit(1)
    except ValueError as e:
        print(f"Error: {e}", file=sys.stderr)
        sys.exit(1)


if __name__ == "__main__":
    main()