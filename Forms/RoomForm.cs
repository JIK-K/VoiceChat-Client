using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VoiceChat.Forms
{
    public partial class RoomForm : MaterialForm
    {
        private MaterialCard _contentArea;  
        private ChatPanel _chatPanel;
        private VoicePanel _voicePanel;

        private ITcpManager _tcp;
        private MainForm _mainForm;

        private int _myUserId;
        private int _myRoomId;

        private bool _joined = false;

        public RoomForm(ITcpManager tcp, MainForm mainForm, int myUserId, int myRoomId)
        {
            InitializeComponent();

            _tcp = tcp;
            _mainForm = mainForm;
            _myUserId = myUserId;
            _myRoomId = myRoomId;   

            SubscribeEvents();

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

            _voicePanel.OnLeaveClick += OnLeaveRoom;

            _chatPanel.Visible = false;
            _voicePanel.Visible = false;

            _contentArea.Controls.Add(_chatPanel);
            _contentArea.Controls.Add(_voicePanel);

            // ── 채널 리스트 (왼쪽) ──
            var channelList = new ChannelListPanel();
            channelList.OnChannelSelected += OnChannelSelected;

            this.Controls.Add(_contentArea);
            this.Controls.Add(channelList);
        }

        private void SubscribeEvents()
        {
            _tcp.OnUserListReceived += OnUserListReceived;
            _tcp.OnUserJoined += OnUserJoined;
            _tcp.OnUserLeft += OnUserLeft;
        }

        private void OnUserListReceived(List<int> users)
        {
            Invoke((Action)(() =>
            {
                Console.WriteLine($"[RoomForm] user_list 수신 - {users.Count}명");
                _voicePanel.ClearParticipants(); // 기존 목록 초기화

                foreach (var user in users)
                    _voicePanel.AddParticipant(user.ToString(), isSpeaking: false);
            }));
        }

        private void OnUserJoined(int userId)
        {
            Invoke((Action)(() =>
            {
                Console.WriteLine($"[RoomForm] user_joined: {userId}");
                _voicePanel.AddParticipant(userId.ToString(), isSpeaking: false);
            }));
        }

        private void OnUserLeft(int userId)
        {
            Invoke((Action)(() =>
            {
                Console.WriteLine($"[RoomForm] user_left: {userId}");
                _voicePanel.RemoveParticipant(userId.ToString());
            }));
        }

        private void OnLeaveRoom()
        {
            _tcp.OnUserListReceived -= OnUserListReceived;
            _tcp.OnUserJoined -= OnUserJoined;
            _tcp.OnUserLeft -= OnUserLeft;

            _tcp.LeaveRoom(_myUserId, _myRoomId); // 방 나가기 요청
           // _tcp.RequestRoomList();
            _mainForm.Show();
           
            this.Close();
        }

        private void OnChannelSelected(string channelName, bool isVoice)
        {
            _chatPanel.Visible = false;
            _voicePanel.Visible = false;

            if (isVoice)
            {
                if (!_joined)
                {
                    _voicePanel.AddParticipant(_myUserId.ToString(), isSpeaking: false); // 나 자신
                                                                             
                   
                    _joined = true;
                }
                _voicePanel.Visible = true;
            }
            else
                _chatPanel.Visible = true;
        }
        private void RoomForm_Load(object sender, System.EventArgs e) { }
    }
}