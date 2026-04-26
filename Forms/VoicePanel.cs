using System.Drawing;
using System.Windows.Forms;

namespace VoiceChat.Forms
{
    public class VoicePanel : Panel
    {
        private readonly Color COL_BG = Color.FromArgb(54, 57, 63);

        public VoicePanel()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = COL_BG;

            // 나중에 음성방 UI 추가할 자리
            this.Controls.Add(new Label
            {
                Text = "음성방",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 14f),
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            });
        }
    }
}