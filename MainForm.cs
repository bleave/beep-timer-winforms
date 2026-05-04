using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace BeepTimerWinForms
{
    public partial class MainForm : Form
    {
        private Timer tickTimer;
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;
        private DateTime nextBeepTime;
        private int intervalSeconds = 30;
        private bool useSoundFile = false;
        private string soundFilePath = "beep.wav";

        public MainForm()
        {
            InitializeComponent();
            InitializeTray();
            StartPosition = FormStartPosition.CenterScreen;
            TopMost = false;
        }

        private void InitializeTray()
        {
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Show", null, (s, e) => Show());
            trayMenu.Items.Add("Exit", null, (s, e) => Application.Exit());

            trayIcon = new NotifyIcon
            {
                Text = "Beep Timer",
                Icon = SystemIcons.Information,
                ContextMenuStrip = trayMenu,
                Visible = true
            };

            trayIcon.DoubleClick += (s, e) => Show();
        }

        private void InitializeComponent()
        {
            this.Text = "Beep Timer";
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Size = new Size(260, 120);

            Label lblInterval = new Label() { Text = "Interval (s):", Location = new Point(10, 10), AutoSize = true };
            NumericUpDown numInterval = new NumericUpDown() { Location = new Point(80, 8), Width = 60, Minimum = 1, Value = 30 };
            Button btnStart = new Button() { Text = "Start", Location = new Point(150, 6), Width = 80 };
            Label lblCountdown = new Label() { Text = "Countdown: --", Location = new Point(10, 40), Width = 220 };
            CheckBox chkTop = new CheckBox() { Text = "Always on top", Location = new Point(10, 65) };
            CheckBox chkSound = new CheckBox() { Text = "Use sound file", Location = new Point(120, 65) };

            tickTimer = new Timer { Interval = 250 };
            tickTimer.Tick += (s, e) =>
            {
                var now = DateTime.Now;
                int remaining = (int)(nextBeepTime - now).TotalSeconds;
                lblCountdown.Text = $"Countdown: {Math.Max(0, remaining)}";

                if (remaining <= 0)
                {
                    if (useSoundFile && System.IO.File.Exists(soundFilePath))
                    {
                        SoundPlayer player = new SoundPlayer(soundFilePath);
                        player.Play();
                    }
                    else
                    {
                        Console.Beep();
                    }

                    nextBeepTime = DateTime.Now.AddSeconds(intervalSeconds);
                }
            };

            btnStart.Click += (s, e) =>
            {
                intervalSeconds = (int)numInterval.Value;
                nextBeepTime = DateTime.Now.AddSeconds(intervalSeconds - (DateTime.Now.Second % intervalSeconds));
                tickTimer.Start();
            };

            chkTop.CheckedChanged += (s, e) => this.TopMost = chkTop.Checked;
            chkSound.CheckedChanged += (s, e) => useSoundFile = chkSound.Checked;

            this.Controls.Add(lblInterval);
            this.Controls.Add(numInterval);
            this.Controls.Add(btnStart);
            this.Controls.Add(lblCountdown);
            this.Controls.Add(chkTop);
            this.Controls.Add(chkSound);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}