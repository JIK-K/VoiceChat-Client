using Concentus;
using Concentus.Enums;
using NAudio.Wave;
using System;
using VoiceChat.protocol;
using VoiceChat.Utils;

namespace VoiceChat.Audio
{
    public class AudioCapture : IDisposable
    {
        private const int SAMPLE_RATE = 48000;
        private const int CHANNELS = 1;
        private const int BITS_PER_SAMPLE = 16;
        private const int FRAME_MS = 20;
        private const int FRAME_SAMPLES = SAMPLE_RATE * FRAME_MS / 1000; // 960
        private const int FRAME_BYTES = FRAME_SAMPLES * CHANNELS * (BITS_PER_SAMPLE / 8); // 1920

        private WaveInEvent _waveIn;
        private IOpusEncoder _encoder;
        private byte[] _frameBuffer;
        private int _frameBuffered;
        private ushort _sequence;

        public Action<byte[]> OnPacketReady { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }

        public AudioCapture()
        {
            // OpusCodecFactory → 플랫폼에 따라 네이티브 코드 자동 선택
            _encoder = OpusCodecFactory.CreateEncoder(SAMPLE_RATE, CHANNELS, OpusApplication.OPUS_APPLICATION_VOIP);
            _encoder.Bitrate = 32000;

            _frameBuffer = new byte[FRAME_BYTES];
            _frameBuffered = 0;

            _waveIn = new WaveInEvent();
            _waveIn.WaveFormat = new WaveFormat(SAMPLE_RATE, BITS_PER_SAMPLE, CHANNELS);
            _waveIn.BufferMilliseconds = 10;
            _waveIn.DataAvailable += OnDataAvailable;
        }

        public void Start()
        {
            Logger.Instance.Log("INFO", $"마이크 캡처 시작 - userId: {UserId}");
            _sequence = 0;
            _waveIn.StartRecording();
        }

        public void Stop()
        {
            Logger.Instance.Log("INFO", $"마이크 캡처 중지 - userId: {UserId}");
            _waveIn.StopRecording();
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            int offset = 0;

            while (offset < e.BytesRecorded)
            {
                int needed = FRAME_BYTES - _frameBuffered;
                int available = e.BytesRecorded - offset;
                int toCopy = Math.Min(needed, available);

                Buffer.BlockCopy(e.Buffer, offset, _frameBuffer, _frameBuffered, toCopy);
                _frameBuffered += toCopy;
                offset += toCopy;

                if (_frameBuffered >= FRAME_BYTES)
                {
                    EncodeAndMakePacket();
                    _frameBuffered = 0;
                }
            }
        }

        private void EncodeAndMakePacket()
        {
            // PCM → Opus 인코딩 (Span 버전)
            short[] pcm = new short[FRAME_SAMPLES];
            Buffer.BlockCopy(_frameBuffer, 0, pcm, 0, FRAME_BYTES);

            byte[] opusOutput = new byte[1275];
            int encodedBytes = _encoder.Encode(pcm.AsSpan(0, FRAME_SAMPLES),FRAME_SAMPLES, opusOutput.AsSpan(), opusOutput.Length);

            byte[] opusData = new byte[encodedBytes];
            Buffer.BlockCopy(opusOutput, 0, opusData, 0, encodedBytes);

            PacketHeader header = new PacketHeader
            {
                Type = PacketConstants.PACKET_TYPE_VOICE,
                RoomId = RoomId,
                UserId = UserId,
                Sequence = _sequence++,
                PayloadLength = (ushort)encodedBytes
            };

            byte[] packet = PacketHandler.Serialize(header, opusData);
            OnPacketReady?.Invoke(packet);
        }

        public void Dispose()
        {
            _waveIn?.StopRecording();
            _waveIn?.Dispose();
        }
    }
}