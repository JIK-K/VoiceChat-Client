using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VoiceChat;
using VoiceChat.Forms;
using VoiceChat.protocol;

internal class TcpManager : ITcpManager
{
    // ── 이벤트 ──
    public event Action<int> OnConnected;
    public event Action<string> OnConnectFailed;
    public event Action<int> OnUserJoined;
    public event Action<int> OnUserLeft;           // string → int
    public event Action<List<int>> OnUserListReceived; // List<string> → List<int>
    public event Action<List<RoomInfo>> OnRoomListReceived;
    public event Action OnLeaveSuccess;

    // ── 필드 ──
    private TcpClient _client;
    private NetworkStream _stream;
    private int _UserId;




    // ── 메서드 ──
    public void Connect(string ip, int port)
    {
        Task.Run(async () =>
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(ip, port);
                _stream = _client.GetStream();

                _UserId = ((System.Net.IPEndPoint)_client.Client.LocalEndPoint).Port;
                OnConnected?.Invoke(_UserId);
                ReceiveLoop();
            }
            catch (Exception ex)
            {
                OnConnectFailed?.Invoke(ex.Message);
            }
        });
    }

    public void RequestRoomList()
    {
        Console.WriteLine("[TcpManager] RequestRoomList 호출");
        if (_stream == null)
        {
            Console.WriteLine("[TcpManager] _stream null");
            return;
        }
        SendPacket(PacketConstants.PACKET_TYPE_CONTROL, _UserId, 0,
            new { cmd = "room_list" });
    }

    public void JoinRoom(int userId, int roomId)
    {
        Console.WriteLine($"[TcpManager] JoinRoom 전송: userId={userId}, roomId={roomId}");
        SendPacket(PacketConstants.PACKET_TYPE_CONTROL, userId, roomId,
            new { cmd = "join", userId = userId, roomId = roomId });
    }

    public void LeaveRoom(int userId, int roomId)
    {
        Console.WriteLine($"[TcpManager] LeaveRoom 전송: userId={userId}, roomId={roomId}");
        SendPacket(PacketConstants.PACKET_TYPE_CONTROL, userId, roomId,
            new { cmd = "leave", userId = userId, roomId = roomId });
    }

    // ── 내부 ──
    private void SendPacket(byte type, int userId, int roomId, object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        var payloadData = Encoding.UTF8.GetBytes(json);

        PacketHeader header = new PacketHeader
        {
            Type = type,
            RoomId = roomId,
            UserId = userId,
            Sequence = 0,
            PayloadLength = (ushort)payloadData.Length
        };

        byte[] packetData = PacketHandler.Serialize(header, payloadData);
        _stream.Write(packetData, 0, packetData.Length);
    }

    private void ParseEvent(byte[] payload)
    {
        string json = Encoding.UTF8.GetString(payload);

        Console.WriteLine($"[TcpManager] ParseEvent: {json}");

        var doc = JsonDocument.Parse(json);
        // result:ok 먼저 체크
        if (doc.RootElement.TryGetProperty("result", out var result)
            && result.GetString() == "ok")
        {
            OnLeaveSuccess?.Invoke();
            return;
        }

        // event 없으면 그냥 리턴
        if (!doc.RootElement.TryGetProperty("event", out var evtProp)) return;
        var evt = evtProp.GetString();

        switch (evt)
        {
            case "room_list":
                var rooms = doc.RootElement
                    .GetProperty("rooms")
                    .EnumerateArray()
                    .Select(r => new RoomInfo
                    {
                        RoomId = r.GetProperty("roomId").GetInt32(),
                        CurrentUsers = r.GetProperty("count").GetInt32(),
                    })
                    .Where(r => r.CurrentUsers > 0)
                    .ToList();
                Console.WriteLine($"[TcpManager] room_list 수신 - 방 개수: {rooms.Count}");
                OnRoomListReceived?.Invoke(rooms);
                break;

            case "user_list":
                var users = doc.RootElement
                    .GetProperty("users")
                    .EnumerateArray()
                    .Select(u => u.GetInt32())
                    .ToList();
                Console.WriteLine($"[TcpManager] user_list 수신 - {users.Count}명");
                OnUserListReceived?.Invoke(users);
                break;

            case "user_joined":
                var joinedId = doc.RootElement.GetProperty("userId").GetInt32();
                Console.WriteLine($"[TcpManager] user_joined: {joinedId}");
                OnUserJoined?.Invoke(joinedId);
                break;

            case "user_left":
                var leftId = doc.RootElement.GetProperty("userId").GetInt32();
                Console.WriteLine($"[TcpManager] user_left: {leftId}");
                OnUserLeft?.Invoke(leftId);
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
                    byte[] headerBuf = new byte[PacketConstants.HEADER_SIZE];
                    int bytesRead = ReadExact(_stream, headerBuf, PacketConstants.HEADER_SIZE);
                    if (bytesRead == 0) break;

                    if (!PacketHandler.DeserializeHeader(headerBuf, bytesRead, out PacketHeader header))
                    {
                        Console.WriteLine("[TcpManager] 헤더 파싱 실패");
                        continue;
                    }

                    byte[] payload = new byte[header.PayloadLength];
                    ReadExact(_stream, payload, header.PayloadLength);


                    ParseEvent(payload);
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