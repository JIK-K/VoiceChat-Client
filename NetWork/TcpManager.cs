using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VoiceChat.protocol;


namespace VoiceChat.Forms
{
    internal class TcpManager : ITcpManager
    {
        
        public event Action<string> OnConnectFailed;
        public event Action<List<RoomInfo>> OnRoomListReceived;


        public event Action<List<string>> OnUserListReceived;
        public event Action<string> OnUserLeft;

        private TcpClient _client;
        private NetworkStream _stream;

        public event Action<int> OnConnected;
        public event Action<int> OnUserJoined;
        public event Action<string> OnRoomCreated;

        public void RequestRoomList()
        {
            Console.WriteLine("[TcpManager] RequestRoomList 호출");

            if (_stream == null)
            {
                Console.WriteLine("[TcpManager] RequestRoomList 실패 - _stream이 null (연결 안됨)");
                return;
            }

            Console.WriteLine("[TcpManager] room_list 패킷 전송");


            Send(new { cmd = "room_list" });
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


                    int userId = ((System.Net.IPEndPoint)_client.Client.LocalEndPoint).Port;

                    OnConnected?.Invoke(userId);

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

                case "room_list":
                    var rooms = doc.RootElement
                        .GetProperty("rooms")
                        .EnumerateArray()
                        .Select(r => new RoomInfo
                        {
                            RoomId = r.GetProperty("roomId").GetInt32(),
                            //Name = $"방 {r.GetProperty("roomId").GetInt32()}", // roomId로 이름 대체
                           
                        })
                        .ToList();
                    OnRoomListReceived?.Invoke(rooms);
                    break;

            }
        }

        private void ReceiveLoop()
        {
            Task.Run(() =>
            {
                Console.WriteLine("[TcpManager] ReceiveLoop 시작");
                while (true)
                {
                    try
                    {
                        // 1. 헤더 13바이트 먼저 읽기
                        byte[] headerBuf = new byte[PacketConstants.HEADER_SIZE];
                        int bytesRead = ReadExact(_stream, headerBuf, PacketConstants.HEADER_SIZE);
                        if (bytesRead == 0) break;

                        // 2. 헤더 파싱
                        if (!PacketHandler.DeserializeHeader(headerBuf, bytesRead, out PacketHeader header))
                        {
                            Console.WriteLine("[TcpManager] 헤더 파싱 실패");
                            continue;
                        }

                        // 3. payload 읽기
                        byte[] payload = new byte[header.PayloadLength];
                        ReadExact(_stream, payload, header.PayloadLength);

                        // 4. JSON 변환 후 파싱
                        string json = Encoding.UTF8.GetString(payload);
                        Console.WriteLine($"[TcpManager] 수신: {json}");
                        ParseEvent(json);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[TcpManager] ReceiveLoop 예외: {ex.Message}");
                        break;
                    }
                }
                Console.WriteLine("[TcpManager] ReceiveLoop 종료");
            });
        }

        // 정확히 n바이트 읽는 헬퍼 메서드
        private int ReadExact(NetworkStream stream, byte[] buffer, int count)
        {
            int total = 0;
            while (total < count)
            {
                int n = stream.Read(buffer, total, count - total);
                if (n == 0) return 0;
                total += n;
            }
            return total;
        }
    }
}
