using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoiceChat.Forms
{
    internal class TestTcpManager : ITcpManager
    {
        public event Action OnConnected;
        public event Action<string> OnConnectFailed;
        public event Action<string> OnUserJoined;
        public event Action<string> OnUserLeft;
        public event Action<List<RoomInfo>> OnRoomListReceived;
        public event Action<List<string>> OnUserListReceived;

        public void RequestRoomList()
        {
            Console.WriteLine("[TEST] RequestRoomList 호출");

            Task.Delay(500).ContinueWith(_ =>
                OnRoomListReceived?.Invoke(new List<RoomInfo>
                {
                    new RoomInfo { Name = "room1", CurrentUsers = 1, MaxUsers = 10 },
                    new RoomInfo { Name = "room2", CurrentUsers = 2, MaxUsers = 10 },
                    new RoomInfo { Name = "room3", CurrentUsers = 3, MaxUsers = 10 }
                }));
        }

        public void Connect(string ip, int port, string nickname)
        {
            Console.WriteLine($"[TEST] Connect: {ip}:{port} {nickname}");

            // 1초 후 연결 성공
            Task.Delay(1000).ContinueWith(_ =>
            {
                OnConnected?.Invoke();
            });
        }

        public void JoinVoiceChannel(string channelName)
        {
            Console.WriteLine($"[TEST] JoinVoiceChannel: {channelName}");

            // 기존 참여자 목록 전달
            OnUserListReceived?.Invoke(new List<string> { "홍길동", "김철수" });

        }
    }
}
