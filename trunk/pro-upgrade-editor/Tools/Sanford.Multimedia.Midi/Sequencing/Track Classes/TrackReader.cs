
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
        
        private ChannelMessageBuilder cmBuilder = new ChannelMessageBuilder();

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

        public List<IMidiMessage> Read(Stream strm)
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



            var ret = ParseTrackData();

            
            track = newTrack;

           
            return ret;
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

        private List<IMidiMessage> ParseTrackData()
        {
            var ret = new List<IMidiMessage>();
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

                var m = ParseMessage();
                if (m != null)
                {
                    
                    ret.Add(m);
                }
            }
            return ret;
        }

        
        private IMidiMessage ParseMessage()
        {
            IMidiMessage ret = null;
            // If this is a channel message.
            if (status >= (int)ChannelCommand.NoteOff &&
                status <= (int)ChannelCommand.PitchWheel +
                ChannelMessage.MidiChannelMaxValue)
            {
                ret = ParseChannelMessage();
                
            }
            // Else if this is a meta message.
            else if (status == 0xFF)
            {
                ret = ParseMetaMessage();
                
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
            return ret;
        }

        private ChannelMessage ParseChannelMessage()
        {
            
            if (trackIndex >= trackData.Length)
            {
                throw new MidiFileException("End of track unexpectedly reached.");
            }

            cmBuilder.Command = ChannelMessage.UnpackCommand(status);
            cmBuilder.MidiChannel = ChannelMessage.UnpackMidiChannel(status);
            cmBuilder.Data1 = trackData[trackIndex];

            trackIndex++;

            if (ChannelMessage.DataBytesPerType(cmBuilder.Command) == 2)
            {
                if (trackIndex >= trackData.Length)
                {
                    throw new MidiFileException("End of track unexpectedly reached.");
                }

                cmBuilder.Data2 = trackData[trackIndex];
                
                trackIndex++;
            }

            cmBuilder.Build();
            newTrack.Insert(ticks, cmBuilder.Result);
            runningStatus = status;
            return cmBuilder.Result;
        }
        
        private MetaMessage ParseMetaMessage()
        {
            MetaMessage ret = null;
            if (trackIndex >= trackData.Length)
            {
                throw new MidiFileException("End of track unexpectedly reached.");
            }

            MetaType type = (MetaType)trackData[trackIndex];

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

                ret = new MetaMessage(type, data);
                newTrack.Insert(ticks, ret);  

                trackIndex += data.Length;

            }
            return ret;
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
            SysRealtimeMessage e = null;

            switch ((SysRealtimeType)status)
            {
                case SysRealtimeType.ActiveSense:
                    e = SysRealtimeMessage.ActiveSenseMessage;
                    break;

                case SysRealtimeType.Clock:
                    e = SysRealtimeMessage.ClockMessage;
                    break;

                case SysRealtimeType.Continue:
                    e = SysRealtimeMessage.ContinueMessage;
                    break;

                case SysRealtimeType.Reset:
                    e = SysRealtimeMessage.ResetMessage;
                    break;

                case SysRealtimeType.Start:
                    e = SysRealtimeMessage.StartMessage;
                    break;

                case SysRealtimeType.Stop:
                    e = SysRealtimeMessage.StopMessage;
                    break;

                case SysRealtimeType.Tick:
                    e = SysRealtimeMessage.TickMessage;
                    break;
            }

            newTrack.Insert(ticks, e);
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
