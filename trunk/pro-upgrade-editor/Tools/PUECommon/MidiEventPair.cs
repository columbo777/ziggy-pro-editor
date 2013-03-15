using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;

using System.Runtime.CompilerServices;
using System.Globalization;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public struct MidiEventPair : DownUpPair<MidiEvent>
    {
        GuitarMessageList owner;
        MidiEvent down;
        MidiEvent up;
        Data1ChannelPair dPair;


        public MidiEventPair(GuitarMessageList owner, MidiEventPair pair) : this(owner, pair.down, pair.up) { }
        public MidiEventPair(MidiEventPair pair) : this(pair.owner, pair.down, pair.up) { }
        public MidiEventPair(GuitarMessageList owner) : this(owner, null, null) { }
        public MidiEventPair(GuitarMessageList owner, MidiEvent down) : this(owner, down, null) { }
        public MidiEventPair(GuitarMessageList owner, MidiEvent down, MidiEvent up)
        {
            this.owner = owner;
            this.down = down;
            this.up = up;
            if (down == null)
            {
                dPair = Data1ChannelPair.NullValue;
            }
            else
            {
                dPair = new Data1ChannelPair(down.Data1, down.Channel);
            }
        }

        public GuitarTrack OwnerTrack { get { return owner.Owner.GuitarTrack; } }

        public int Data1 { get { return Down != null ? Down.Data1 : Int32.MinValue; } }
        public int Data2 { get { return Down != null ? Down.Data2 : Int32.MinValue; } }

        public int Channel { get { return Down != null ? Down.Channel : Int32.MinValue; } }

        public Data1ChannelPair Data1ChannelPair { get { return dPair; } }

        public bool HasDown { get { return Down != null; } }
        public bool HasUp { get { return Up != null; } }

        public bool IsValid { get { return (HasUp || HasDown) && !IsDeleted; } }

        public bool IsDeleted { get { return IsDownDeleted || IsUpDeleted; } }

        public bool IsUpDeleted { get { return HasUp ? Up.Deleted : false; } }
        public bool IsDownDeleted { get { return HasDown ? Down.Deleted : false; } }

        public MidiEventPair GetInsertedClone(GuitarMessageList owner)
        {
            return new MidiEventPair(owner,
                HasDown ? Down.GetInsertedClone(owner) : null,
                HasUp ? Up.GetInsertedClone(owner) : null);
        }

        public MidiEventPair CloneToMemory(GuitarMessageList owner)
        {
            return new MidiEventPair(owner,
                HasDown ? new MidiEvent(null, Down.AbsoluteTicks, Down.Clone()) : null,
                HasUp ? new MidiEvent(null, Up.AbsoluteTicks, Up.Clone()) : null);
        }

        public MidiEvent Down
        {
            get { return this.down; }
            set { this.down = value; }
        }

        public MidiEvent Up
        {
            get { return this.up; }
            set { this.up = value; }
        }


        public bool IsNull
        {
            get { return !(HasUp || HasDown); }
        }

        public int DownTick { get { return Down != null ? Down.AbsoluteTicks : Int32.MinValue; } }
        public int UpTick { get { return Up != null ? Up.AbsoluteTicks : Down.AbsoluteTicks; } }

        public int TickLength { get { return UpTick - DownTick; } }
    }
}