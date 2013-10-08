using System;
using System.Collections.Generic;
using System.Text;
using NAudio.Utils;

namespace NAudio.Wave.SampleProviders
{
    class FourChannel8SampleChunkConverter : ISampleChunkConverter
    {
        private long offset;
        private byte[] sourceBuffer;
        private int sourceBytes;
        WaveFormat waveFormat;

        public bool Supports(WaveFormat waveFormat)
        {
            if (waveFormat.Encoding == WaveFormatEncoding.Pcm &&
                waveFormat.BitsPerSample == 8 &&
                waveFormat.Channels != 2)
            {
                this.waveFormat = waveFormat;
                return true;
            }
            return false;
        }

        public void LoadNextChunk(IWaveProvider source, int samplePairsRequired)
        {
            int sourceBytesRequired = samplePairsRequired * waveFormat.Channels;
            sourceBuffer = BufferHelpers.Ensure(sourceBuffer, sourceBytesRequired);
            sourceBytes = source.Read(sourceBuffer, 0, sourceBytesRequired);
            offset = 0;
        }

        public bool GetNextSample(out float sampleLeft, out float sampleRight)
        {
            if (offset < sourceBytes)
            {
                sampleLeft = sourceBuffer[offset++] / 256f;
                if (waveFormat.Channels > 1)
                {
                    sampleRight = sourceBuffer[offset++] / 256f;
                }
                else
                {
                    sampleRight = sampleLeft;
                }
                if (waveFormat.Channels > 2)
                {
                    float sampleLeft2 = sourceBuffer[offset++] / 256f;
                    sampleLeft = (sampleLeft + sampleLeft2) / 2.0f;

                    if (waveFormat.Channels > 3)
                    {
                        float sampleRight2 = sourceBuffer[offset++] / 256f;
                        sampleRight = (sampleRight + sampleRight2) / 2.0f;

                        if (waveFormat.Channels > 4)
                        {

                            float sampleLeft3 = sourceBuffer[offset++] / 256f;
                            sampleLeft = (sampleLeft + sampleLeft3) / 2.0f;

                            if (waveFormat.Channels > 5)
                            {
                                float sampleRight3 = sourceBuffer[offset++] / 256f;
                                sampleRight = (sampleRight + sampleRight3) / 2.0f;
                                if (waveFormat.Channels > 6)
                                {

                                    float sampleLeft4 = sourceBuffer[offset++] / 256f;
                                    sampleLeft = (sampleLeft + sampleLeft4) / 2.0f;

                                    if (waveFormat.Channels > 7)
                                    {
                                        float sampleRight4 = sourceBuffer[offset++] / 256f;
                                        sampleRight = (sampleRight + sampleRight4) / 2.0f;
                                        if (waveFormat.Channels > 8)
                                        {
                                            float sampleLeft5 = sourceBuffer[offset++] / 256f;
                                            sampleLeft = (sampleLeft + sampleLeft5) / 2.0f;

                                            if (waveFormat.Channels > 9)
                                            {
                                                float sampleRight5 = sourceBuffer[offset++] / 256f;
                                                sampleRight = (sampleRight + sampleRight5) / 2.0f;

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
            else
            {
                sampleLeft = 0.0f;
                sampleRight = 0.0f;
                return false;
            }
        }
    }

    class Stereo8SampleChunkConverter : ISampleChunkConverter
    {
        private int offset;
        private byte[] sourceBuffer;
        private int sourceBytes;

        public bool Supports(WaveFormat waveFormat)
        {
            return waveFormat.Encoding == WaveFormatEncoding.Pcm &&
                waveFormat.BitsPerSample == 8 &&
                waveFormat.Channels == 2;
        }

        public void LoadNextChunk(IWaveProvider source, int samplePairsRequired)
        {
            int sourceBytesRequired = samplePairsRequired * 2;
            sourceBuffer = BufferHelpers.Ensure(sourceBuffer, sourceBytesRequired);
            sourceBytes = source.Read(sourceBuffer, 0, sourceBytesRequired);
            offset = 0;
        }

        public bool GetNextSample(out float sampleLeft, out float sampleRight)
        {
            if (offset < sourceBytes)
            {
                sampleLeft = sourceBuffer[offset++] / 256f;
                sampleRight = sourceBuffer[offset++] / 256f;
                return true;
            }
            else
            {
                sampleLeft = 0.0f;
                sampleRight = 0.0f;
                return false;
            }
        }
    }
}
