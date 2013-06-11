

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sanford.Multimedia.Midi
{
    /// <summary>
    /// Writes a Track to a Stream.
    /// </summary>
    internal class TrackWriter
    {
        private static readonly byte[] TrackHeader =
            {
                (byte)'M',
                (byte)'T',
                (byte)'r',
                (byte)'k'
            };

        // The Track to write to the Stream.
        private Track track = new Track(FileType.Unknown);

        // The Stream to write to.
        private Stream stream;

        // Running status.
        private int runningStatus = 0;

        // The Track data in raw bytes.
        private List<byte> trackData = new List<byte>();


        public void Write(Stream strm)
        {
            this.stream = strm;

            trackData.Clear();

            stream.Write(TrackHeader, 0, TrackHeader.Length);

            int absTick = 0;

            foreach (MidiEvent e in track.AllIterator())
            {
                WriteVariableLengthValue(e.AbsoluteTicks - absTick);

                absTick = e.AbsoluteTicks;

                switch (e.MessageType)
                {
                    case MessageType.Channel:
                        Write((ChannelMessage)e.Clone());
                        break;

                    case MessageType.SystemExclusive:
                        Write((SysExMessage)e.Clone());
                        break;

                    case MessageType.Meta:
                        {
                            Write((MetaMessage)e.Clone());
                        }
                        break;

                    case MessageType.SystemCommon:
                        Write((SysCommonMessage)e.Clone());
                        break;

                    case MessageType.SystemRealtime:
                        Write((SysRealtimeMessage)e.Clone());
                        break;
                }
            }

            byte[] trackLength = BitConverter.GetBytes(trackData.Count);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(trackLength);
            }

            stream.Write(trackLength, 0, trackLength.Length);

            foreach (byte b in trackData)
            {
                stream.WriteByte(b);
            }
        }

        private void WriteVariableLengthValue(int value)
        {
            int v = value;
            byte[] array = new byte[4];
            int count = 0;

            array[0] = (byte)(v & 0x7F);

            v >>= 7;

            while (v > 0)
            {
                count++;
                array[count] = (byte)((v & 0x7F) | 0x80);
                v >>= 7;
            }

            while (count >= 0)
            {
                trackData.Add(array[count]);
                count--;
            }
        }

        private void Write(ChannelMessage message)
        {
            if (runningStatus != message.Status)
            {
                trackData.Add((byte)message.Status);
                runningStatus = message.Status;
            }

            trackData.Add((byte)message.Data1);

            if (ChannelMessage.DataBytesPerType(message.Command) == 2)
            {
                trackData.Add((byte)message.Data2);
            }
        }

        private void Write(SysExMessage message)
        {
            // System exclusive message cancel running status.
            runningStatus = 0;

            trackData.Add((byte)message.Status);

            WriteVariableLengthValue(message.Length - 1);

            for (int i = 1; i < message.Length; i++)
            {
                trackData.Add(message[i]);
            }
        }

        private void Write(MetaMessage message)
        {


            trackData.Add((byte)message.Status);

            trackData.Add((byte)message.MetaType);

            WriteVariableLengthValue(message.Length);

            trackData.AddRange(message.GetBytes());
        }

        private void Write(SysCommonMessage message)
        {
            // Escaped messages cancel running status.
            runningStatus = 0;

            // Escaped message.
            trackData.Add((byte)0xF7);

            trackData.Add((byte)message.Status);

            switch (message.SysCommonType)
            {
                case SysCommonType.MidiTimeCode:
                    trackData.Add((byte)message.Data1);
                    break;

                case SysCommonType.SongPositionPointer:
                    trackData.Add((byte)message.Data1);
                    trackData.Add((byte)message.Data2);
                    break;

                case SysCommonType.SongSelect:
                    trackData.Add((byte)message.Data1);
                    break;
            }
        }

        private void Write(SysRealtimeMessage message)
        {
            // Escaped messages cancel running status.
            runningStatus = 0;

            // Escaped message.
            trackData.Add((byte)0xF7);

            trackData.Add((byte)message.Status);
        }

        /// <summary>
        /// Gets or sets the Track to write to the Stream.
        /// </summary>
        public Track Track
        {
            get
            {
                return track;
            }
            set
            {

                runningStatus = 0;
                trackData.Clear();

                track = value;
            }
        }
    }
}
