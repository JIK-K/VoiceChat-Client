using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.protocol;
using VoiceChat.Utils;

namespace VoiceChat.Audio
{
    public class JitterBuffer
    {
        private const int BUFFER_SIZE = 5;

        private SortedDictionary<ushort, byte[]> _buffer;

        private ushort _nextSequence;
        private bool _initalized;
        private object _lock = new object();

        // 재생할 패킷 준비됬을 때 호출 되는 콜백
        public Action<byte[]> OnPacketReady { get; set; }

        public JitterBuffer()
        {
            _buffer = new SortedDictionary<ushort, byte[]>();
            _initalized = false;
        }

        // UDP에서 수신한 패킷 추가
        public void Push(PacketHeader header,  byte[] opusData)
        {
            lock (_lock)
            {
                if (_initalized)
                {
                    _nextSequence = header.Sequence;
                    _initalized = true;
                }

                // 늦게온놈은 낙오처리
                if(IsOlderThan(_nextSequence, header.Sequence))
                {
                    return;
                }

                _buffer[header.Sequence] = opusData;

                Flush();
            }
        }

        private void Flush()
        {
            while(_buffer.Count >= BUFFER_SIZE)
            {
                // 다음 sequence packet 있으면 콜백
                if (_buffer.ContainsKey(_nextSequence))
                {
                    byte[] opusData = _buffer[_nextSequence];
                    _buffer.Remove(_nextSequence);
                    OnPacketReady?.Invoke(opusData);
                }
                else
                {

                    Logger.Instance.Log("WARN", $"패킷 손실 감지 - Sequence: {_nextSequence}");

                    // 해당 sequence가 없으면 유실된거
                    // null로 콜백 -> AudioPlayBack에서 무음처리
                    OnPacketReady?.Invoke(null);
                }
                unchecked { _nextSequence++; }
            }
        }

        // a가 b보다 오래된 sequence인지 확인
        // ushort 오버플로우 처리 (65535 → 0 넘어가는 경우)
        private bool IsOlderThan(ushort next, ushort incoming)
        {
            // sequence 차이가 반바퀴(32768) 이상이면 오버플로우로 판단
            return ((ushort)(incoming - next)) > 32768;
        }

        public void Reset()
        {
            lock (_lock)
            {
                _buffer.Clear();
                _initalized = false;
            }
        }
    }
}
