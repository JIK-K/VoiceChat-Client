using MaterialSkin;
using MaterialSkin.Controls;
using System.Windows.Forms;

namespace VoiceChat.Forms
{
    public partial class RoomForm : MaterialForm
    {
        private MaterialCard _contentArea;   // 오른쪽 콘텐츠 영역
        private ChatPanel _chatPanel;
        private VoicePanel _voicePanel;

        public RoomForm()
        {
            InitializeComponent();
            InitializeMaterialSkin();
            InitializeLayout();
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
            // ── 콘텐츠 영역 (MaterialCard) ──
            _contentArea = new MaterialCard
            {
                Dock = DockStyle.Fill,
                Padding = new System.Windows.Forms.Padding(0)
            };

            _chatPanel = new ChatPanel();
            _voicePanel = new VoicePanel();

            _chatPanel.Visible = false;
            _voicePanel.Visible = false;

            _contentArea.Controls.Add(_chatPanel);
            _contentArea.Controls.Add(_voicePanel);

            // ── 채널 리스트 (왼쪽) ──
            var channelList = new ChannelListPanel();
            channelList.OnChannelSelected += OnChannelSelected;

            // Fill 먼저, Left 나중
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