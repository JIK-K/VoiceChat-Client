using System.Drawing;
using System.Windows.Forms;

namespace VoiceChat.Forms
{
    public class ChatPanel : Panel
    {
        private readonly Color COL_BG = Color.FromArgb(54, 57, 63);

        public ChatPanel()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = COL_BG;

            // 나중에 채팅 UI 추가할 자리
            this.Controls.Add(new Label
            {
                Text = "채팅방",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 14f),
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            });
        }
    }
}