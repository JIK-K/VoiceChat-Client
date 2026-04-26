using System.Drawing;
using System.Windows.Forms;

namespace VoiceChat.Forms
{
    public class ChannelListPanel : Panel
    {
        private readonly Color COL_BG = Color.FromArgb(43, 45, 49);
        private readonly Color COL_TEXT_MUTED = Color.FromArgb(120, 124, 130);

        public event System.Action<string, bool> OnChannelSelected;

        public ChannelListPanel()
        {
            this.Width = 200;
            this.Dock = DockStyle.Left;
            this.BackColor = COL_BG;
            Build();
        }

        private void Build()
        {
            int y = 8;

            y = AddCategory("채팅 채널", y);
            y = AddChannel("# 채팅방1", y, isVoice: false);

            y += 8;

            y = AddCategory("음성 채널", y);
            y = AddChannel("🔊 일반1", y, isVoice: true);
        }

        private int AddCategory(string title, int y)
        {
            this.Controls.Add(new Label
            {
                Text = title + " >",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = COL_TEXT_MUTED,
                BackColor = COL_BG,
                Left = 8,
                Top = y,
                Width = 184,
                Height = 24,
                TextAlign = ContentAlignment.MiddleLeft,
                RightToLeft = RightToLeft.No
            });
            return y + 28;
        }

        private int AddChannel(string name, int y, bool isVoice)
        {
            var lbl = new Label
            {
                Text = name,
                Font = new Font("Segoe UI", 10f),
                ForeColor = COL_TEXT_MUTED,
                BackColor = COL_BG,
                Left = 12,
                Top = y,
                Width = 184,
                Height = 32,
                TextAlign = ContentAlignment.MiddleLeft,
                RightToLeft = RightToLeft.No,
                Cursor = Cursors.Hand
            };

            lbl.Click += (s, e) => OnChannelSelected?.Invoke(name, isVoice);

            this.Controls.Add(lbl);
            return y + 34;
        }
    }
}