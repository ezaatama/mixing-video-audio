# Builds GabungAudioVideo-windows.zip: app + ffmpeg.exe + ffmpeg's GPL license and source link.
# Usage: powershell -ExecutionPolicy Bypass -File release.ps1 [-Ffmpeg C:\path\to\ffmpeg.exe]
param([string]$Ffmpeg = (Get-Command ffmpeg -ErrorAction Stop).Source)
$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot

& .\build.bat
if ($LASTEXITCODE -ne 0) { throw 'Build gagal' }

$real = (Get-Item $Ffmpeg).Target  # winget installs ffmpeg as a symlink
if (-not $real) { $real = $Ffmpeg }
$ffDir = Split-Path (Split-Path $real)  # gyan.dev layout: <dir>\bin\ffmpeg.exe, LICENSE + README.txt in <dir>

Remove-Item dist -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory dist | Out-Null
Copy-Item GabungAudioVideo.exe, README.md, LICENSE dist
Copy-Item $real dist\ffmpeg.exe
# GPL requires shipping the license and pointing to the source with the binary
foreach ($f in 'LICENSE', 'README.txt') {
    if (-not (Test-Path "$ffDir\$f")) { throw "Tidak ada $ffDir\$f - lisensi ffmpeg wajib ikut dibagikan" }
    Copy-Item "$ffDir\$f" "dist\FFMPEG-$f"
}

Compress-Archive dist\* GabungAudioVideo-windows.zip -Force
Write-Host 'Selesai: GabungAudioVideo-windows.zip'
