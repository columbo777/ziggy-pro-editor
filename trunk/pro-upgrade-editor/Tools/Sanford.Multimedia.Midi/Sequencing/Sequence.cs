
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Sanford.Multimedia.Midi
{
    /// <summary>
    /// Represents a collection of Tracks.
    /// </summary>

    public enum FileType
    {
        Pro,
        Guitar5,
        DTA,
        CON,
        Unknown,
    }

    public sealed class Sequence : IComponent, ICollection<Track>, IDisposable
    {
        // Sequence Members

        // Fields

        // The collection of Tracks for the Sequence.
        private List<Track> tracks = new List<Track>();

        // The Sequence's MIDI file properties.
        private MidiFileProperties properties = new MidiFileProperties();

        
        private ISite site = null;

        FileType fileType;
        public FileType FileType
        {
            get { return fileType; }
            set
            {
                fileType = value;
                Tracks.ToList().ForEach(x => x.FileType = fileType);
            }
        }





        public static Sequence FromStream(MemoryStream ms)
        {
            Sequence ret = null;
            try
            {
                var seq = new Sequence(FileType.Unknown, 480);
                seq.Load(ms);
                ret = seq;

                if (ret.Any(x => x.Name != null && x.Name.EndsWith("_22")))
                {
                    ret.FileType = FileType.Pro;
                }
                else if (ret.Any(x => x.Name != null && (x.Name.ToLower()=="part guitar" || x.Name.ToLower()=="part bass")))
                {
                    ret.FileType = FileType.Guitar5;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return ret;
        }
        public void Load(MemoryStream ms)
        {
            ms.Seek(0, SeekOrigin.Begin);

            var newProperties = new MidiFileProperties();
            var reader = new TrackReader();
            var newTracks = new List<Track>();

            newProperties.Read(ms);

            for (int i = 0; i < newProperties.TrackCount; i++)
            {
                reader.Read(ms);
                reader.Track.Sequence = this;
                reader.Track.FileType = this.FileType;
                newTracks.Add(reader.Track);
            }

            properties = newProperties;
            tracks = newTracks;
        }

        // Events



        // Construction

        /// <summary>
        /// Initializes a new instance of the Sequence class.
        /// </summary>
        public Sequence(FileType type, int division)
        {
            this.dirty = true;
            this.FileType = type;
            
            properties.Division = division == Int32.MinValue ? 480 : division;
            properties.Format = 1;
            
        }

        /// <summary>
        /// Initializes a new instance of the Sequence class with the specified
        /// file name of the MIDI file to load.
        /// </summary>
        /// <param name="fileName">
        /// The name of the MIDI file to load.
        /// </param>
        public Sequence(FileType type, string fileName)
        {
            this.dirty = true;
            this.FileType = type;

            Load(fileName);
        }


        public Track[] Tracks { get { return tracks.ToArray(); } }

        public void Load(string fileName)
        {
            var stream = new FileStream(fileName, FileMode.Open,
                FileAccess.Read, FileShare.Read);

            using (stream)
            {
                var newProperties = new MidiFileProperties();
                var reader = new TrackReader();
                var newTracks = new List<Track>();

                newProperties.Read(stream);

                for (int i = 0; i < newProperties.TrackCount; i++)
                {
                    reader.Read(stream);
                    reader.Track.Sequence = this;
                    reader.Track.FileType = this.FileType;
                    newTracks.Add(reader.Track);
                }

                properties = newProperties;
                tracks = newTracks;
            }

        }

        public Track GetTrack(string name)
        {
            return tracks.FirstOrDefault(x => string.Compare(x.Name, name, true) == 0);
        }

        bool dirty;

        public bool Dirty
        {
            get
            {
                return dirty | tracks.Any(x => x.Dirty == true);
            }
            set
            {
                dirty = value;

                foreach (var t in tracks)
                {
                    t.Dirty = value;
                }
            }
        }

        

        public MemoryStream Save()
        {
            var ms = new MemoryStream();
            properties.Write(ms);

            var writer = new TrackWriter();
            foreach (Track trk in tracks)
            {
                writer.Track = trk;
                writer.Write(ms);
            }
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
        /// <summary>
        /// Gets the length in ticks of the Sequence.
        /// </summary>
        /// <returns>
        /// The length in ticks of the Sequence.
        /// </returns>
        /// <remarks>
        /// The length in ticks of the Sequence is represented by the Track 
        /// with the longest length.
        /// </remarks>
        public int GetLength()
        {
            int length = 0;

            foreach (Track t in this)
            {
                if (t.Length > length)
                {
                    length = t.Length;
                }
            }

            return length;
        }




        // Properties

        /// <summary>
        /// Gets the Track at the specified index.
        /// </summary>
        /// <param name="index">
        /// The index of the Track to get.
        /// </param>
        /// <returns>
        /// The Track at the specified index.
        /// </returns>
        public Track this[int index]
        {
            get
            {


                return tracks[index];
            }
        }

        /// <summary>
        /// Gets the Sequence's division value.
        /// </summary>
        public int Division
        {
            get
            {

                return properties.Division;
            }
        }

        /// <summary>
        /// Gets or sets the Sequence's format value.
        /// </summary>
        public int Format
        {
            get
            {
                return properties.Format;
            }
            set
            {
                properties.Format = value;
            }
        }

        /// <summary>
        /// Gets the Sequence's type.
        /// </summary>
        public SequenceType SequenceType
        {
            get
            {
                return properties.SequenceType;
            }
        }

        public void AddTempo(Track item)
        {
            item.Sequence = this;
            tracks.Insert(0, item);
            properties.TrackCount = tracks.Count;
        }

        public void Add(Track item)
        {
            item.Sequence = this;
            item.FileType = this.FileType;
            tracks.Add(item);
            properties.TrackCount = tracks.Count;
        }

        public void Insert(int index, Track item)
        {
            item.Sequence = this;
            item.FileType = this.FileType;
            if (index > tracks.Count)
            {
                index = tracks.Count;
            }
            tracks.Insert(index, item);
            properties.TrackCount = tracks.Count;
        }

        public void MoveTrack(int startIndex, int destIndex)
        {
            var track = tracks[startIndex];

            tracks.Remove(track);

            if (destIndex >= tracks.Count)
            {
                tracks.Add(track);
            }
            else
            {
                tracks.Insert(destIndex, track);
            }
        }

        public void Clear()
        {
            tracks.ToList().ForEach(x => x.Clear());
            tracks.Clear();

            properties.TrackCount = tracks.Count;
        }

        public bool Contains(Track item)
        {
            return tracks.Contains(item);
        }

        public void CopyTo(Track[] array, int arrayIndex)
        {
            tracks.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return tracks.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(Track item)
        {
            bool ret = false;
            if (tracks.Contains(item) && tracks.Remove(item))
            {
                properties.TrackCount = tracks.Count;
                ret = true;
            }
            return ret;
        }



        // IEnumerable<Track> Members

        public IEnumerator<Track> GetEnumerator()
        {
            return tracks.GetEnumerator();
        }



        // IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return tracks.GetEnumerator();
        }



        // IComponent Members

        public event EventHandler Disposed;

        public ISite Site
        {
            get
            {
                return site;
            }
            set
            {
                site = value;
            }
        }



        // IDisposable Members

        public void Dispose()
        {
            Clear();

            EventHandler handler = Disposed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

    }
}
