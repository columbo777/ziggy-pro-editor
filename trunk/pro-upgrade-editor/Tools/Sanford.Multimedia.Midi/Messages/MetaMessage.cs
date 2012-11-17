
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
        ProprietaryEvent = 0x7F
    }

    #endregion

	/// <summary>
	/// Represents MIDI meta messages.
	/// </summary>
	/// <remarks>
	/// Meta messages are MIDI messages that are stored in MIDI files. These
	/// messages are not sent or received via MIDI but are read and 
	/// interpretted from MIDI files. They provide information that describes 
	/// a MIDI file's properties. For example, tempo changes are implemented
	/// using meta messages.
	/// </remarks>
	[ImmutableObject(true)]
	public sealed class MetaMessage : IMidiMessage
	{
        #region MetaMessage Members

        #region Constants

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

        #endregion

        #region Class Fields

        /// <summary>
        /// End of track meta message.
        /// </summary>
        public static readonly MetaMessage EndOfTrackMessage = 
            new MetaMessage(MetaType.EndOfTrack, new byte[0]);

        #endregion

        #region Fields

        // The meta message type.
        private MetaType type;

        // The meta message data.
        private byte[] data;

        // The hash code value.
        private int hashCode;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the MetaMessage class.
        /// </summary>
        /// <param name="type">
        /// The type of MetaMessage.
        /// </param>
        /// <param name="data">
        /// The MetaMessage data.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The length of the MetaMessage is not valid for the MetaMessage type.
        /// </exception>
        /// <remarks>
        /// Each MetaMessage has type and length properties. For certain 
        /// types, the length of the message data must be a specific value. For
        /// example, tempo messages must have a data length of exactly three. 
        /// Some MetaMessage types can have any data length. Text messages are
        /// an example of a MetaMessage that can have a variable data length.
        /// When a MetaMessage is created, the length of the data is checked
        /// to make sure that it is valid for the specified type. If it is not,
        /// an exception is thrown. 
        /// </remarks>
		public MetaMessage(MetaType type, byte[] data)
        {
            
            this.type = type;
            
            // Create storage for meta message data.
            this.data = new byte[data.Length];

            // Copy data into storage.
            data.CopyTo(this.data, 0);

        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Gets a copy of the data bytes for this meta message.
        /// </summary>
        /// <returns>
        /// A copy of the data bytes for this meta message.
        /// </returns>
        public byte[] GetBytes()
        {
            return (byte[])data.Clone();
        }

        /// <summary>
        /// Returns a value for the current MetaMessage suitable for use in 
        /// hashing algorithms.
        /// </summary>
        /// <returns>
        /// A hash code for the current MetaMessage.
        /// </returns>
        public override int GetHashCode()
        {
            return hashCode;            
        }

        /// <summary>
        /// Determines whether two MetaMessage instances are equal.
        /// </summary>
        /// <param name="obj">
        /// The MetaMessage to compare with the current MetaMessage.
        /// </param>
        /// <returns>
        /// <b>true</b> if the specified MetaMessage is equal to the current 
        /// MetaMessage; otherwise, <b>false</b>.
        /// </returns>
        public override bool Equals(object obj)
        {
            
            bool equal = true;
            MetaMessage message = (MetaMessage)obj;

            // If the types do not match.
            if(MetaType != message.MetaType)
            {
                // The messages are not equal
                equal = false;
            }

            // If the message lengths are not equal.
            if(equal && Length != message.Length)
            {
                // The message are not equal.
                equal = false;
            }

            // Check to see if the data is equal.
            for(int i = 0; i < Length && equal; i++)
            {
                // If a data value does not match.
                if(this[i] != message[i])
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

            for(int i = 0; i < data.Length; i += 3)
            {
                hashCode ^= data[i];
            }

            for(int i = 1; i < data.Length; i += 3)
            {
                hashCode ^= data[i] << Shift;
            }

            for(int i = 2; i < data.Length; i += 3)
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
            if (!string.IsNullOrEmpty(Text))
            {
                return Text;
            }
            else
            {
                return MetaType.ToString();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// index is less than zero or greater than or equal to Length.
        /// </exception>
        public byte this[int index]
        {
            get
            {
              

                return data[index];
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

        #endregion

        #endregion        

        #region IMidiMessage Members

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

        #endregion
    }
}
