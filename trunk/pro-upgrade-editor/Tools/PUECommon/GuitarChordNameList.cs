using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

using System.Collections;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public class GuitarChordNameList : IEnumerable<GuitarChordName>
    {
        List<GuitarChordName> names;
        GuitarChord owner;

        public GuitarChordNameList(GuitarChord owner)
        {
            this.owner = owner;
            names = new List<GuitarChordName>();
        }

        public void Remove(GuitarChordName name)
        {
            names.Remove(name);
        }

        public void SetNames(IEnumerable<GuitarChordName> names)
        {
            Clear();
            if (names != null)
            {
                names.ForEach(x => internalAddName(x));
            }
        }
        public void Clear()
        {
            names.ToList().ForEach(n => Remove(n));
            names.Clear();
        }

        void internalAddName(GuitarChordName n)
        {
            n.OwnerList = this;
            n.OwnerChord = this.owner;
            names.Add(n);
        }


        public IEnumerator<GuitarChordName> GetEnumerator()
        {
            return names.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}