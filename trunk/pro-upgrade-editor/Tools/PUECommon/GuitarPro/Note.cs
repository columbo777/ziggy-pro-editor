
namespace PhoneGuitarTab.Tablature
{
    /// <summary>
    /// represents a note
    /// </summary>
    public class Note
    {
        //TODO header

        public bool IsLegato { get; set; }
        //public bool IsDotted { get; set; }

        public Bend Bend { get; set; }
        public Slide Slide { get; set; }
        private string _fret;
        public string Fret
        {
            get { return _fret; }
            set
            {
                _fret = value;

            }
        }
    }
}
