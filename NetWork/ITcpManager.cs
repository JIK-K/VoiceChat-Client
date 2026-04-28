using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChat.Forms
{
    public interface ITcpManager
    {
      
        event Action<string> OnConnectFailed;
        event Action<List<RoomInfo>> OnRoomListReceived;
        event Action<List<string>> OnUserListReceived; // 추가
      
       
        event Action<string> OnUserLeft;
        event Action<string> OnRoomCreated;

        event Action<int> OnConnected;
        event Action<int> OnUserJoined;

        void Connect(string ip, int port);
        void JoinRoom(int userId, int roomId);
        void RequestRoomList();
        void CreateRoom(string roomName);
    }
}
