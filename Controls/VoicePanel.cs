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
        private readonly Color COL_TEXT_MAIN = Color.FromArgb(220, 221, 222);
        private readonly Color COL_TEXT_MUTED = Color.FromArgb(120, 124, 130);
        private readonly Color COL_GREEN = Color.FromArgb(59, 165, 93);

        private bool _isMuted = false;
        private Panel _participantList;  
        private MaterialLabel _memberCount;

        private Label _myMicLbl = null; //본인
        private Panel _myDot = null;
        private Panel _myRow = null;
        private MaterialButton btnMic;

        public event System.Action OnLeaveClick;

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


            // ── 참여자 리스트 영역 ──
            _participantList = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = COL_BG,
                AutoScroll = true
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

                if (_myRow != null)
                {
                    _participantList.Controls.Remove(_myRow);
                    _myRow = null;
                    _myMicLbl = null;
                    _myDot = null;
                    _memberCount.Text = $"👥 {_participantList.Controls.Count}명";
                }

                OnLeaveClick?.Invoke();
            };

            bottomBar.Controls.Add(btnMic);
            bottomBar.Controls.Add(btnLeave);

            // 순서: Bottom → Fill → Top
            this.Controls.Add(_participantList);
            this.Controls.Add(bottomBar);
            this.Controls.Add(topBar);
        }

        // 참여자 행 추가
        public void AddParticipant(string name, bool isSpeaking)
        {


            var row = new Panel
            {
                Height = 48,
                BackColor = COL_ROW,
                Margin = new Padding(0),
                Dock = DockStyle.Top,
                Tag = name,
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
            _participantList.Controls.SetChildIndex(row, 0);

            if (_myMicLbl == null)
            {
                _myMicLbl = micLbl;
                _myDot = statusDot;
                _myRow = row;
            }

            _memberCount.Text = $"👥 {_participantList.Controls.Count}명";
        }

        // 참여자 행 제거
        public void RemoveParticipant(string name)
        {
            foreach (Control c in _participantList.Controls)
            {
                if (c is Panel row && row.Tag as string == name)
                {
                    _participantList.Controls.Remove(row);
                    break;
                }
            }
            _memberCount.Text = $"👥 {_participantList.Controls.Count}명";
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