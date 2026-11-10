# Gabung Audio Video

Aplikasi Windows sederhana untuk menggabungkan voice over (MP3/WAV/M4A) ke dalam video (MP4/MOV/MKV).

- **Ganti audio**: suara asli video diganti dengan voice over.
- **Campur**: suara asli tetap ada (volume 30%), voice over di atasnya.
- Gambar video tidak di-encode ulang, jadi cepat dan kualitas tetap.
- Panjang hasil selalu sama dengan panjang video.

## Cara pakai

1. Unduh `GabungAudioVideo-windows.zip` dari halaman [Releases](https://github.com/ezaatama/mixing-video-audio/releases), lalu ekstrak. Unduh hanya dari halaman ini agar terhindar dari salinan palsu.
2. Jalankan `GabungAudioVideo.exe` (`ffmpeg.exe` harus ada di folder yang sama).
3. Pilih video dan voice over, lalu klik **Gabungkan & Simpan**.

Butuh Windows 10/11 (.NET Framework 4.x sudah bawaan).

## Build dari source

Tidak perlu install Visual Studio. Jalankan:

```
build.bat
```

Untuk membuat zip rilis (perlu ffmpeg terpasang, misalnya `winget install Gyan.FFmpeg`):

```
powershell -ExecutionPolicy Bypass -File release.ps1
```

## Lisensi

Kode aplikasi ini berlisensi MIT (lihat `LICENSE`).

File rilis menyertakan `ffmpeg.exe` dari [gyan.dev](https://www.gyan.dev/ffmpeg/builds/), yang berlisensi **GPL v3**. FFmpeg adalah program terpisah yang dipanggil oleh aplikasi ini. Lisensi dan link source code-nya ada di `FFMPEG-LICENSE` dan `FFMPEG-README.txt`.
