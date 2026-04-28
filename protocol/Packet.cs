using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChat.protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketHeader
    {
        public byte Type;           // 패킷 종류 (0x01: 제어, 0x02: 음성)
        public int RoomId;          // 대상 방 번호
        public int UserId;          // 송신자 ID
        public ushort Sequence;     // 패킷 순서 번호
        public ushort PayloadLength; // 헤더 뒤에 오는 데이터 크기
    }

    public static class PacketConstants
    {
        public const byte PACKET_TYPE_CONTROL = 0x01;
        public const byte PACKET_TYPE_VOICE = 0x02;
        public const int HEADER_SIZE = 13;
    }
}
