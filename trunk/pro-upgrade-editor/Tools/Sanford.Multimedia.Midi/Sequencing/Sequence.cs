
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
        Unknown,
    }

    public sealed class Sequence : IComponent, ICollection<Track>
    {
        // Sequence Members

        // Fields

        // The collection of Tracks for the Sequence.
        private List<Track> tracks = new List<Track>();

        // The Sequence's MIDI file properties.
        private MidiFileProperties properties = new MidiFileProperties();

        private BackgroundWorker loadWorker = new BackgroundWorker();

        private BackgroundWorker saveWorker = new BackgroundWorker();

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

        public void Load(MemoryStream ms)
        {
            ms.Seek(0, SeekOrigin.Begin);

            using (ms)
            {
                var newProperties = new MidiFileProperties();
                var reader = new TrackReader();
                var newTracks = new List<Track>();

                newProperties.Read(ms);

                for (int i = 0; i < newProperties.TrackCount; i++)
                {
                    var events = reader.Read(ms);
                    reader.Track.Sequence = this;
                    reader.Track.FileType = this.FileType;
                    newTracks.Add(reader.Track);
                }

                properties = newProperties;
                tracks = newTracks;
            }
        }

        // Events

        public event EventHandler<AsyncCompletedEventArgs> LoadCompleted;

        public event ProgressChangedEventHandler LoadProgressChanged;

        public event EventHandler<AsyncCompletedEventArgs> SaveCompleted;

        public event ProgressChangedEventHandler SaveProgressChanged;

        

        // Construction

        /// <summary>
        /// Initializes a new instance of the Sequence class.
        /// </summary>
        public Sequence(FileType type, int division = int.MinValue)
        {
            this.dirty = true;
            this.FileType = type;
            if (division != int.MinValue)
            {
                properties.Division = division;
                properties.Format = 1;
            }
            InitializeBackgroundWorkers();
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
            InitializeBackgroundWorkers();

            Load(fileName);
        }

        

        private void InitializeBackgroundWorkers()
        {
            loadWorker.DoWork += new DoWorkEventHandler(LoadDoWork);
            loadWorker.ProgressChanged += new ProgressChangedEventHandler(OnLoadProgressChanged);
            loadWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnLoadCompleted);
            loadWorker.WorkerReportsProgress = true;

            saveWorker.DoWork += new DoWorkEventHandler(SaveDoWork);
            saveWorker.ProgressChanged += new ProgressChangedEventHandler(OnSaveProgressChanged);
            saveWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnSaveCompleted);
            saveWorker.WorkerReportsProgress = true;
        }

        public Track[] Tracks { get { return tracks.ToArray(); } }
        
        public void Load(string fileName)
        {
            var stream = new FileStream(fileName, FileMode.Open,
                FileAccess.Read, FileShare.Read);

            using(stream)
            {
                var newProperties = new MidiFileProperties();
                var reader = new TrackReader();
                var newTracks = new List<Track>();

                newProperties.Read(stream);

                for(int i = 0; i < newProperties.TrackCount; i++)
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

        public void LoadAsync(string fileName)
        {
            loadWorker.RunWorkerAsync(fileName);
        }

        public void LoadAsyncCancel()
        {
            loadWorker.CancelAsync();
        }

        public Track GetTrack(string name)
        {
            return tracks.FirstOrDefault(x => string.Compare(x.Name, name) == 0);
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

        /// <summary>
        /// Saves the Sequence as a MIDI file.
        /// </summary>
        /// <param name="fileName">
        /// The name to use for saving the MIDI file.
        /// </param>
        public void Save(string fileName)
        {
            var stream = new FileStream(fileName, FileMode.Create,
                FileAccess.Write, FileShare.None);

            using(stream)
            {
                properties.Write(stream);

                var writer = new TrackWriter();

                foreach(Track trk in tracks)
                {
                    writer.Track = trk;
                    writer.Write(stream);
                }
            }
        }

        public void SaveAsync(string fileName)
        {
            saveWorker.RunWorkerAsync(fileName);
        }

        public void SaveAsyncCancel()
        {
          
            saveWorker.CancelAsync();
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

            foreach(Track t in this)
            {
                if(t.Length > length)
                {
                    length = t.Length;
                }
            }

            return length;
        }

        private void OnLoadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EventHandler<AsyncCompletedEventArgs> handler = LoadCompleted;

            if(handler != null)
            {
                handler(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, null));
            }
        }

        private void OnLoadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChangedEventHandler handler = LoadProgressChanged;

            if(handler != null)
            {
                handler(this, e);
            }
        }

        private void LoadDoWork(object sender, DoWorkEventArgs e)
        {
            string fileName = (string)e.Argument;

            FileStream stream = new FileStream(fileName, FileMode.Open,
                FileAccess.Read, FileShare.Read);

            using(stream)
            {
                MidiFileProperties newProperties = new MidiFileProperties();
                TrackReader reader = new TrackReader();
                List<Track> newTracks = new List<Track>();

                newProperties.Read(stream);

                float percentage;

                for(int i = 0; i < newProperties.TrackCount && !loadWorker.CancellationPending; i++)
                {
                    reader.Read(stream);
                    reader.Track.Sequence = this;
                    reader.Track.FileType = this.FileType;
                    newTracks.Add(reader.Track);

                    percentage = (i + 1f) / newProperties.TrackCount;

                    loadWorker.ReportProgress((int)(100 * percentage));
                }

                if(loadWorker.CancellationPending)
                {
                    e.Cancel = true;
                }
                else
                {
                    properties = newProperties;
                    tracks = newTracks;
                }
            }            
        }

        private void OnSaveCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EventHandler<AsyncCompletedEventArgs> handler = SaveCompleted;

            if(handler != null)
            {
                handler(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, null));
            }
        }

        private void OnSaveProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChangedEventHandler handler = SaveProgressChanged;

            if(handler != null)
            {
                handler(this, e);
            }
        }

        private void SaveDoWork(object sender, DoWorkEventArgs e)
        {
            string fileName = (string)e.Argument;

            FileStream stream = new FileStream(fileName, FileMode.Create,
                FileAccess.Write, FileShare.None);

            using(stream)
            {
                properties.Write(stream);

                TrackWriter writer = new TrackWriter();

                float percentage;

                for(int i = 0; i < tracks.Count && !saveWorker.CancellationPending; i++)
                {
                    writer.Track = tracks[i];
                    writer.Write(stream);

                    percentage = (i + 1f) / properties.TrackCount;

                    saveWorker.ReportProgress((int)(100 * percentage));
                }

                if(saveWorker.CancellationPending)
                {
                    e.Cancel = true;
                }
            }
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

        public bool IsBusy
        {
            get
            {
                return loadWorker.IsBusy || saveWorker.IsBusy;
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
            if (tracks.Contains(item))
            {
                ret = tracks.Remove(item);

                if (ret)
                {
                    properties.TrackCount = tracks.Count;
                }
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
            loadWorker.Dispose();
            saveWorker.Dispose();

            EventHandler handler = Disposed;
            if(handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        
    }
}
