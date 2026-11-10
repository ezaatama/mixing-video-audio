# Builds GabungAudioVideo-windows.zip (app only; users install ffmpeg themselves, so no GPL redistribution).
# Usage: powershell -ExecutionPolicy Bypass -File release.ps1
$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot

& .\build.bat
if ($LASTEXITCODE -ne 0) { throw 'Build gagal' }

Remove-Item dist -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory dist | Out-Null
Copy-Item GabungAudioVideo.exe, README.md, LICENSE dist
Compress-Archive dist\* GabungAudioVideo-windows.zip -Force
Write-Host 'Selesai: GabungAudioVideo-windows.zip'
