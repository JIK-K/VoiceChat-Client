using MaterialSkin;
using MaterialSkin.Controls;
using System.Windows.Forms;

namespace VoiceChat.Forms
{
    public partial class RoomForm : MaterialForm
    {
        private Panel _contentArea;   // 오른쪽 콘텐츠 영역
        private ChatPanel _chatPanel;
        private VoicePanel _voicePanel;

        public RoomForm()
        {
            InitializeComponent();
            InitializeMaterialSkin();
            //InitializeLayout();
        }

        private void InitializeMaterialSkin()
        {
            var skin = MaterialSkinManager.Instance;
            skin.AddFormToManage(this);
            skin.ColorScheme = new ColorScheme(
                Primary.BlueGrey900, Primary.BlueGrey900,
                Primary.BlueGrey500, Accent.LightBlue200,
                TextShade.WHITE
            );
        }

        private void InitializeLayout()
        {
            // ── 콘텐츠 패널 ──
            _contentArea = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.FromArgb(54, 57, 63)
            };

            // ── 패널 미리 생성 ──
            _chatPanel = new ChatPanel();
            _voicePanel = new VoicePanel();

            // 기본은 숨김
            _chatPanel.Visible = false;
            _voicePanel.Visible = false;

            _contentArea.Controls.Add(_chatPanel);
            _contentArea.Controls.Add(_voicePanel);

        
            var channelList = new ChannelListPanel();
            channelList.OnChannelSelected += OnChannelSelected;

    
            this.Controls.Add(_contentArea);
            this.Controls.Add(channelList);
        }

        // 채널 클릭 시 호출
        private void OnChannelSelected(string channelName, bool isVoice)
        {
            // 둘 다 숨기고
            _chatPanel.Visible = false;
            _voicePanel.Visible = false;

            // 해당 패널만 표시
            if (isVoice)
                _voicePanel.Visible = true;
            else
                _chatPanel.Visible = true;
        }

        private void RoomForm_Load(object sender, System.EventArgs e) { }
    }
}