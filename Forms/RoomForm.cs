using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VoiceChat.Audio;
using VoiceChat.NetWork;
using VoiceChat.protocol;

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
        // ── 음성 채팅 관련 필드 ──
        private UdpManager _udp;
        private AudioCapture _capture;
        // 다른 사용자별 재생기 관리 (UserId -> (JitterBuffer, AudioPlayback))
        private Dictionary<int, (JitterBuffer jitter, AudioPlayback playback)> _remotePlayers 
            = new Dictionary<int, (JitterBuffer, AudioPlayback)>();

        private bool _isVoiceConnected = false;

        public RoomForm(ITcpManager tcp, MainForm mainForm, int myUserId, int myRoomId)
        {
            InitializeComponent();

            _tcp = tcp;
            _mainForm = mainForm;
            _myUserId = myUserId;
            _myRoomId = myRoomId;   

            InitializeMaterialSkin();
            InitializeLayout();

            SubscribeEvents();

            // 1. 핸들이 생성된 후 TCP Join 호출 (Invoke 오류 방지)
            this.HandleCreated += (s, e) => {
                _tcp.JoinRoom(_myUserId, _myRoomId);
            };

            // 2. UDP 및 캡처 객체 초기화
            _udp = new UdpManager(Environment.GetEnvironmentVariable("SERVER_IP"), 9001); // UDP 포트는 9001로 가정
            _udp.OnVoiceReceived = OnRemoteVoiceReceived;

            _capture = new AudioCapture();
            _capture.UserId = _myUserId;
            _capture.RoomId = _myRoomId;
            _capture.OnPacketReady = (packet) => _udp.Send(packet);
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

            _voicePanel.OnMicToggle += OnMicToggle;
        }

        private void OnMicToggle(bool isMuted)
        {
            if (isMuted)
                _capture.Stop();
            else
                _capture.Start();
        }

        // 다른 사용자의 음성 패킷을 받았을 때
        private void OnRemoteVoiceReceived(PacketHeader header, byte[] opusData)
        {
            if (header.UserId == _myUserId) return; // 내 목소리 에코 방지

            if (!_remotePlayers.TryGetValue(header.UserId, out var player))
            {
                // 새로운 사용자의 재생기 생성
                var jitter = new JitterBuffer();
                var playback = new AudioPlayback();
                
                jitter.OnPacketReady = (data) => playback.PlayOpus(data);
                playback.Start();

                player = (jitter, playback);
                _remotePlayers[header.UserId] = player;
            }

            player.jitter.Push(header, opusData);
        }

        private void OnUserListReceived(List<int> users)
        {
            Invoke((Action)(() =>
            {
                Console.WriteLine($"[RoomForm] user_list 수신 - {users.Count}명");
                _voicePanel.ClearParticipants(); // 기존 목록 초기화

                // 1. 나 자신 먼저 추가
                _voicePanel.AddParticipant(_myUserId.ToString(), isSpeaking: false);

                // 2. 서버 목록 중 나를 제외한 나머지 추가
                foreach (var user in users)
                {
                    if (user == _myUserId) continue;
                    _voicePanel.AddParticipant(user.ToString(), isSpeaking: false);
                }
            }));
        }

        private void OnUserJoined(int userId)
        {
            Invoke((Action)(() =>
            {
                // 나 자신에 대한 입장 이벤트는 무시 (이미 추가됨)
                if (userId == _myUserId) return;

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
            _capture.Stop();
            _udp.Stop();

            foreach (var player in _remotePlayers.Values)
            {
                player.playback.Dispose();
            }
            _remotePlayers.Clear();

            // 나중에 _tcp.LeaveRoom() 호출
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
                if (!_isVoiceConnected)
                {
                    // 나 자신은 OnUserListReceived 또는 여기서 명시적으로 한 번만 추가됨
                    // _voicePanel.AddParticipant(_myUserId.ToString(), isSpeaking: false); 
                                                                             
                    _udp.Start();
                    // 처음에는 마이크 꺼진 상태로 시작 (필요 시 _capture.Start() 호출)
                    _isVoiceConnected = true;
                }
                _voicePanel.Visible = true;
            }
            else
                _chatPanel.Visible = true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _tcp.OnUserListReceived -= OnUserListReceived;
            _tcp.OnUserJoined -= OnUserJoined;
            _tcp.OnUserLeft -= OnUserLeft;
            base.OnFormClosing(e);
        }
        private void RoomForm_Load(object sender, System.EventArgs e) { }
    }
}