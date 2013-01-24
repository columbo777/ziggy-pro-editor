
using System;
using System.Diagnostics;
using System.IO;

namespace Sanford.Multimedia.Midi
{
    /// <summary>
    /// Defintes constants representing SMPTE frame rates.
    /// </summary>
    public enum SmpteFrameRate
    {
        Smpte24 = 24,
        Smpte25 = 25,
        Smpte30Drop = 29,
        Smpte30 = 30
    }

    /// <summary>
    /// The different types of sequences.
    /// </summary>
    public enum SequenceType
    {
        Ppqn,
        Smpte
    }

    /// <summary>
    /// Represents MIDI file properties.
    /// </summary>
    internal class MidiFileProperties
    {
        private const int PropertyLength = 2;

        private static readonly byte[] MidiFileHeader =
            {
                (byte)'M',
                (byte)'T',
                (byte)'h',
                (byte)'d',
                0, 
                0, 
                0,
                6
            };

        private int format = 1;

        private int trackCount = 0;

        private int division = PpqnClock.DefaultPpqnValue;

        private SequenceType sequenceType = SequenceType.Ppqn;

        public MidiFileProperties()
        {
        }

        public void Read(Stream strm)
        {


            format = trackCount = division = 0;

            FindHeader(strm);
            Format = (int)ReadProperty(strm);
            TrackCount = (int)ReadProperty(strm);
            Division = (int)ReadProperty(strm);

        }

        private void FindHeader(Stream stream)
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

                        if (result == 'h')
                        {
                            result = stream.ReadByte();

                            if (result == 'd')
                            {
                                found = true;
                            }
                        }
                    }
                }

                if (result < 0)
                {
                    throw new MidiFileException("Unable to find MIDI file header.");
                }
            }

            // Eat the header length.
            for (int i = 0; i < 4; i++)
            {
                if (stream.ReadByte() < 0)
                {
                    throw new MidiFileException("Unable to find MIDI file header.");
                }
            }
        }

        private ushort ReadProperty(Stream strm)
        {
            byte[] data = new byte[PropertyLength];

            int result = strm.Read(data, 0, data.Length);

            if (result != data.Length)
            {
                throw new MidiFileException("End of MIDI file unexpectedly reached.");
            }

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }

            return BitConverter.ToUInt16(data, 0);
        }

        public void Write(Stream strm)
        {


            strm.Write(MidiFileHeader, 0, MidiFileHeader.Length);
            WriteProperty(strm, (ushort)Format);
            WriteProperty(strm, (ushort)TrackCount);
            WriteProperty(strm, (ushort)Division);
        }

        private void WriteProperty(Stream strm, ushort property)
        {
            byte[] data = BitConverter.GetBytes(property);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }

            strm.Write(data, 0, PropertyLength);
        }

        private static bool IsSmpte(int division)
        {
            bool result;
            byte[] data = BitConverter.GetBytes((short)division);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }

            if ((sbyte)data[0] < 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }



        public int Format
        {
            get
            {
                return format;
            }
            set
            {


                format = value;


            }
        }

        public int TrackCount
        {
            get
            {
                return trackCount;
            }
            set
            {


                trackCount = value;


            }
        }

        public int Division
        {
            get
            {
                return division;
            }
            set
            {
                if (IsSmpte(value))
                {
                    byte[] data = BitConverter.GetBytes((short)value);

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(data);
                    }

                    if ((sbyte)data[0] != -(int)SmpteFrameRate.Smpte24 &&
                        (sbyte)data[0] != -(int)SmpteFrameRate.Smpte25 &&
                        (sbyte)data[0] != -(int)SmpteFrameRate.Smpte30 &&
                        (sbyte)data[0] != -(int)SmpteFrameRate.Smpte30Drop)
                    {
                        throw new ArgumentException("Invalid SMPTE frame rate.");
                    }
                    else
                    {
                        sequenceType = SequenceType.Smpte;
                    }
                }
                else
                {

                    sequenceType = SequenceType.Ppqn;

                }

                division = value;


            }
        }

        public SequenceType SequenceType
        {
            get
            {
                return sequenceType;
            }
        }
    }

    public class MidiFileException : ApplicationException
    {
        public MidiFileException(string message)
            : base(message)
        {
        }
    }
}
