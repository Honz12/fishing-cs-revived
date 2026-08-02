#!/bin/bash
# Sestavi hru pro Linux i Windows do slozky build/ (prepise stary obsah).
# Vy zduje samostatne binarky, ktere nepotrebuji nainstalovany .NET.
set -e

cd "$(dirname "$0")/.."

PROJECT="fishing-cs-revived.csproj"
BUILD_DIR="build"

DOTNET="$(command -v dotnet || true)"
if [ -z "$DOTNET" ] && [ -x "$HOME/.dotnet/dotnet" ]; then
    DOTNET="$HOME/.dotnet/dotnet"
fi
if [ -z "$DOTNET" ]; then
    echo "Chyba: príkaz 'dotnet' nebyl nalezen. Nainstaluj .NET SDK 10.0."
    exit 1
fi
LINUX_DIR="$BUILD_DIR/linux"
WINDOWS_DIR="$BUILD_DIR/windows"

if [ -d "$BUILD_DIR" ]; then
    echo "Mazu stary obsah slozky $BUILD_DIR ..."
    rm -rf "$BUILD_DIR"
fi

echo "=== Sestavuji Linux (linux-x64) ==="
"$DOTNET" publish "$PROJECT" -c Release -r linux-x64 --self-contained true \
    -p:PublishSingleFile=true -p:PublishTrimmed=false -o "$LINUX_DIR"

echo "=== Sestavuji Windows (win-x64) ==="
"$DOTNET" publish "$PROJECT" -c Release -r win-x64 --self-contained true \
    -p:PublishSingleFile=true -p:PublishTrimmed=false -o "$WINDOWS_DIR"

# Launcher pro Linux: poklik v souborovem manazeru jinak spusti hru bez terminalu
cat > "$LINUX_DIR/spustit.sh" <<'EOF'
#!/bin/bash
cd "$(dirname "$0")"
GAME="$(pwd)/fishing-cs-revived"

run_game() {
    bash -c "\"$GAME\"; read -rsp $'Hra skoncila. Stiskni klavesu pro zavreni...' -n 1 >/dev/null 2>&1"
}

for term in gnome-terminal konsole xfce4-terminal x-terminal-emulator tilix kitty alacritty wezterm xterm; do
    if command -v "$term" >/dev/null 2>&1; then
        case "$term" in
            gnome-terminal)  "$term" -- bash -c "run_game() { \"$GAME\"; read -rsp $'Hra skoncila. Stiskni klavesu pro zavreni...' -n 1 >/dev/null 2>&1; }; run_game" ;;
            konsole)         "$term" -e bash -c "run_game() { \"$GAME\"; read -rsp $'Hra skoncila. Stiskni klavesu pro zavreni...' -n 1 >/dev/null 2>&1; }; run_game" ;;
            xfce4-terminal)  "$term" -e bash -c "run_game() { \"$GAME\"; read -rsp $'Hra skoncila. Stiskni klavesu pro zavreni...' -n 1 >/dev/null 2>&1; }; run_game" ;;
            x-terminal-emulator) "$term" -e bash -c "run_game() { \"$GAME\"; read -rsp $'Hra skoncila. Stiskni klavesu pro zavreni...' -n 1 >/dev/null 2>&1; }; run_game" ;;
            tilix)           "$term" -e bash -c "run_game() { \"$GAME\"; read -rsp $'Hra skoncila. Stiskni klavesu pro zavreni...' -n 1 >/dev/null 2>&1; }; run_game" ;;
            kitty)           "$term" bash -c "run_game() { \"$GAME\"; read -rsp $'Hra skoncila. Stiskni klavesu pro zavreni...' -n 1 >/dev/null 2>&1; }; run_game" ;;
            alacritty)       "$term" -e bash -c "run_game() { \"$GAME\"; read -rsp $'Hra skoncila. Stiskni klavesu pro zavreni...' -n 1 >/dev/null 2>&1; }; run_game" ;;
            wezterm)         "$term" start -- bash -c "run_game() { \"$GAME\"; read -rsp $'Hra skoncila. Stiskni klavesu pro zavreni...' -n 1 >/dev/null 2>&1; }; run_game" ;;
            xterm)           "$term" -e bash -c "run_game() { \"$GAME\"; read -rsp $'Hra skoncila. Stiskni klavesu pro zavreni...' -n 1 >/dev/null 2>&1; }; run_game" ;;
        esac
        exit 0
    fi
done

echo "Nenasel jsem terminal emulator, spoustim hru naprimo:"
"$GAME"
EOF
chmod +x "$LINUX_DIR/spustit.sh"

# Launcher pro Windows: po dvojkliku na .exe otevre konzoli a podrzi ji otevrenou
cat > "$WINDOWS_DIR/spustit.bat" <<'EOF'
@echo off
start "" "%~dp0fishing-cs-revived.exe"
EOF

echo ""
echo "Hotovo! Vysledky:"
echo "  $LINUX_DIR/spustit.sh   (Linux - spusti hru v terminalu)"
echo "  $LINUX_DIR/fishing-cs-revived   (Linux)"
echo "  $WINDOWS_DIR/spustit.bat   (Windows)"
echo "  $WINDOWS_DIR/fishing-cs-revived.exe   (Windows)"
