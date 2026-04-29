using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VoiceChat.Audio;
using VoiceChat.Forms;
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

        private int _myUserId;
       // private int _myRoomId;


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
            _tcp.OnLeaveSuccess += OnLeaveSuccess;
        }

        private void OnConnected(int userId)
        {

            _myUserId = userId;
            _tcp.RequestRoomList();

        }

        private void OnConnectFailed(string msg)
        {
            Invoke((Action)(() => MessageBox.Show(msg, "연결 실패")));
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

                item.OnJoinClick += (s, e) => JoinRoomRequest(room.RoomId);

                RoomLayoutPanel.Controls.Add(item);
            }

            RoomLayoutPanel.ResumeLayout();
        }

        private void JoinRoomRequest(int roomId)
        {

            _tcp.Connect("127.0.0.1", 9000);
            var roomForm = new RoomForm(_tcp, this, _myUserId, roomId);
            _tcp.JoinRoom(_myUserId, roomId);
            roomForm.Show();
            this.Hide();

        }

        private void OnLeaveSuccess()
        {
            Invoke((Action)(() => RefreshRoomList()));
        }

        private void OnRoomListReceived(List<RoomInfo> rooms)
        {
            Invoke((Action)(() =>
            {
                _currentRooms = rooms; // 저장
                UpdateRoomList(_currentRooms);
            }));
        }

        public void RefreshRoomList()
        {
            _tcp.RequestRoomList(); // 서버에 room_list 재요청
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
           

            // Audio Test Code
            _audioPlayback = new AudioPlayback();
            _jitterBuffer = new JitterBuffer();
            _audioCapture = new AudioCapture();

            _audioCapture.UserId = 1;
            _audioCapture.RoomId = 100;

            // JitterBuffer → AudioPlayback 콜백 연결
            _jitterBuffer.OnPacketReady = (opusData) =>
            {

                //if (opusData == null)
                //    Console.WriteLine("[AudioPlayback] 패킷 유실 → 무음 처리");
                //else
                //    Console.WriteLine($"[AudioPlayback] PlayOpus 호출 - opusData 크기: {opusData.Length} bytes");

                _audioPlayback.PlayOpus(opusData);

            };

            // AudioCapture → JitterBuffer 콜백 연결
            // @Todo UDPManager로 변경
            // JitterBuffer 콜백 연결이 아니라 서버로 전송하는 로직으로 변경해야함
            _audioCapture.OnPacketReady = (packet) =>
            {
                // 패킷에서 헤더 파싱
                if (PacketHandler.DeserializeHeader(packet, packet.Length, out PacketHeader header))
                {
                    //Console.WriteLine($"[AudioCapture] 패킷 생성 - 크기: {packet.Length} bytes / seq: {header.Sequence}");

                    // payload 추출 (헤더 13바이트 이후)
                    byte[] opusData = new byte[header.PayloadLength];
                    Buffer.BlockCopy(packet, PacketConstants.HEADER_SIZE, opusData, 0, header.PayloadLength);


                   // Console.WriteLine($"[JitterBuffer] Push - seq: {header.Sequence} / 버퍼 크기: {header.PayloadLength} bytes");
                    _jitterBuffer.Push(header, opusData);

                }
            };

            _audioPlayback.Start();
            _audioCapture.Start();
        }

        private void CreateRoomButton_Click(object sender, EventArgs e)
        {
            var dialog = new RoomDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                var newRoom = new RoomInfo
                {
                    Name = dialog.RoomName,
                    RoomId = new Random().Next(1, 9999)
                };

                _currentRooms.Add(newRoom);
                UpdateRoomList(_currentRooms);

                JoinRoomRequest(newRoom.RoomId); 
            }
        }
        
    }


    // 1. 방 정보를 담는 데이터 모델 (나중에 서버 JSON 파싱용으로도 사용됨)
    public class RoomInfo
    {
        public string Name { get; set; }
        public int RoomId { get; set; }

        public int CurrentUsers { get; set; } 

    }
}