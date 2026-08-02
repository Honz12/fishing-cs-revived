@echo off
REM Sestavi hru pro Windows i Linux do slozky build\ (prepise stary obsah).
REM Vysledky jsou samostatne binarky, ktere nepotrebuji nainstalovany .NET.
setlocal

cd /d "%~dp0.."

set "PROJECT=fishing-cs-revived.csproj"
set "BUILD_DIR=build"
set "LINUX_DIR=%BUILD_DIR%\linux"
set "WINDOWS_DIR=%BUILD_DIR%\windows"

where dotnet >nul 2>nul
if errorlevel 1 (
    if exist "%ProgramFiles%\dotnet\dotnet.exe" (
        set "PATH=%ProgramFiles%\dotnet;%PATH%"
    ) else if exist "%LOCALAPPDATA%\Microsoft\dotnet\dotnet.exe" (
        set "PATH=%LOCALAPPDATA%\Microsoft\dotnet;%PATH%"
    ) else (
        echo Chyba: prikaz 'dotnet' nebyl nalezen. Nainstaluj .NET SDK 10.0.
        exit /b 1
    )
)

if exist "%BUILD_DIR%" (
    echo Mazu stary obsah slozky %BUILD_DIR% ...
    rmdir /s /q "%BUILD_DIR%"
)

echo === Sestavuji Windows (win-x64) ===
dotnet publish "%PROJECT%" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -p:IncludeAllContentForSelfExtract=true -o "%WINDOWS_DIR%"

echo === Sestavuji Linux (linux-x64) ===
dotnet publish "%PROJECT%" -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -p:IncludeAllContentForSelfExtract=true -o "%LINUX_DIR%"

echo === Generuji launchery ===
> "%WINDOWS_DIR%\spustit.bat" echo @echo off
>> "%WINDOWS_DIR%\spustit.bat" echo start "" "%%~dp0fishing-cs-revived.exe"

> "%LINUX_DIR%\spustit.sh" echo #!/bin/bash
>> "%LINUX_DIR%\spustit.sh" echo cd "$(dirname "$0")"
>> "%LINUX_DIR%\spustit.sh" echo GAME="$(pwd)/fishing-cs-revived"
>> "%LINUX_DIR%\spustit.sh" echo for term in gnome-terminal konsole xfce4-terminal x-terminal-emulator tilix kitty alacritty wezterm xterm; do
>> "%LINUX_DIR%\spustit.sh" echo   if command -v "$term" >/dev/null 2>&1; then
>> "%LINUX_DIR%\spustit.sh" echo     "$term" -e bash -c "\"$GAME\"; read -rsp $'Hra skoncila. Stiskni klavesu pro zavreni...' -n 1 >/dev/null 2>&1" 2>/dev/null || "$term" -- bash -c "\"$GAME\"; read -rsp $'Hra skoncila. Stiskni klavesu pro zavreni...' -n 1 >/dev/null 2>&1"
>> "%LINUX_DIR%\spustit.sh" echo     exit 0
>> "%LINUX_DIR%\spustit.sh" echo   fi
>> "%LINUX_DIR%\spustit.sh" echo done
>> "%LINUX_DIR%\spustit.sh" echo echo "Nenasel jsem terminal, spoustim naprimo:"
>> "%LINUX_DIR%\spustit.sh" echo "$GAME"

echo.
echo Hotovo! Vysledky:
echo   %WINDOWS_DIR%\spustit.bat   ^(Windows - spusti hru^)
echo   %WINDOWS_DIR%\fishing-cs-revived.exe   ^(Windows^)
echo   %LINUX_DIR%\spustit.sh   ^(Linux^)
echo   %LINUX_DIR%\fishing-cs-revived   ^(Linux^)

endlocal
