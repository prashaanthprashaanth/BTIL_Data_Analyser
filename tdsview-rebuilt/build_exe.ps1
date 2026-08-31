[CmdletBinding()]
param(
    [switch]$SkipTests
)

$ErrorActionPreference = "Stop"
$projectDirectory = $PSScriptRoot
$executablePath = Join-Path $projectDirectory "dist\TDSView.exe"

Push-Location $projectDirectory
try {
    & py -3 -c "import openpyxl, PyInstaller"
    if ($LASTEXITCODE -ne 0) {
        throw "Build dependencies are missing. Run: py -3 -m pip install -r requirements.txt -r requirements-build.txt"
    }

    if (-not $SkipTests) {
        & py -3 -m unittest discover -s tests -v
        if ($LASTEXITCODE -ne 0) {
            throw "The test suite failed; the executable was not built."
        }
    }

    & py -3 -m PyInstaller `
        --noconfirm `
        --clean `
        --onefile `
        --windowed `
        --noupx `
        --name TDSView `
        --version-file (Join-Path $projectDirectory "version_info.txt") `
        --distpath (Join-Path $projectDirectory "dist") `
        --workpath (Join-Path $projectDirectory "build\work") `
        --specpath (Join-Path $projectDirectory "build") `
        (Join-Path $projectDirectory "app.py")
    if ($LASTEXITCODE -ne 0) {
        throw "PyInstaller failed with exit code $LASTEXITCODE."
    }

    if (-not (Test-Path -LiteralPath $executablePath -PathType Leaf)) {
        throw "Build completed without producing $executablePath"
    }

    $artifact = Get-Item -LiteralPath $executablePath
    $hash = Get-FileHash -LiteralPath $executablePath -Algorithm SHA256
    Write-Host ""
    Write-Host "Executable: $($artifact.FullName)"
    Write-Host "Size:       $([math]::Round($artifact.Length / 1MB, 2)) MB"
    Write-Host "SHA-256:    $($hash.Hash)"
}
finally {
    Pop-Location
}
