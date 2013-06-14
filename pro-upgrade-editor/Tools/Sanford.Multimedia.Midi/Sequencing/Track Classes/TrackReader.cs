
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Sanford.Multimedia.Midi
{


    internal class TrackReader
    {
        private Track track = new Track(FileType.Unknown);

        private Track newTrack = new Track(FileType.Unknown);

        private SysCommonMessageBuilder scBuilder = new SysCommonMessageBuilder();

        private Stream stream;

        private byte[] trackData;

        private int trackIndex;

        private int previousTicks;

        private int ticks;

        private int status;

        private int runningStatus;

        public TrackReader()
        {
        }

        public void Read(Stream strm)
        {
            newTrack = new Track(FileType.Unknown);


            stream = strm;
            FindTrack();

            int trackLength = GetTrackLength();
            trackData = new byte[trackLength];



            int result = strm.Read(trackData, 0, trackLength);

            if (result < 0)
            {
                throw new MidiFileException("End of MIDI file unexpectedly reached.");
            }



            ParseTrackData();


            track = newTrack;

        }

        private void FindTrack()
        {
            bool found = false;
            int result;

            while (!found)
            {
                result = stream.ReadByte();

                if (result == 'M')
                {
                    result = stream.ReadByte();

                    if (result == 'T')
                    {
                        result = stream.ReadByte();

                        if (result == 'r')
                        {
                            result = stream.ReadByte();

                            if (result == 'k')
                            {
                                found = true;
                            }
                        }
                    }
                }

                if (result < 0)
                {
                    throw new MidiFileException("Unable to find track in MIDI file.");
                }
            }
        }

        private int GetTrackLength()
        {
            byte[] trackLength = new byte[4];

            int result = stream.Read(trackLength, 0, trackLength.Length);

            if (result < trackLength.Length)
            {
                throw new MidiFileException("End of MIDI file unexpectedly reached.");
            }

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(trackLength);
            }

            return BitConverter.ToInt32(trackLength, 0);
        }

        private void ParseTrackData()
        {
            
            trackIndex = ticks = runningStatus = 0;

            while (trackIndex < trackData.Length)
            {
                previousTicks = ticks;

                ticks += ReadVariableLengthValue();

                if ((trackData[trackIndex] & 0x80) == 0x80)
                {
                    status = trackData[trackIndex];
                    trackIndex++;
                }
                else
                {
                    status = runningStatus;
                }

                ParseMessage();
            }
        }


        private void ParseMessage()
        {
            
            // If this is a channel message.
            if (status >= (int)ChannelCommand.NoteOff &&
                status <= (int)ChannelCommand.PitchWheel +
                ChannelMessage.MidiChannelMaxValue)
            {
                ParseChannelMessage();

            }
            // Else if this is a meta message.
            else if (status == 0xFF)
            {
                ParseMetaMessage();

            }
            // Else if this is the start of a system exclusive message.
            else if (status == (int)SysExType.Start)
            {
                ParseSysExMessageStart();
            }
            // Else if this is a continuation of a system exclusive message.
            else if (status == (int)SysExType.Continuation)
            {
                ParseSysExMessageContinue();
            }
            // Else if this is a system common message.
            else if (status >= (int)SysCommonType.MidiTimeCode &&
                status <= (int)SysCommonType.TuneRequest)
            {
                ParseSysCommonMessage();
            }
            // Else if this is a system realtime message.
            else if (status >= (int)SysRealtimeType.Clock &&
                status <= (int)SysRealtimeType.Reset)
            {
                ParseSysRealtimeMessage();
            }
            
        }

        private void ParseChannelMessage()
        {

            if (trackIndex >= trackData.Length)
            {
                throw new MidiFileException("End of track unexpectedly reached.");
            }

            var command = ChannelMessage.UnpackCommand(status);
            var channel = ChannelMessage.UnpackMidiChannel(status);
            var data1 = trackData[trackIndex];

            trackIndex++;
            var data2 = 0;
            if (ChannelMessage.DataBytesPerType(command) == 2)
            {
                if (trackIndex >= trackData.Length)
                {
                    throw new MidiFileException("End of track unexpectedly reached.");
                }

                data2 = trackData[trackIndex];

                trackIndex++;
            }

            newTrack.Insert(ticks, new ChannelMessage(command, data1, data2, channel));
            runningStatus = status;
            
        }

        private void ParseMetaMessage()
        {
            
            if (trackIndex >= trackData.Length)
            {
                throw new MidiFileException("End of track unexpectedly reached.");
            }

            var type = (MetaType)trackData[trackIndex];

            trackIndex++;

            if (trackIndex >= trackData.Length)
            {
                throw new MidiFileException("End of track unexpectedly reached.");
            }

            if (type == MetaType.EndOfTrack)
            {
                if (previousTicks != ticks)
                {
                    newTrack.EndOfTrackOffset = ticks - previousTicks;
                }

                trackIndex++;
            }

            else
            {
                byte[] data = new byte[ReadVariableLengthValue()];
                Array.Copy(trackData, trackIndex, data, 0, data.Length);

                newTrack.Insert(ticks, new MetaMessage(type, data));

                trackIndex += data.Length;
            }
        }

        private void ParseSysExMessageStart()
        {
            // System exclusive cancels running status.
            runningStatus = 0;

            byte[] data = new byte[ReadVariableLengthValue() + 1];
            data[0] = (byte)SysExType.Start;

            Array.Copy(trackData, trackIndex, data, 1, data.Length - 1);
            newTrack.Insert(ticks, new SysExMessage(data));

            trackIndex += data.Length - 1;
        }

        private void ParseSysExMessageContinue()
        {
            trackIndex++;

            if (trackIndex >= trackData.Length)
            {
                throw new MidiFileException("End of track unexpectedly reached.");
            }

            // System exclusive cancels running status.
            runningStatus = 0;

            // If this is an escaped message rather than a system exclusive 
            // continuation message.
            if ((trackData[trackIndex] & 0x80) == 0x80)
            {
                status = trackData[trackIndex];
                trackIndex++;

                ParseMessage();
            }
            else
            {
                byte[] data = new byte[ReadVariableLengthValue() + 1];
                data[0] = (byte)SysExType.Continuation;

                Array.Copy(trackData, trackIndex, data, 1, data.Length - 1);
                newTrack.Insert(ticks, new SysExMessage(data));

                trackIndex += data.Length - 1;
            }
        }

        private void ParseSysCommonMessage()
        {
            if (trackIndex >= trackData.Length)
            {
                throw new MidiFileException("End of track unexpectedly reached.");
            }

            // System common cancels running status.
            runningStatus = 0;

            scBuilder.Type = (SysCommonType)status;

            switch ((SysCommonType)status)
            {
                case SysCommonType.MidiTimeCode:
                    scBuilder.Data1 = trackData[trackIndex];
                    trackIndex++;
                    break;

                case SysCommonType.SongPositionPointer:
                    scBuilder.Data1 = trackData[trackIndex];
                    trackIndex++;

                    if (trackIndex >= trackData.Length)
                    {
                        throw new MidiFileException("End of track unexpectedly reached.");
                    }

                    scBuilder.Data2 = trackData[trackIndex];
                    trackIndex++;
                    break;

                case SysCommonType.SongSelect:
                    scBuilder.Data1 = trackData[trackIndex];
                    trackIndex++;
                    break;

                case SysCommonType.TuneRequest:
                    // Nothing to do here.
                    break;
            }

            scBuilder.Build();

            newTrack.Insert(ticks, scBuilder.Result);
        }

        private void ParseSysRealtimeMessage()
        {
            newTrack.Insert(ticks, SysRealtimeMessage.FromType((SysRealtimeType)status));
        }

        private int ReadVariableLengthValue()
        {
            if (trackIndex >= trackData.Length)
            {
                throw new MidiFileException("End of track unexpectedly reached.");
            }

            int result = 0;

            result = trackData[trackIndex];

            trackIndex++;

            if ((result & 0x80) == 0x80)
            {
                result &= 0x7F;

                int temp;

                do
                {
                    if (trackIndex >= trackData.Length)
                    {
                        throw new MidiFileException("End of track unexpectedly reached.");
                    }

                    temp = trackData[trackIndex];
                    trackIndex++;
                    result <<= 7;
                    result |= temp & 0x7F;
                } while ((temp & 0x80) == 0x80);
            }

            return result;
        }

        public Track Track
        {
            get
            {
                return track;
            }
        }
    }


}
