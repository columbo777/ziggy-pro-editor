using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    [Serializable()]
    public class ChordNameMeta
    {
        public string RootNote { get; set; }

        public bool IsSharp { get; set; }
        public bool IsFlat { get; set; }

        public bool IsSuper { get; set; }

        public string Number { get; set; }
        public string Super { get; set; }

        public ChordNameMeta Clone()
        {
            return new ChordNameMeta()
            {
                RootNote = this.RootNote,
                IsSharp = this.IsSharp,
                IsFlat = this.IsFlat,
                IsSuper = this.IsSuper,
                Number = this.Number,
                Super = this.Super,
            };
        }

        
        public ToneNameEnum ToneName
        {
            set
            {
                var val = value.ToString();
                RootNote = val.First().ToString();
                IsFlat = value.IsFlat();
            }
            get
            {
                var ret = ToneNameEnum.NotSet;
                if (RootNote.IsNotEmpty())
                {
                    switch (RootNote.First())
                    {
                        case 'E':
                            ret = ToneNameEnum.E;
                            break;
                        case 'F':
                            ret = ToneNameEnum.F;
                            break;
                        case 'G':
                            ret = ToneNameEnum.G;
                            break;
                        case 'A':
                            ret = ToneNameEnum.A;
                            break;
                        case 'B':
                            ret = ToneNameEnum.B;
                            break;
                        case 'C':
                            ret = ToneNameEnum.C;
                            break;
                        case 'D':
                            ret = ToneNameEnum.D;
                            break;
                    }
                    if (IsFlat)
                    {
                        while (!ret.IsFlat())
                        {
                            ret = ret.PreviousSemiNote();
                        }
                    }
                    else if (IsSharp)
                    {
                        while (!ret.IsFlat())
                        {
                            ret = ret.NextSemiNote();
                        }
                    }
                }
                return ret;
            }
        }
        
        public ChordNameMeta()
        {
            RootNote = string.Empty;
            IsSharp = false;
            IsFlat = false;
            Number = string.Empty;
            Super = string.Empty;
            IsSuper = false;
        }

        public override string ToString()
        {
            var ret = new StringBuilder();
            ret.Append(RootNote);
            if (IsSharp)
            {
                ret.Append("# ");
            }
            else if (IsFlat)
            {
                ret.Append("b ");
            }
            if (Number.IsNotEmpty())
            {
                ret.Append(Number);
            }
            if (Super.IsNotEmpty())
            {
                ret.Append(Super);
            }
            return ret.ToString().Trim();
        }
    }
}