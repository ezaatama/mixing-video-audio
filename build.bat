@echo off
rem Uses the C# compiler that ships with every Windows 10/11 (.NET Framework 4.x) - nothing to install.
cd /d "%~dp0"
"%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /target:winexe /out:GabungAudioVideo.exe /r:System.Windows.Forms.dll /r:System.Drawing.dll GabungAudioVideo.cs
