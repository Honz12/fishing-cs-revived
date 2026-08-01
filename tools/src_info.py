import argparse
import os
import sys
from pathlib import Path

if hasattr(sys.stdout, "reconfigure"):
    sys.stdout.reconfigure(encoding="utf-8")

BASE_DIR = Path(__file__).resolve().parent.parent


def count_lines(file_path):
    try:
        with open(file_path, "r", encoding="utf-8") as f:
            return sum(1 for _ in f)
    except (UnicodeDecodeError, OSError):
        return None


def format_table(rows):
    headers = ["NAME", "SIZE (BYTES)", "LINES"]
    col_widths = [len(h) for h in headers]
    for row in rows:
        for i, cell in enumerate(row):
            col_widths[i] = max(col_widths[i], len(str(cell)))

    def fmt_row(row):
        return " │ ".join(str(cell).ljust(col_widths[i]) for i, cell in enumerate(row))

    separator = "─┼─".join("─" * w for w in col_widths)
    top = "─┬─".join("─" * w for w in col_widths)
    bottom = "─┴─".join("─" * w for w in col_widths)
    lines = [top, fmt_row(headers), separator]
    lines.extend(fmt_row(row) for row in rows)
    lines.append(bottom)
    return "\n".join(lines)


def main():
    parser = argparse.ArgumentParser(
        description="Display a table of files and directories under ../src."
    )
    parser.add_argument(
        "src_dir",
        type=str,
        nargs="?",
        default=str(BASE_DIR / "src"),
        help="Path to the src directory (default: ../src).",
    )
    args = parser.parse_args()

    src_path = Path(args.src_dir)
    if not src_path.is_dir():
        parser.error(f"'{args.src_dir}' is not a directory.")

    rows = []
    ext_stats = {}
    for root, dirs, files in os.walk(src_path):
        root_path = Path(root)
        for name in sorted(files):
            file_path = root_path / name
            size = file_path.stat().st_size
            lines = count_lines(file_path)
            rows.append(
                (
                    str(file_path.relative_to(src_path)),
                    size,
                    lines if lines is not None else "-",
                )
            )
            ext = file_path.suffix.lower() or "(no ext)"
            stats = ext_stats.setdefault(ext, {"bytes": 0, "lines": 0})
            stats["bytes"] += size
            stats["lines"] += lines or 0

    print(format_table(rows))
    print()

    summary_rows = []
    total_bytes = 0
    total_lines = 0
    for ext in sorted(ext_stats):
        stats = ext_stats[ext]
        summary_rows.append((ext, stats["bytes"], stats["lines"]))
        total_bytes += stats["bytes"]
        total_lines += stats["lines"]
    summary_rows.append(("TOTAL", total_bytes, total_lines))
    print(format_table(summary_rows))


if __name__ == "__main__":
    main()
