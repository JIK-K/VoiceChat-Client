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
        public event Action<List<string>> OnUserListReceived;
        public event Action<string> OnUserJoined;
        public event Action<string> OnUserLeft;


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

            // 3초 후 유저 퇴장
            Task.Delay(3000).ContinueWith(_ =>
                OnUserLeft?.Invoke("김철수"));
        }
    }
}
