using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChat.Forms
{
    public interface ITcpManager
    {
        event Action<int> OnConnected;
        event Action<string> OnConnectFailed;
        event Action<int> OnUserJoined;
        event Action<int> OnUserLeft;
        event Action<List<int>> OnUserListReceived;
        event Action<List<RoomInfo>> OnRoomListReceived;

        void Connect(string ip, int port);
        void RequestRoomList();
        void JoinRoom(int userId, int roomId);
        void LeaveRoom(int userId, int roomId); // 추가
    }
}
