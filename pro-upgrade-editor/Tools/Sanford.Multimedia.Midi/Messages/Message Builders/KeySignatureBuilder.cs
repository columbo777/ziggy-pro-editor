#region License

/* Copyright (c) 2006 Leslie Sanford
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
using Sanford.Multimedia;

namespace Sanford.Multimedia.Midi
{
    /// <summary>
    /// Builds key signature MetaMessages.
    /// </summary>
    public class KeySignatureBuilder : IMessageBuilder
    {
        private Key key = Key.CMajor;

        private MetaMessage result = null;

        /// <summary>
        /// Initializes a new instance of the KeySignatureBuilder class.
        /// </summary>
        public KeySignatureBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the KeySignatureBuilder class with 
        /// the specified key signature MetaMessage.
        /// </summary>
        /// <param name="message">
        /// The key signature MetaMessage to use for initializing the 
        /// KeySignatureBuilder class.
        /// </param>
        public KeySignatureBuilder(MetaMessage message)
        {
            Initialize(message);
        }

        /// <summary>
        /// Initializes the KeySignatureBuilder with the specified MetaMessage.
        /// </summary>
        /// <param name="message">
        /// The key signature MetaMessage to use for initializing the 
        /// KeySignatureBuilder.
        /// </param>
        public void Initialize(MetaMessage message)
        {
            var ks = new KeySignature(message);
            key = ks.Key;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public Key Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }

        /// <summary>
        /// The build key signature MetaMessage.
        /// </summary>
        public MetaMessage Result
        {
            get
            {
                return result;
            }
        }

        #region IMessageBuilder Members

        /// <summary>
        /// Builds the key signature MetaMessage.
        /// </summary>
        public void Build()
        {
            byte[] data = new byte[MetaMessage.KeySigLength];

            unchecked
            {
                switch (Key)
                {
                    case Key.CFlatMajor:
                        data[0] = (byte)-7;
                        data[1] = 0;
                        break;

                    case Key.GFlatMajor:
                        data[0] = (byte)-6;
                        data[1] = 0;
                        break;

                    case Key.DFlatMajor:
                        data[0] = (byte)-5;
                        data[1] = 0;
                        break;

                    case Key.AFlatMajor:
                        data[0] = (byte)-4;
                        data[1] = 0;
                        break;

                    case Key.EFlatMajor:
                        data[0] = (byte)-3;
                        data[1] = 0;
                        break;

                    case Key.BFlatMajor:
                        data[0] = (byte)-2;
                        data[1] = 0;
                        break;

                    case Key.FMajor:
                        data[0] = (byte)-1;
                        data[1] = 0;
                        break;

                    case Key.CMajor:
                        data[0] = 0;
                        data[1] = 0;
                        break;

                    case Key.GMajor:
                        data[0] = 1;
                        data[1] = 0;
                        break;

                    case Key.DMajor:
                        data[0] = 2;
                        data[1] = 0;
                        break;

                    case Key.AMajor:
                        data[0] = 3;
                        data[1] = 0;
                        break;

                    case Key.EMajor:
                        data[0] = 4;
                        data[1] = 0;
                        break;

                    case Key.BMajor:
                        data[0] = 5;
                        data[1] = 0;
                        break;

                    case Key.FSharpMajor:
                        data[0] = 6;
                        data[1] = 0;
                        break;

                    case Key.CSharpMajor:
                        data[0] = 7;
                        data[1] = 0;
                        break;

                    case Key.AFlatMinor:
                        data[0] = (byte)-7;
                        data[1] = 1;
                        break;

                    case Key.EFlatMinor:
                        data[0] = (byte)-6;
                        data[1] = 1;
                        break;

                    case Key.BFlatMinor:
                        data[0] = (byte)-5;
                        data[1] = 1;
                        break;

                    case Key.FMinor:
                        data[0] = (byte)-4;
                        data[1] = 1;
                        break;

                    case Key.CMinor:
                        data[0] = (byte)-3;
                        data[1] = 1;
                        break;

                    case Key.GMinor:
                        data[0] = (byte)-2;
                        data[1] = 1;
                        break;

                    case Key.DMinor:
                        data[0] = (byte)-1;
                        data[1] = 1;
                        break;

                    case Key.AMinor:
                        data[0] = 1;
                        data[1] = 0;
                        break;

                    case Key.EMinor:
                        data[0] = 1;
                        data[1] = 1;
                        break;

                    case Key.BMinor:
                        data[0] = 2;
                        data[1] = 1;
                        break;

                    case Key.FSharpMinor:
                        data[0] = 3;
                        data[1] = 1;
                        break;

                    case Key.CSharpMinor:
                        data[0] = 4;
                        data[1] = 1;
                        break;

                    case Key.GSharpMinor:
                        data[0] = 5;
                        data[1] = 1;
                        break;

                    case Key.DSharpMinor:
                        data[0] = 6;
                        data[1] = 1;
                        break;

                    case Key.ASharpMinor:
                        data[0] = 7;
                        data[1] = 1;
                        break;
                }
            }

            result = new MetaMessage(MetaType.KeySignature, data);
        }

        #endregion
    }
}
