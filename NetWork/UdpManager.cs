using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VoiceChat.protocol;

namespace VoiceChat.NetWork
{
    public class UdpManager : IDisposable
    {
        private UdpClient _udpClient;
        private IPEndPoint _serverEndPoint;
        private Thread _receivedThread;
        private bool _running;

        // 수신한 음성 패킷을 Jitter로 전달 콜백
        public Action<PacketHeader, byte[]> OnVoiceReceived { get; set; }

        public UdpManager(string serverIp, int serverPort)
        {
            _serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
            _udpClient = new UdpClient();
            _udpClient.Connect(_serverEndPoint);
        }

        // Voice Packet Send (AudioCapture.OnPacketReady)
        public void Send(byte[] packet)
        {
            try
            {
                _udpClient.Send(packet, packet.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[UdpManager] Send 실패: {e.Message}");
            }
        }

        public void Start()
        {
            _running = true;
            _receivedThread = new Thread(ReceivedLoop);
            _receivedThread.IsBackground = true;
            _receivedThread.Start();
        }

        public void Stop()
        {
            _running = false;
            _udpClient.Close();
        }


        public void ReceivedLoop()
        {
            while (_running)
            {
                try
                {
                    // 서버로부터 UDP 패킷 수신
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = _udpClient.Receive(ref remoteEP);

                    if (!PacketHandler.DeserializeHeader(data, data.Length, out PacketHeader header))
                        continue;

                    byte[] opusData = new byte[header.PayloadLength];
                    Buffer.BlockCopy(data, PacketConstants.HEADER_SIZE, opusData, 0, header.PayloadLength);

                    OnVoiceReceived?.Invoke(header, opusData);
                }
                catch (Exception e)
                {
                    if(_running)
                        Console.WriteLine($"[UdpManager] Receive 실패: {e.Message}");
                }
            }
        }

        public void Dispose()
        {
            Stop();
            _udpClient?.Dispose();
        }
    }
}
