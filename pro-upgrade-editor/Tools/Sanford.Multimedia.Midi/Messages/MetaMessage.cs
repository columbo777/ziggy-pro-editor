
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sanford.Multimedia.Midi
{
    #region Meta Message Types

    /// <summary>
    /// Represents MetaMessage types.
    /// </summary>
    public enum MetaType
    {

        /// <summary>
        /// Represents sequencer number type.
        /// </summary>
        SequenceNumber,

        /// <summary>
        /// Represents the text type.
        /// </summary>
        Text,

        /// <summary>
        /// Represents the copyright type.
        /// </summary>
        Copyright,

        /// <summary>
        /// Represents the track name type.
        /// </summary>
        TrackName,

        /// <summary>
        /// Represents the instrument name type.
        /// </summary>
        InstrumentName,

        /// <summary>
        /// Represents the lyric type.
        /// </summary>
        Lyric,

        /// <summary>
        /// Represents the marker type.
        /// </summary>
        Marker,

        /// <summary>
        /// Represents the cue point type.
        /// </summary>
        CuePoint,

        /// <summary>
        /// Represents the program name type.
        /// </summary>
        ProgramName,

        /// <summary>
        /// Represents the device name type.
        /// </summary>
        DeviceName,

        /// <summary>
        /// Represents then end of track type.
        /// </summary>
        EndOfTrack = 0x2F,

        /// <summary>
        /// Represents the tempo type.
        /// </summary>
        Tempo = 0x51,

        /// <summary>
        /// Represents the Smpte offset type.
        /// </summary>
        SmpteOffset = 0x54,

        /// <summary>
        /// Represents the time signature type.
        /// </summary>
        TimeSignature = 0x58,

        /// <summary>
        /// Represents the key signature type.
        /// </summary>
        KeySignature,

        /// <summary>
        /// Represents the proprietary event type.
        /// </summary>
        ProprietaryEvent = 0x7F,

        Unknown = 0xFF,
    }

    #endregion

    public class KeySignature
    {


        public KeySignature(MetaMessage msg)
        {
            var b = msg.GetBytes();
            IsMajor = (((sbyte)b[1]) == 0);
            Key = FromBytes(b[0], b[1]);
        }
        public KeySignature(byte dataKey, byte dataMajor)
        {
            IsMajor = ((sbyte)dataMajor) == 0;
            Key = FromBytes(dataKey, dataMajor);
        }

        public static Key FromBytes(byte dataKey, byte dataMajor)
        {
            Key ret = (Key)0;
            var major = ((sbyte)dataMajor == 0);
            // If the key is major.
            if (major)
            {
                switch ((sbyte)dataKey)
                {
                    case -7:
                        ret = Key.CFlatMajor;
                        break;

                    case -6:
                        ret = Key.GFlatMajor;
                        break;

                    case -5:
                        ret = Key.DFlatMajor;
                        break;

                    case -4:
                        ret = Key.AFlatMajor;
                        break;

                    case -3:
                        ret = Key.EFlatMajor;
                        break;

                    case -2:
                        ret = Key.BFlatMajor;
                        break;

                    case -1:
                        ret = Key.FMajor;
                        break;

                    case 0:
                        ret = Key.CMajor;
                        break;

                    case 1:
                        ret = Key.GMajor;
                        break;

                    case 2:
                        ret = Key.DMajor;
                        break;

                    case 3:
                        ret = Key.AMajor;
                        break;

                    case 4:
                        ret = Key.EMajor;
                        break;

                    case 5:
                        ret = Key.BMajor;
                        break;

                    case 6:
                        ret = Key.FSharpMajor;
                        break;

                    case 7:
                        ret = Key.CSharpMajor;
                        break;
                }

            }
            // Else the Key is minor.
            else
            {
                switch ((sbyte)dataKey)
                {
                    case -7:
                        ret = Key.AFlatMinor;
                        break;

                    case -6:
                        ret = Key.EFlatMinor;
                        break;

                    case -5:
                        ret = Key.BFlatMinor;
                        break;

                    case -4:
                        ret = Key.FMinor;
                        break;

                    case -3:
                        ret = Key.CMinor;
                        break;

                    case -2:
                        ret = Key.GMinor;
                        break;

                    case -1:
                        ret = Key.DMinor;
                        break;

                    case 0:
                        ret = Key.AMinor;
                        break;

                    case 1:
                        ret = Key.EMinor;
                        break;

                    case 2:
                        ret = Key.BMinor;
                        break;

                    case 3:
                        ret = Key.FSharpMinor;
                        break;

                    case 4:
                        ret = Key.CSharpMinor;
                        break;

                    case 5:
                        ret = Key.GSharpMinor;
                        break;

                    case 6:
                        ret = Key.DSharpMinor;
                        break;

                    case 7:
                        ret = Key.ASharpMinor;
                        break;
                }
            }
            return ret;
        }

        public Key Key { get; internal set; }
        public bool IsMajor { get; internal set; }
    }

    [ImmutableObject(true)]
    public sealed class MetaMessage : IMidiMessage
    {
        /// <summary>
        /// The amount to shift data bytes when calculating the hash code.
        /// </summary>
        private const int Shift = 7;

        //
        // Meta message length constants.
        //

        /// <summary>
        /// Length in bytes for tempo meta message data.
        /// </summary>
        public const int TempoLength = 3;

        /// <summary>
        /// Length in bytes for SMPTE offset meta message data.
        /// </summary>
        public const int SmpteOffsetLength = 5;

        /// <summary>
        /// Length in bytes for time signature meta message data.
        /// </summary>
        public const int TimeSigLength = 4;

        /// <summary>
        /// Length in bytes for key signature meta message data.
        /// </summary>
        public const int KeySigLength = 2;

        /// <summary>
        /// End of track meta message.
        /// </summary>
        public static readonly MetaMessage EndOfTrackMessage =
            new MetaMessage(MetaType.EndOfTrack, new byte[0]);

        // The meta message type.
        private MetaType type;

        // The meta message data.
        private byte[] data;

        // The hash code value.
        private int hashCode;

        public MetaMessage(MetaType type, byte[] data)
        {
            this.type = type;
            this.data = data;
        }

        public MetaMessage(MetaType type, string data)
            : this(type, System.Text.Encoding.ASCII.GetBytes(data))
        {
        }


        public byte[] GetBytes()
        {
            return data;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            bool equal = true;
            MetaMessage message = (MetaMessage)obj;

            // If the types do not match.
            if (MetaType != message.MetaType)
            {
                // The messages are not equal
                equal = false;
            }

            // If the message lengths are not equal.
            if (equal && Length != message.Length)
            {
                // The message are not equal.
                equal = false;
            }

            // Check to see if the data is equal.
            for (int i = 0; i < Length && equal; i++)
            {
                // If a data value does not match.
                if (this.data[i] != message.data[i])
                {
                    // The messages are not equal.
                    equal = false;
                }
            }

            return equal;
        }

        // Calculates the hash code.
        private void CalculateHashCode()
        {
            // TODO: This algorithm may need work.

            hashCode = (int)MetaType;

            for (int i = 0; i < data.Length; i += 3)
            {
                hashCode ^= data[i];
            }

            for (int i = 1; i < data.Length; i += 3)
            {
                hashCode ^= data[i] << Shift;
            }

            for (int i = 2; i < data.Length; i += 3)
            {
                hashCode ^= data[i] << Shift * 2;
            }
        }

        public string Text
        {
            get
            {
                if (MetaType == Midi.MetaType.TrackName ||
                    MetaType == Midi.MetaType.Text ||
                    MetaType == Midi.MetaType.Marker ||
                    MetaType == Midi.MetaType.Lyric ||
                    MetaType == Midi.MetaType.InstrumentName ||
                    MetaType == Midi.MetaType.DeviceName ||
                    MetaType == Midi.MetaType.CuePoint)
                {
                    var gb = GetBytes();
                    if (gb != null && gb.Length > 0)
                    {
                        return System.Text.Encoding.ASCII.GetString(gb);
                    }
                }
                return string.Empty;
            }
        }

        public override string ToString()
        {
            var ret = Text;
            if (!string.IsNullOrEmpty(ret))
            {
                return ret;
            }
            else
            {
                return MetaType.ToString();
            }
        }

        /// <summary>
        /// Gets the length of the meta message.
        /// </summary>
        public int Length
        {
            get
            {
                return data.Length;
            }
        }

        /// <summary>
        /// Gets the type of meta message.
        /// </summary>
        public MetaType MetaType
        {
            get
            {
                return type;
            }
        }

        /// <summary>
        /// Gets the status value.
        /// </summary>
        public int Status
        {
            get
            {
                // All meta messages have the same status value (0xFF).
                return 0xFF;
            }
        }

        /// <summary>
        /// Gets the MetaMessage's MessageType.
        /// </summary>
        public MessageType MessageType
        {
            get
            {
                return MessageType.Meta;
            }
        }

        public KeySignature KeySignature
        {
            get { return IsKeySignature ? new KeySignature(data[0], data[1]) : null; }
        }

        public bool IsKeySignature { get { return MetaType == Midi.MetaType.KeySignature; } }

    }
}
