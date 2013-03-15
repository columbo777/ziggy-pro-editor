using System.Collections.Generic;

namespace PhoneGuitarTab.Tablature
{
    public class TabFile
    {
        public Header Header { get; set; }
        public IList<Track> Tracks { get; set; }
        public TabFile()
        {
            Tracks = new List<Track>();
        }
    }
}
