// License

/* Copyright (c) 2005 Leslie Sanford
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software. 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
 */

//

// Contact

/*
 * Leslie Sanford
 * Email: jabberdabber@hotmail.com
 */

//

using System;
using System.Text;

namespace Sanford.Multimedia.Midi
{
    /// <summary>
    /// Provides functionality for building meta text messages.
    /// </summary>
    public class MetaTextBuilder : IMessageBuilder
    {
        public static MetaMessage Create(MetaType type, string str)
        {
            var tb = new MetaTextBuilder(type, str);
            
            return tb.Result;
        }

        // MetaTextBuilder Members

        // Fields

        // The text represented by the MetaMessage.
        private string text;

        // The MetaMessage type - must be one of the text based types.
        private MetaType type = MetaType.Text;

        
        //

        // Construction

        /// <summary>
        /// Initializes a new instance of the MetaMessageTextBuilder class.
        /// </summary>
        public MetaTextBuilder()
        {
            text = string.Empty;
        }

        public MetaTextBuilder(MetaType type)
        {
            this.text = string.Empty;
            this.type = type;
        }

        public MetaTextBuilder(MetaType type, string text)
        {
            this.type = type;

            if (text != null)
            {
                this.text = text;
            }
            else
            {
                this.text = string.Empty;
            }
        }

        public MetaTextBuilder(MetaMessage message)
        {
            Initialize(message);
        }

        //

        // Methods

        /// <summary>
        /// Initializes the MetaMessageTextBuilder with the specified MetaMessage.
        /// </summary>
        /// <param name="message">
        /// The MetaMessage to use for initializing the MetaMessageTextBuilder.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the MetaMessage is not a text based type.
        /// </exception>
        public void Initialize(MetaMessage message)
        {
            text = message.Text;
            this.type = message.MetaType;
        }

        /// <summary>
        /// Indicates whether or not the specified MetaType is a text based 
        /// type.
        /// </summary>
        /// <param name="type">
        /// The MetaType to test.
        /// </param>
        /// <returns>
        /// <b>true</b> if the MetaType is a text based type; 
        /// otherwise, <b>false</b>.
        /// </returns>
        private bool IsTextType(MetaType type)
        {
            bool result;

            if (type == MetaType.Copyright ||
                type == MetaType.CuePoint ||
                type == MetaType.DeviceName ||
                type == MetaType.InstrumentName ||
                type == MetaType.Lyric ||
                type == MetaType.Marker ||
                type == MetaType.ProgramName ||
                type == MetaType.Text ||
                type == MetaType.TrackName)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        //

        // Properties

        /// <summary>
        /// Gets or sets the text for the MetaMessage.
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value ?? "";
            }
        }

        public MetaType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;

            }
        }

        public MetaMessage Result
        {
            get
            {
                return new MetaMessage(Type, Encoding.ASCII.GetBytes(text));
            }
        }


        //
    }
}
