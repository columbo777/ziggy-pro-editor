using System;
using System.Collections.Generic;

namespace PhoneGuitarTab.Tablature
{
    public class Measure
    {
        //TODO: Metadata of measure

        public int StringsNumber { get; set; } //TODO: ambiguous usage

        public Byte NumeratorSignature { get; set; }
        public Byte DenominatorSignature { get; set; }

        public List<Beat> Beats { get; set; }

        public Measure()
        {
            Beats = new List<Beat>();
        }
    }
}
