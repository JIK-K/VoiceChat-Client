using System.Drawing;
using System.Windows.Forms;
using MaterialSkin.Controls;

namespace VoiceChat.Forms
{
    public class VoicePanel : Panel
    {
        private readonly Color COL_BG = Color.FromArgb(54, 57, 63);
        private readonly Color COL_TOP_BAR = Color.FromArgb(40, 43, 48);
        private readonly Color COL_BOTTOM_BAR = Color.FromArgb(40, 43, 48);
        private readonly Color COL_ROW = Color.FromArgb(47, 49, 54);
        private readonly Color COL_ROW_HOVER = Color.FromArgb(60, 63, 70);
        private readonly Color COL_TEXT_MAIN = Color.FromArgb(220, 221, 222);
        private readonly Color COL_TEXT_MUTED = Color.FromArgb(120, 124, 130);
        private readonly Color COL_GREEN = Color.FromArgb(59, 165, 93);

        private bool _isMuted = false;
        private Panel _participantList;  
        private MaterialLabel _memberCount;

        private Label _myMicLbl = null; //본인
        private Panel _myDot = null;
        private MaterialButton btnMic;


        public VoicePanel()
        {
            this.Dock = DockStyle.Fill;

            Build();
        }

        private void Build()
        {
            // ── 상단 바 ──
            var topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 48,
               
            };

            var topLabel = new MaterialLabel
            {
                Text = "🔊 일반1",
                Left = 16,
                Top = 0,
                Width = 200,
                Height = 48,
                FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle1
            };

            // 인원수 표시
            _memberCount = new MaterialLabel
            {
                Text = "👥 0명",
                Left = 220,
                Top = 0,
                Width = 100,
                Height = 48,
                FontType = MaterialSkin.MaterialSkinManager.fontType.Body1
            };

            topBar.Controls.Add(topLabel);
            topBar.Controls.Add(_memberCount);

            // ── 테스트용 입력 바 ──
            var testBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = COL_TOP_BAR,
                Padding = new Padding(8, 8, 8, 8)
            };

            var inputName = new MaterialTextBox
            {
                Hint = "이름 입력 (테스트용)",
                Left = 8,
                Top = 8,
                Width = 160,
                Height = 36
            };

            var btnAdd = new MaterialButton
            {
                Text = "입장",
                Left = 176,
                Top = 8,
                Width = 80,
                Height = 36,
                AutoSize = false,
                Type = MaterialButton.MaterialButtonType.Contained
            };

            testBar.Controls.Add(inputName);
            testBar.Controls.Add(btnAdd);

            // ── 참여자 리스트 영역 ──
            _participantList = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = COL_BG,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0),
            };

            // 입장 버튼 클릭
            btnAdd.Click += (s, e) =>
            {
                string name = inputName.Text.Trim();
                if (string.IsNullOrEmpty(name)) return;

                AddParticipant(name, isSpeaking: false);
                inputName.Text = "";
            };

            // ── 하단 바 ──
            var bottomBar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 64,
                BackColor = COL_BOTTOM_BAR,
                Padding = new Padding(16, 12, 16, 12)
            };

            btnMic = new MaterialButton
            {
                Text = "🎙 마이크",
                Left = 16,
                Top = 12,
                Width = 110,
                Height = 36,
                AutoSize = false,
                Type = MaterialButton.MaterialButtonType.Outlined
            };
            btnMic.Click += OnMicClick;

            var btnLeave = new MaterialButton
            {
                Text = "📞 나가기",
                Left = 134,
                Top = 12,
                Width = 110,
                Height = 36,
                AutoSize = false,
                Type = MaterialButton.MaterialButtonType.Contained
            };
            btnLeave.Click += (s, e) =>
            {
                // 나중에 서버 퇴장 로직
            };

            bottomBar.Controls.Add(btnMic);
            bottomBar.Controls.Add(btnLeave);

            // 순서: Bottom → Fill → testBar → Top
            this.Controls.Add(_participantList);
            this.Controls.Add(bottomBar);
            this.Controls.Add(testBar);
            this.Controls.Add(topBar);
        }

        // 참여자 행 추가
        public void AddParticipant(string name, bool isSpeaking)
        {
        

            // row
            var row = new Panel
            {
                Width = _participantList.ClientSize.Width,
                Height = 48,
                BackColor = COL_ROW
            };
            _participantList.SizeChanged += (s, e) =>
            {
                row.Width = _participantList.ClientSize.Width;
            };
            var statusDot = new Panel
            {
                Width = 12,
                Height = 12,
                Left = 12,
                Top = 18,
                BackColor = isSpeaking ? COL_GREEN : COL_TEXT_MUTED
            };

            var nameLbl = new Label
            {
                Text = name,
                Font = new Font("Segoe UI", 10f),
                ForeColor = COL_TEXT_MAIN,
                BackColor = Color.Transparent,
                Left = 36,
                Top = 0,
                Width = 200,
                Height = 48,
                TextAlign = ContentAlignment.MiddleLeft,
                RightToLeft = RightToLeft.No
            };

            var micLbl = new Label
            {
                Text = isSpeaking ? "🎙" : "🔇",
                Font = new Font("Segoe UI Emoji", 12f),
                ForeColor = isSpeaking ? COL_GREEN : COL_TEXT_MUTED,
                BackColor = Color.Transparent,
                Width = 32,
                Height = 48,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Right
            };

            row.Controls.Add(micLbl);
            row.Controls.Add(statusDot);
            row.Controls.Add(nameLbl);

           
      
            _participantList.Controls.Add(row);

            if (_myMicLbl == null)
            {
                _myMicLbl = micLbl;
                _myDot = statusDot;
            }

            _memberCount.Text = $"👥 {_participantList.Controls.Count / 2}명";
        }

        private void OnMicClick(object sender, System.EventArgs e)
        {
            _isMuted = !_isMuted;
            btnMic.Text = _isMuted ? "🎙 마이크" : "🔇 음소거";

            if (_myMicLbl == null) return;  // 아직 아무도 없으면 무시

            _myMicLbl.Text = _isMuted ? "🎙" : "🔇";
            _myMicLbl.ForeColor = _isMuted ? COL_GREEN : COL_TEXT_MUTED;
            _myDot.BackColor = _isMuted ? COL_GREEN : COL_TEXT_MUTED;
        }
    }
}