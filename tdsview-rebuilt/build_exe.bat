@echo off
setlocal
powershell.exe -NoLogo -NoProfile -ExecutionPolicy Bypass -File "%~dp0build_exe.ps1" %*
if errorlevel 1 (
    echo.
    echo The executable build failed.
    pause
    exit /b 1
)
echo.
echo Build completed successfully.
pause
