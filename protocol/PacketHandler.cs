using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace VoiceChat.protocol
{
    public static class PacketHandler
    {
        // 직렬화 PacketHeader + Payload -> byte[]
        public static byte[] Serialize(PacketHeader header, byte[] payload)
        {
            ushort payloadLen = (ushort)(payload?.Length ?? 0);
            header.PayloadLength = payloadLen;

            byte[] buffer = new byte[PacketConstants.HEADER_SIZE + payload.Length];
            
            // 헤더 구조체를 바이트 배열로 변환
            IntPtr ptr = Marshal.AllocHGlobal(PacketConstants.HEADER_SIZE);
            try
            {
                Marshal.StructureToPtr(header, ptr, false);
                Marshal.Copy(ptr, buffer, 0, PacketConstants.HEADER_SIZE);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }

            // payload copy
            if(payloadLen > 0)
            {
                Buffer.BlockCopy(payload, 0, buffer, PacketConstants.HEADER_SIZE, payloadLen);
            }

            return buffer;
        }

        // 역직렬화 byte[] -> PacketHeader
        public static bool DeserializeHeader(byte[] buffer, int bytesRead, out PacketHeader header)
        {
            header = new PacketHeader();

            if (bytesRead < PacketConstants.HEADER_SIZE)
                return false;

            // 바이트 배열의 메모리 주소를 고정하고 구조체로 캐스팅
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                header = Marshal.PtrToStructure<PacketHeader>(handle.AddrOfPinnedObject());
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                handle.Free();
            }
        }

        // 수신된 패킷 타입에 따라 분기 처리 (예시)
        public static void HandlePacket(PacketHeader header, byte[] payload)
        {
            switch (header.Type)
            {
                case PacketConstants.PACKET_TYPE_CONTROL:
                    string json = System.Text.Encoding.UTF8.GetString(payload);
                    Console.WriteLine($"[Control] Room: {header.RoomId}, Data: {json}");
                    // TODO: UI 업데이트나 로그인/입장 로직 연결
                    break;

                case PacketConstants.PACKET_TYPE_VOICE:
                    // TODO: Opus 디코더 및 오디오 재생 스트림으로 전달
                    Console.WriteLine($"[Voice] User: {header.UserId}, Seq: {header.Sequence}, Len: {header.PayloadLength}");
                    break;
            }
        }
    }
}
