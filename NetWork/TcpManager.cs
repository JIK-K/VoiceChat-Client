using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


namespace VoiceChat.Forms
{
    internal class TcpManager : ITcpManager
    {
        public event Action OnConnected;
        public event Action<string> OnConnectFailed;
        //public event Action<string> OnUserJoined;
        public event Action<string> OnUserLeft;
        public event Action<List<RoomInfo>> OnRoomListReceived;
        public event Action<List<string>> OnUserListReceived;
        

        private TcpClient _client;
        private NetworkStream _stream;

        public event Action<int> OnUserJoined;
        public event Action<string> OnRoomCreated;

        public void RequestRoomList()
        {
            Console.WriteLine("[TEST] RequestRoomList 호출");

            Task.Delay(500).ContinueWith(_ =>
               OnRoomListReceived?.Invoke(new List<RoomInfo>
               {
                    new RoomInfo { Name = "room1",},
                   new RoomInfo { Name = "room2",},
                   new RoomInfo { Name = "room3",}
               }));
        }

        public void Connect(string ip, int port)
        {
            Task.Run(async () =>
            {
                try
                {
                    _client = new TcpClient();
                    await _client.ConnectAsync(ip, port);
                    _stream = _client.GetStream();

                    OnConnected?.Invoke();

                    // 수신 루프
                    ReceiveLoop();
                }
                catch (Exception ex)
                {
                    OnConnectFailed?.Invoke(ex.Message);
                }
            });
        }

        public void JoinRoom(int userId, int roomId)
        {
            Send(new { cmd = "join", userId = userId, roomId = roomId });
        }



        private void Send(object obj)
        {
            var json = JsonSerializer.Serialize(obj);
            var data = Encoding.UTF8.GetBytes(json + "\n");
            _stream.Write(data, 0, data.Length);
        }
        //public void JoinVoiceChannel(string channelName)
        //{
        //    Console.WriteLine($"[TEST] JoinVoiceChannel: {channelName}");

        //    // 기존 참여자 목록 전달
        //    OnUserListReceived?.Invoke(new List<string> { "홍길동", "김철수" });

        //}

        public void CreateRoom(string roomName)
        {
            Console.WriteLine($"[TEST] CreateRoom: {roomName}");
            Task.Delay(500).ContinueWith(_ =>
            {
                // 방 생성 완료 알림
                OnRoomCreated?.Invoke(roomName);
            });
        }


        private void ParseEvent(string json)
        {
            var doc = JsonDocument.Parse(json);
            var evt = doc.RootElement.GetProperty("event").GetString();

            switch (evt)
            {
                //case "user_list":
                //    var users = doc.RootElement
                //        .GetProperty("users")
                //        .EnumerateArray()
                //        .Select(u => u.GetInt32()) // userId가 int
                //        .ToList();
                //    OnUserListReceived?.Invoke(users);
                //    break;

                case "user_joined":
                    var joinedId = doc.RootElement.GetProperty("userId").GetInt32();
                    OnUserJoined?.Invoke(joinedId);
                    break;

                //case "user_left":
                //    var leftId = doc.RootElement.GetProperty("userId").GetInt32();
                //    OnUserLeft?.Invoke(leftId);
                //    break;
            }
        }

        private void ReceiveLoop()
        {
            Task.Run(() =>
            {
                var buffer = new byte[4096];
                while (true)
                {
                    try
                    {
                        int n = _stream.Read(buffer, 0, buffer.Length);
                        if (n == 0) break;

                        var json = Encoding.UTF8.GetString(buffer, 0, n);
                        ParseEvent(json); // 수신하면 바로 파싱
                    }
                    catch { break; }
                }
            });
        }
    }
}
