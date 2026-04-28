using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VoiceChat.Audio;
using VoiceChat.Forms;
using VoiceChat.protocol;

namespace VoiceChat
{

    public partial class MainForm : MaterialForm
    {
        // Audio Test Code
        private AudioCapture _audioCapture;
        private AudioPlayback _audioPlayback;
        private JitterBuffer _jitterBuffer;

        public MainForm()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
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
                item.RoomCount = $"{room.CurrentUsers}/{room.MaxUsers}";

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
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 3. 테스트용 가짜 데이터 (객체 리스트)
            var dummyRooms = new List<RoomInfo> 
            { 
                new RoomInfo { Name = "롤 5인큐 구함", CurrentUsers = 3, MaxUsers = 5 },
                new RoomInfo { Name = "C++ 고수만", CurrentUsers = 1, MaxUsers = 10 },
                new RoomInfo { Name = "음성채팅 테스트방", CurrentUsers = 8, MaxUsers = 20 },
                new RoomInfo { Name = "음성채팅 테스트방", CurrentUsers = 8, MaxUsers = 20 },
                new RoomInfo { Name = "음성채팅 테스트방", CurrentUsers = 8, MaxUsers = 20 },
                new RoomInfo { Name = "음성채팅 테스트방", CurrentUsers = 8, MaxUsers = 20 },
                new RoomInfo { Name = "음성채팅 테스트방", CurrentUsers = 8, MaxUsers = 20 },
                new RoomInfo { Name = "음성채팅 테스트방", CurrentUsers = 8, MaxUsers = 20 },
                new RoomInfo { Name = "음성채팅 테스트방", CurrentUsers = 8, MaxUsers = 20 }

            };

            UpdateRoomList(dummyRooms);

            // Audio Test Code
            _audioPlayback = new AudioPlayback();
            _jitterBuffer = new JitterBuffer();
            _audioCapture = new AudioCapture();

            _audioCapture.UserId = 1;
            _audioCapture.RoomId = 100;

            // JitterBuffer → AudioPlayback 콜백 연결
            _jitterBuffer.OnPacketReady = (opusData) =>
            {

                if (opusData == null)
                    Console.WriteLine("[AudioPlayback] 패킷 유실 → 무음 처리");
                else
                    Console.WriteLine($"[AudioPlayback] PlayOpus 호출 - opusData 크기: {opusData.Length} bytes");

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
                    Console.WriteLine($"[AudioCapture] 패킷 생성 - 크기: {packet.Length} bytes / seq: {header.Sequence}");

                    // payload 추출 (헤더 13바이트 이후)
                    byte[] opusData = new byte[header.PayloadLength];
                    Buffer.BlockCopy(packet, PacketConstants.HEADER_SIZE, opusData, 0, header.PayloadLength);


                    Console.WriteLine($"[JitterBuffer] Push - seq: {header.Sequence} / 버퍼 크기: {header.PayloadLength} bytes");
                    _jitterBuffer.Push(header, opusData);

                }
            };

            _audioPlayback.Start();
            _audioCapture.Start();
        }
    }


    // 1. 방 정보를 담는 데이터 모델 (나중에 서버 JSON 파싱용으로도 사용됨)
    public class RoomInfo
    {
        public string Name { get; set; }
        public int CurrentUsers { get; set; }
        public int MaxUsers { get; set; }
    }
}