using System.Drawing;
using System.Windows.Forms;
using MaterialSkin.Controls;

namespace VoiceChat.Forms
{
    public class ChannelListPanel : Panel
    {
        private readonly Color COL_BG = Color.FromArgb(43, 45, 49);

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
            var lbl = new MaterialLabel
            {
                Text = title + " >",
                Left = 8,
                Top = y,
                Width = 184,
                Height = 24,
                AutoSize = false
            };
            this.Controls.Add(lbl);
            return y + 28;
        }

        private int AddChannel(string name, int y, bool isVoice)
        {
            var btn = new MaterialButton
            {
                Text = name,
                Left = 4,
                Top = y,
                Width = 192,
                Height = 36,
                AutoSize = false,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Type = MaterialButton.MaterialButtonType.Text,  // 텍스트 스타일
                UseAccentColor = false,
                HighEmphasis = false,
                //NoAccentTextColor = true
            };

            btn.Click += (s, e) => OnChannelSelected?.Invoke(name, isVoice);

            this.Controls.Add(btn);
            return y + 40;
        }
    }
}