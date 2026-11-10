# Gabung Audio Video

Aplikasi Windows sederhana untuk menggabungkan voice over (MP3/WAV/M4A) ke dalam video (MP4/MOV/MKV).

- **Ganti audio**: suara asli video diganti dengan voice over.
- **Campur**: suara asli tetap ada (volume 30%), voice over di atasnya.
- Gambar video tidak di-encode ulang, jadi cepat dan kualitas tetap.
- Panjang hasil selalu sama dengan panjang video.

## Cara pakai

1. Install FFmpeg (sekali saja). Buka Command Prompt, jalankan:
   ```
   winget install Gyan.FFmpeg
   ```
   Atau unduh manual dari [gyan.dev](https://www.gyan.dev/ffmpeg/builds/) dan taruh `ffmpeg.exe` di folder yang sama dengan aplikasi.
2. Unduh `GabungAudioVideo-windows.zip` dari halaman [Releases](https://github.com/ezaatama/mixing-video-audio/releases), lalu ekstrak. Unduh hanya dari halaman ini agar terhindar dari salinan palsu.
3. Jalankan `GabungAudioVideo.exe`, pilih video dan voice over, lalu klik **Gabungkan & Simpan**.

Butuh Windows 10/11 (.NET Framework 4.x sudah bawaan). Jika muncul "Windows protected your PC", klik **More info → Run anyway** (aplikasi belum ditandatangani digital).

## Build dari source

Tidak perlu install Visual Studio. Jalankan:

```
build.bat
```

Untuk membuat zip rilis:

```
powershell -ExecutionPolicy Bypass -File release.ps1
```

## Lisensi

Kode aplikasi ini berlisensi MIT (lihat `LICENSE`).

Aplikasi ini memanggil [FFmpeg](https://ffmpeg.org) sebagai program terpisah. FFmpeg tidak disertakan dan dipasang sendiri oleh pengguna, dengan lisensinya sendiri (LGPL/GPL).
