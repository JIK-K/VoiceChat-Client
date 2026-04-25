using System;
using System.Drawing;
using System.Windows.Forms;

namespace VoiceChat.Forms
{
    public class ChannelListPanel : Panel
    {
        private readonly Color COL_BG = Color.FromArgb(43, 45, 49);
        private readonly Color COL_SELECTED = Color.FromArgb(80, 85, 95);  //  더 밝게
        private readonly Color COL_TEXT_MAIN = Color.FromArgb(255, 255, 255);  //  완전 흰색
        private readonly Color COL_TEXT_MUTED = Color.FromArgb(100, 105, 110);  //  더 어둡게
        private readonly Color COL_DIVIDER = Color.FromArgb(55, 57, 62);

        private Label _selectedItem = null;

        public ChannelListPanel()
        {
            this.Width = 110;
            this.Dock = DockStyle.Left;
            this.BackColor = COL_BG;
            this.ForeColor = COL_TEXT_MUTED;
            Build();
        }

        private void Build()
        {
            int y = 8;
            y = AddCategory("채팅 채널", y);
            y = AddChannelItem("# 일반", y);
            y = AddDivider(y);
            y = AddCategory("음성 채널", y);
            y = AddChannelItem("🔊 일반", y);
        }

        private int AddCategory(string title, int y)
        {
            this.Controls.Add(new Label
            {
                Text = title.ToUpper(),
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = COL_TEXT_MUTED,
                BackColor = COL_BG,
                Left = 8,
                Top = y,
                Width = 104,
                Height = 24,
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                RightToLeft = RightToLeft.No,
                
            });
            return y + 26;
        }

        private int AddChannelItem(string name, int y)
        {
            var item = new Label
            {
                Text = name,
                Font = new Font("Segoe UI", 10f),
                ForeColor = COL_TEXT_MUTED,
                BackColor = Color.Transparent,
                Left = 8,
                Top = y,
                Width = 104,
                Height = 32,
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                RightToLeft = RightToLeft.No,
            };
            item.Click += (s, e) => Select(item);
            this.Controls.Add(item);
            return y + 34;
        }

        private int AddDivider(int y)
        {
            y += 4;
            this.Controls.Add(new Panel
            {
                Left = 8,
                Top = y,
                Width = 104,
                Height = 1,
                BackColor = COL_DIVIDER,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            });
            return y + 8;
        }

        private void Select(Label item)
        {
            if (_selectedItem != null)
            {
                Console.WriteLine($"해제 전: {_selectedItem.Text} ForeColor = {_selectedItem.ForeColor}");
                _selectedItem.BackColor = Color.Transparent;
                _selectedItem.ForeColor = COL_TEXT_MUTED;
                Console.WriteLine($"해제 후: {_selectedItem.Text} ForeColor = {_selectedItem.ForeColor}");
            }

            _selectedItem = item;
            item.BackColor = COL_SELECTED;
            item.ForeColor = COL_TEXT_MAIN;
        }
    }
}