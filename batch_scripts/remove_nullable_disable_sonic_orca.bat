@echo off
setlocal enabledelayedexpansion

echo Starting removal of "#nullable disable" from C# files...

if not exist "..\SonicOrca" (
    echo Error: SonicOrca folder not found
    echo Please run this script from the root directory of the SonicOrca decompilation
    pause
    exit /b 1
)

set "tempfile=%temp%\temp.cs"

for /r "..\SonicOrca" %%f in (*.cs) do (
    echo Processing: %%f
    set "file=%%f"
    
    findstr /C:"#nullable disable" "!file!" >nul
    if not errorlevel 1 (
        findstr /V /C:"#nullable disable" "!file!" > "%tempfile%"
        
        move /Y "%tempfile%" "!file!" >nul
        echo Removed #nullable disable from: %%f
    )
)

echo.
echo Process completed!
pause 