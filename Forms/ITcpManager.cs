using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChat.Forms
{
    public interface ITcpManager
    {
        event Action OnConnected;
        event Action<string> OnConnectFailed;
        event Action<List<RoomInfo>> OnRoomListReceived;
        event Action<List<string>> OnUserListReceived; // 추가
        event Action<string> OnUserJoined;
        event Action<string> OnUserLeft;
        event Action<string> OnRoomCreated;

        void Connect(string ip, int port, string nickname);
        void JoinVoiceChannel(string channelName);

        void RequestRoomList();

        void CreateRoom(string roomName);
    }
}
