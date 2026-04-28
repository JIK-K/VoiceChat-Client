using Concentus;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChat.Audio
{
    public class AudioPlayback : IDisposable
    {
        private const int SAMPLE_RATE = 48000;
        private const int CHANNELS = 1;
        private const int FRAME_SAMPLES = SAMPLE_RATE * 20 / 1000;
        private const int FRAME_BYTES = FRAME_SAMPLES * CHANNELS * 2;

        private IOpusDecoder _decoder;
        private BufferedWaveProvider _buffer;
        private WaveOutEvent _waveOut;

        public AudioPlayback()
        {
            // Opus decoder 초기화
            _decoder = OpusCodecFactory.CreateDecoder(SAMPLE_RATE, CHANNELS);

            // NAudio 재생 버퍼 초기화
            _buffer = new BufferedWaveProvider(new WaveFormat(SAMPLE_RATE, 16, CHANNELS));
            _buffer.BufferDuration = TimeSpan.FromSeconds(2); // 최대 2초 버퍼
            _buffer.DiscardOnBufferOverflow = true; // 꽉차면 오래된거 버리기

            // 스피커 출력 초기화
            _waveOut = new WaveOutEvent();
            _waveOut.Init(_buffer);
        }

        public void Start()
        {
            _waveOut.Play();
        }

        public void Stop()
        {
            _waveOut.Stop();
        }

        // JitterBuffer에서 호출 -> Opus 데이터 받아서 디코딩 후 재생
        public void PlayOpus(byte[] opusData)
        {
            // null = 패킷 유실 = 무음 처리
            if (opusData == null)
            {
                byte[] slience = new byte[FRAME_BYTES];
                _buffer.AddSamples(slience, 0, FRAME_BYTES);
                return;
            }

            // Opus -> PCM 디코딩
            short[] pcm = new short[FRAME_SAMPLES];
            int decodedSamples = _decoder.Decode(
                opusData.AsSpan(),
                pcm.AsSpan(0, FRAME_SAMPLES),
                FRAME_SAMPLES
            );


            // short[] -> byte[] 변환, 재생 버퍼 추가
            byte[] pcmBytes = new byte[decodedSamples * 2];
            Buffer.BlockCopy(pcm, 0, pcmBytes, 0, pcmBytes.Length);
            _buffer.AddSamples(pcmBytes, 0, pcmBytes.Length);
        }

        public void Dispose()
        {
            _waveOut?.Stop();
            _waveOut?.Dispose();
        }
    }
}
