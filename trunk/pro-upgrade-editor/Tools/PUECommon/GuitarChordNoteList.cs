using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

using System.Drawing;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public class GuitarChordNoteList : IEnumerable<GuitarNote>
    {
        List<GuitarNote> notes;
        GuitarChord owner;

        public bool NotesAligned
        {
            get
            {
                var tp = notes.GetTickPair();
                return notes.All(x => x.DownTick == tp.Down && x.UpTick == tp.Up);
            }
        }
        public int[] FretArray
        {
            get
            {
                var ret = Utility.Null6;
                notes.ForEach(x => ret[x.NoteString] = x.NoteFretDown);
                return ret;
            }
        }
        public int[] ChannelArray
        {
            get
            {
                var ret = Utility.Null6;
                notes.ForEach(x => ret[x.NoteString] = x.Channel);
                return ret;
            }
        }
        public int[] FretArrayZero
        {
            get
            {
                return FretArray.Select(x => x.IsNull() == false ? 0 : x).ToArray();
            }
        }
        public int[] ChannelArrayZero
        {
            get
            {
                return ChannelArray.Select(x => x.IsNull() == false ? 0 : x).ToArray();
            }
        }

        public int[] GetFretsAtStringOffset(int offset)
        {
            var ret = Utility.Null6;
            notes.ForEach(x =>
            {
                var idx = x.NoteString + offset;
                if (idx >= 0 && idx <= 5)
                {
                    ret[idx] = x.NoteFretDown;
                }
            });
            return ret;
        }
        public int[] GetChannelsAtStringOffset(int offset)
        {
            var ret = Utility.Null6;
            notes.ForEach(x =>
            {
                var idx = x.NoteString + offset;
                if (idx >= 0 && idx <= 5)
                {
                    ret[idx] = x.Channel;
                }
            });
            return ret;
        }
        public GuitarChordNoteList(GuitarChord owner)
        {
            this.owner = owner;
            notes = new List<GuitarNote>();
        }

        public void Remove(GuitarNote note)
        {
            notes.Remove(note);
        }

        public void SetNotes(IEnumerable<GuitarNote> notes)
        {
            Clear();
            if (notes != null)
            {
                notes.ForEach(x => internalAddNote(x));
            }
        }
        public void Clear()
        {
            notes.ToList().ForEach(n => Remove(n));
            notes.Clear();
        }

        void internalAddNote(GuitarNote n)
        {
            n.OwnerList = this;
            n.OwnerChord = this.owner;
            notes.Add(n);
        }

        public GuitarNote this[int noteString]
        {
            get
            {
                if (noteString < 0 || noteString > 5)
                    return null;

                return notes.FirstOrDefault(x => x.NoteString == noteString);
            }
        }

        public IEnumerator<GuitarNote> GetEnumerator()
        {
            return notes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}