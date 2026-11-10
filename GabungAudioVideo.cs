using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class MainForm : Form
{
    TextBox txtVideo = new TextBox(), txtAudio = new TextBox();
    CheckBox chkMix = new CheckBox();
    Button btnGo = new Button();
    Label lblStatus = new Label();

    MainForm()
    {
        Text = "Gabung Voice Over ke Video";
        ClientSize = new Size(520, 190);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Font = new Font("Segoe UI", 9);

        AddRow("Video (MP4):", txtVideo, 15, "Video|*.mp4;*.mov;*.mkv;*.avi|Semua file|*.*");
        AddRow("Voice over (MP3):", txtAudio, 50, "Audio|*.mp3;*.wav;*.m4a;*.aac|Semua file|*.*");

        chkMix.Text = "Pertahankan audio asli video (volume dikecilkan jadi 30%)";
        chkMix.SetBounds(15, 88, 490, 22);
        Controls.Add(chkMix);

        btnGo.Text = "Gabungkan && Simpan...";
        btnGo.SetBounds(15, 118, 180, 32);
        btnGo.Click += async (s, e) => await Merge();
        Controls.Add(btnGo);

        lblStatus.SetBounds(15, 158, 490, 22);
        Controls.Add(lblStatus);
    }

    void AddRow(string label, TextBox box, int y, string filter)
    {
        var lbl = new Label { Text = label };
        lbl.SetBounds(15, y + 3, 110, 20);
        box.SetBounds(125, y, 300, 23);
        var btn = new Button { Text = "Pilih..." };
        btn.SetBounds(430, y - 1, 75, 25);
        btn.Click += (s, e) =>
        {
            using (var d = new OpenFileDialog { Filter = filter })
                if (d.ShowDialog() == DialogResult.OK) box.Text = d.FileName;
        };
        Controls.AddRange(new Control[] { lbl, box, btn });
    }

    // ffmpeg.exe next to this app wins, otherwise whatever is on PATH
    static string FindFfmpeg()
    {
        string local = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe");
        return File.Exists(local) ? local : "ffmpeg";
    }

    internal static string BuildArgs(string video, string audio, string output, bool mix)
    {
        string audioPart = mix
            // normalize=0 stops amix halving the voice volume; -shortest below trims to video length
            ? "-filter_complex \"[0:a]volume=0.3[bg];[1:a][bg]amix=inputs=2:duration=longest:normalize=0[a]\" -map 0:v -map \"[a]\""
            // apad + -shortest: output is always exactly the video's length
            : "-map 0:v -map 1:a -af apad";
        return string.Format("-y -i \"{0}\" -i \"{1}\" {2} -c:v copy -c:a aac -b:a 192k -shortest \"{3}\"",
            video, audio, audioPart, output);
    }

    async Task Merge()
    {
        if (!File.Exists(txtVideo.Text) || !File.Exists(txtAudio.Text))
        {
            MessageBox.Show("Pilih file video dan voice over dulu.", Text);
            return;
        }
        string output;
        using (var d = new SaveFileDialog
        {
            Filter = "MP4|*.mp4",
            FileName = Path.GetFileNameWithoutExtension(txtVideo.Text) + "_voiceover.mp4",
            InitialDirectory = Path.GetDirectoryName(txtVideo.Text)
        })
        {
            if (d.ShowDialog() != DialogResult.OK) return;
            output = d.FileName;
        }
        if (string.Equals(Path.GetFullPath(output), Path.GetFullPath(txtVideo.Text), StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show("File hasil tidak boleh menimpa video asli.", Text);
            return;
        }

        btnGo.Enabled = false;
        lblStatus.Text = "Memproses...";
        var log = new StringBuilder();
        int exit;
        try
        {
            var psi = new ProcessStartInfo(FindFfmpeg(), BuildArgs(txtVideo.Text, txtAudio.Text, output, chkMix.Checked))
            {
                UseShellExecute = false, CreateNoWindow = true, RedirectStandardError = true
            };
            exit = await Task.Run(() =>
            {
                using (var p = Process.Start(psi))
                {
                    p.ErrorDataReceived += (s, e) => { if (e.Data != null) lock (log) log.AppendLine(e.Data); };
                    p.BeginErrorReadLine();
                    p.WaitForExit();
                    return p.ExitCode;
                }
            });
        }
        catch (Exception ex)
        {
            lblStatus.Text = "";
            btnGo.Enabled = true;
            MessageBox.Show("ffmpeg tidak ditemukan. Taruh ffmpeg.exe di folder yang sama dengan aplikasi ini.\n\n" + ex.Message, Text);
            return;
        }
        btnGo.Enabled = true;
        if (exit == 0)
        {
            lblStatus.Text = "Selesai: " + output;
            Process.Start("explorer.exe", "/select,\"" + output + "\"");
        }
        else
        {
            lblStatus.Text = "Gagal.";
            string tail = log.ToString();
            if (tail.Length > 1500) tail = tail.Substring(tail.Length - 1500);
            MessageBox.Show("ffmpeg gagal (kode " + exit + "):\n\n" + tail, Text);
        }
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new MainForm());
    }
}
