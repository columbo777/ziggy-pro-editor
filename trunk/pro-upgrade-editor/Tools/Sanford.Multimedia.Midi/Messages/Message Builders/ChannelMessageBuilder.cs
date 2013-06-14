#region License

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

#endregion

#region Contact

/*
 * Leslie Sanford
 * Email: jabberdabber@hotmail.com
 */

#endregion

using System;
using System.Collections;

namespace Sanford.Multimedia.Midi
{
    /// <summary>
    /// Provides functionality for building ChannelMessages.
    /// </summary>
    public class ChannelMessageBuilder : IMessageBuilder
    {
        
        // The channel message as a packed integer.
        private int message = 0;

        public ChannelMessageBuilder()
        {
            Command = ChannelCommand.Controller;
            MidiChannel = 0;
            Data1 = (int)ControllerType.AllSoundOff;
            Data2 = 0;
        }

        public ChannelMessageBuilder(ChannelMessage message)
        {
            Initialize(message);
        }

        public ChannelMessageBuilder Initialize(ChannelMessage message)
        {
            this.message = message.Message;
            return this;
        }
        public ChannelMessageBuilder Initialize(MidiEvent ev)
        {
            this.message = ev.MessageData;
            return this;
        }
        
        public ChannelMessage Result
        {
            get
            {
                return new ChannelMessage(message);
            }
        }

        internal int Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }

        public ChannelCommand Command
        {
            get
            {
                return ChannelMessage.UnpackCommand(message);
            }
            set
            {
                message = ChannelMessage.PackCommand(message, value);
            }
        }

        public int MidiChannel
        {
            get
            {
                return ChannelMessage.UnpackMidiChannel(message);
            }
            set
            {
                message = ChannelMessage.PackMidiChannel(message, value);
            }
        }

        public int Data1
        {
            get
            {
                return ShortMessage.UnpackData1(message);
            }
            set
            {
                message = ShortMessage.PackData1(message, value);
            }
        }

        public int Data2
        {
            get
            {
                return ShortMessage.UnpackData2(message);
            }
            set
            {
                message = ShortMessage.PackData2(message, value);
            }
        }

    }
}
