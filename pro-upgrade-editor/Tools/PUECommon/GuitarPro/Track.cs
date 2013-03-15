using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;


namespace PhoneGuitarTab.Tablature
{
    public class Track
    {
        //TODO: Metadata of track

        public int Index { get; set; }
        public string Name { get; set; }

        public bool IsDrum { get; set; }
        public int StringNumber { get; set; }

        public IList<Measure> Measures { get; set; }
        public Track()
        {
            Measures = new List<Measure>();
            //StringNumber = 6;
        }
    }
}
