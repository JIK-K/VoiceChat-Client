using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VoiceChat.Audio;
using VoiceChat.Forms;
using VoiceChat.NetWork;
using VoiceChat.protocol;

namespace VoiceChat
{

    public partial class MainForm : MaterialForm
    {

        private ITcpManager _tcp = new TcpManager();
        private List<RoomInfo> _currentRooms = new List<RoomInfo>();
        // Audio Test Code
        private AudioCapture _audioCapture;
        private AudioPlayback _audioPlayback;
        private JitterBuffer _jitterBuffer;
        private UdpManager _udpManager;


        public MainForm()
        {
            InitializeComponent();

            InitializeMaterialSkin();

            SubscribeEvents();

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



        private void SubscribeEvents()
        {
            _tcp.OnConnected += OnConnected;
            _tcp.OnConnectFailed += OnConnectFailed;
            _tcp.OnRoomListReceived += OnRoomListReceived;
            _tcp.OnRoomCreated += OnRoomCreated;
        }

        private void OnConnected()
        {
            Invoke((Action)(() =>
            {
                var roomForm = new RoomForm( _tcp, this); // this 추가
                roomForm.Show();
                this.Hide();
            }));
        }

        private void OnConnectFailed(string msg)
        {
            Invoke((Action)(() => MessageBox.Show(msg, "연결 실패")));
        }
        private void OnRoomCreated(string roomName)
        {
            Invoke((Action)(() => JoinRoomRequest(roomName)));
        }

        // 2. 이제 List<string>이 아닌 List<RoomInfo>를 받습니다.
        private void UpdateRoomList(List<RoomInfo> rooms)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateRoomList(rooms)));
                return;
            }

            RoomLayoutPanel.Controls.Clear();
            RoomLayoutPanel.SuspendLayout();

            foreach (var room in rooms)
            {
                RoomItemControl item = new RoomItemControl();
                item.RoomName = room.Name;
                //item.RoomCount = $"{room.CurrentUsers}/{room.MaxUsers}";

                // 아이템 간의 간격(Gap)만 설정
                item.Margin = new Padding(0, 0, 0, 10);

                // 너비 계산 (생성자에서 정의한 규칙과 동일하게)
                //item.Width = RoomLayoutPanel.ClientSize.Width - RoomLayoutPanel.Padding.Horizontal - 5;

                item.OnJoinClick += (s, e) => JoinRoomRequest(room.Name);

                RoomLayoutPanel.Controls.Add(item);
            }

            RoomLayoutPanel.ResumeLayout();
        }

        private void JoinRoomRequest(string roomName)
        {
            MessageBox.Show($"{roomName} 입장 요청을 보냅니다.");

            //이민하 : 테스트 코드 추가
            _tcp.Connect("127.0.0.1", 9000);

        }

        private void OnRoomListReceived(List<RoomInfo> rooms)
        {
            Invoke((Action)(() =>
            {
                _currentRooms = rooms; // 저장
                UpdateRoomList(_currentRooms);
            }));
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
        
            _tcp.RequestRoomList();

            // Audio Test Code
            _audioPlayback = new AudioPlayback();
            _jitterBuffer = new JitterBuffer();
            _audioCapture = new AudioCapture();
            _udpManager = new UdpManager("127.0.0.1", 9001);

            _audioCapture.UserId = 1;
            _audioCapture.RoomId = 100;


            _audioCapture.OnPacketReady = (packet) =>
            {
                _udpManager.Send(packet);
            };

            _udpManager.OnVoiceReceived = (header, opusData) =>
            {
                _jitterBuffer.Push(header, opusData);
            };

            _audioPlayback.Start();
            _udpManager.Start();
            _audioCapture.Start();
        }

        private void CreateRoomButton_Click(object sender, EventArgs e)
        {
            var dialog = new RoomDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                //임시 테스트용 코드
                {
                    // UI만 추가
                    _currentRooms.Add(new RoomInfo { Name = dialog.RoomName });
                    UpdateRoomList(_currentRooms);

                    // 바로 조인
                    JoinRoomRequest(dialog.RoomName);
                }
               
            }
        }
        
    }


    // 1. 방 정보를 담는 데이터 모델 (나중에 서버 JSON 파싱용으로도 사용됨)
    public class RoomInfo
    {
        public string Name { get; set; }
        //public int CurrentUsers { get; set; }
        //public int MaxUsers { get; set; }
    }
}