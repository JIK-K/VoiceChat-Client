
using System;
using System.Drawing;
using System.Windows.Forms;

namespace VoiceChat.Forms
{
    public class ServerListPanel : Panel
    {
        private readonly Color COL_BG = Color.FromArgb(30, 31, 35);
        private readonly Color COL_ICON_BG = Color.FromArgb(71, 82, 100);
        private Button _selected = null;
        private Panel _indicator = null;
        private int _nextY = 8;       // 다음 버튼이 추가될 Y 위치
        private int _btnSize = 48;
        private int _gap = 4;
        private int _x = 12;
        public ServerListPanel()
        {
            this.Width = 72;
            this.Dock = DockStyle.Left;
            this.BackColor = COL_BG;
            BuildFixedButtons();
        }
        // 고정 버튼 (홈 + 구분선 + ➕) 만 먼저 세팅
        private void BuildFixedButtons()
        {
            // 홈 버튼
            var home = MakeButton("🏠", _x, _nextY, COL_ICON_BG,
                                  AnchorStyles.Top | AnchorStyles.Left);
            home.Click += (s, e) => Select(home);
            this.Controls.Add(home);
            _nextY += _btnSize + _gap;
            // 구분선
            this.Controls.Add(new Panel
            {
                Width = 34,
                Height = 2,
                Left = 20,
                Top = _nextY,
                BackColor = COL_ICON_BG,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            });
            _nextY += 10;
            // ➕ 버튼 (하단 고정)
            var addBtn = MakeButton("➕", _x, 0, COL_ICON_BG,
                                    AnchorStyles.Bottom | AnchorStyles.Left);
            addBtn.Top = this.Height - _btnSize - 8;
            addBtn.Click += (s, e) => Select(addBtn);
            this.Controls.Add(addBtn);
            Select(home);
        }
        // 외부에서 호출해서 그룹 추가
        public void AddChannel(string name)
        {
            // 이름 첫 글자 표시
            string icon = name.Length > 0 ? name[0].ToString() : "?";
            var btn = MakeButton(icon, _x, _nextY, COL_ICON_BG,
                                 AnchorStyles.Top | AnchorStyles.Left);
            btn.Click += (s, e) => Select(btn);
            this.Controls.Add(btn);
            _nextY += _btnSize + _gap;
        }
        private void Select(Button btn)
        {
            if (_indicator != null)
            {
                this.Controls.Remove(_indicator);
                _indicator.Dispose();
                _indicator = null;
            }
            _selected = btn;
            _indicator = new Panel
            {
                Width = 4,
                Height = 32,
                Left = 0,
                Top = btn.Top + (_btnSize / 2) - 16,
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            this.Controls.Add(_indicator);
            _indicator.BringToFront();
        }
        private Button MakeButton(string text, int left, int top,
                                   Color backColor, AnchorStyles anchor)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI Emoji", 16),
                ForeColor = Color.White,
                BackColor = backColor,
                FlatStyle = FlatStyle.Flat,
                Width = _btnSize,
                Height = _btnSize,
                Left = left,
                Top = top,
                Anchor = anchor,
                FlatAppearance = { BorderSize = 0 }
            };
            btn.UseVisualStyleBackColor = false;
            return btn;
        }
    }
}

