using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProUpgradeEditor
{

    public class SongCacheItem : DeletableEntity, IComparable<SongCacheItem>
    {

        public SongCacheItem()
            : base()
        {
            HasBass = true;
            HasGuitar = true;
            IsComplete = false;
            IsFinalized = false;
            CopyGuitarToBass = false;
            G5FileName = string.Empty;
            G6FileName = string.Empty;
            SongName = string.Empty;
            G6ConFile = string.Empty;
            Description = string.Empty;
            GuitarTuning = new string[] { "0", "0", "0", "0", "0", "0" };
            BassTuning = new string[] { "0", "0", "0", "0", "0", "0" };

            DTAGuitarDifficulty = 0;
            DTABassDifficulty = 0;
            DTASongID = string.Empty;
            DTASongShortName = string.Empty;
            SongMP3Location = string.Empty;
            SongMP3PlaybackOffset = Int32.MinValue;
            EnableSongMP3Playback = false;
            EnableSongMidiPlayback = true;
            SongMidiPlaybackVolume = 100;
            SongMP3PlaybackVolume = 100;

            CacheSongID = -1;
            IsSelected = false;
            AutoGenBassEasy = true;
            AutoGenBassHard = true;
            AutoGenBassMedium = true;
            AutoGenGuitarEasy = true;
            AutoGenGuitarHard = true;
            AutoGenGuitarMedium = true;

            IsDirty = false;
        }

        string g5FileName;
        public string G5FileName { get { return g5FileName; } set { if (g5FileName != value) { g5FileName = value; this.IsUpdated = true; } } }

        string g6FileName;
        public string G6FileName { get { return g6FileName; } set { if (g6FileName != value) { g6FileName = value; this.IsUpdated = true; } } }

        string songName;
        public string SongName { get { return songName; } set { if (songName != value) { songName = value; this.IsUpdated = true; } } }

        string g6ConFile;
        public string G6ConFile { get { return g6ConFile; } set { if (g6ConFile != value) { g6ConFile = value; this.IsUpdated = true; } } }

        string description;
        public string Description { get { return description; } set { if (description != value) { description = value; this.IsUpdated = true; } } }

        bool hasBass;
        public bool HasBass { get { return hasBass; } set { if (hasBass != value) { hasBass = value; this.IsUpdated = true; } } }

        bool hasGuitar;
        public bool HasGuitar { get { return hasGuitar; } set { if (hasGuitar != value) { hasGuitar = value; this.IsUpdated = true; } } }

        bool isComplete;
        public bool IsComplete { get { return isComplete; } set { if (isComplete != value) { isComplete = value; this.IsUpdated = true; } } }

        bool isFinalized;
        public bool IsFinalized { get { return isFinalized; } set { if (isFinalized != value) { isFinalized = value; this.IsUpdated = true; } } }

        bool copyGuitarToBass;
        public bool CopyGuitarToBass { get { return copyGuitarToBass; } set { if (copyGuitarToBass != value) { copyGuitarToBass = value; this.IsUpdated = true; } } }

        string[] guitarTuning;
        public string[] GuitarTuning { get { return guitarTuning; } set { if (guitarTuning != value) { guitarTuning = value; this.IsUpdated = true; } } }

        string[] bassTuning;
        public string[] BassTuning { get { return bassTuning; } set { if (bassTuning != value) { bassTuning = value; this.IsUpdated = true; } } }

        int dTAGuitarDifficulty;
        public int DTAGuitarDifficulty { get { return dTAGuitarDifficulty; } set { if (dTAGuitarDifficulty != value) { dTAGuitarDifficulty = value; this.IsUpdated = true; } } }

        int dTABassDifficulty;
        public int DTABassDifficulty { get { return dTABassDifficulty; } set { if (dTABassDifficulty != value) { dTABassDifficulty = value; this.IsUpdated = true; } } }

        string dTASongID;
        public string DTASongID { get { return dTASongID; } set { if (dTASongID != value) { dTASongID = value; this.IsUpdated = true; } } }

        string dTASongShortName;
        public string DTASongShortName { get { return dTASongShortName; } set { if (dTASongShortName != value) { dTASongShortName = value; this.IsUpdated = true; } } }

        string songMP3Location;
        public string SongMP3Location { get { return songMP3Location; } set { if (songMP3Location != value) { songMP3Location = value; this.IsUpdated = true; } } }

        int songMP3PlaybackOffset;
        public int SongMP3PlaybackOffset { get { return songMP3PlaybackOffset; } set { if (songMP3PlaybackOffset != value) { songMP3PlaybackOffset = value; this.IsUpdated = true; } } }

        bool enableSongMP3Playback;
        public bool EnableSongMP3Playback { get { return enableSongMP3Playback; } set { if (enableSongMP3Playback != value) { enableSongMP3Playback = value; this.IsUpdated = true; } } }

        bool enableSongMidiPlayback;
        public bool EnableSongMidiPlayback { get { return enableSongMidiPlayback; } set { if (enableSongMidiPlayback != value) { enableSongMidiPlayback = value; this.IsUpdated = true; } } }

        int songMidiPlaybackVolume;
        public int SongMidiPlaybackVolume { get { return songMidiPlaybackVolume; } set { if (songMidiPlaybackVolume != value) { songMidiPlaybackVolume = value; this.IsUpdated = true; } } }

        int songMP3PlaybackVolume;
        public int SongMP3PlaybackVolume { get { return songMP3PlaybackVolume; } set { if (songMP3PlaybackVolume != value) { songMP3PlaybackVolume = value; this.IsUpdated = true; } } }

        int cacheSongID;
        public int CacheSongID { get { return cacheSongID; } set { if (cacheSongID != value) { cacheSongID = value; this.IsUpdated = true; } } }

        bool isSelected;
        public bool IsSelected { get { return isSelected; } set { if (isSelected != value) { isSelected = value; this.IsUpdated = true; } } }

        bool autoGenGuitarHard;
        public bool AutoGenGuitarHard { get { return autoGenGuitarHard; } set { if (autoGenGuitarHard != value) { autoGenGuitarHard = value; this.IsUpdated = true; } } }

        bool autoGenGuitarMedium;
        public bool AutoGenGuitarMedium { get { return autoGenGuitarMedium; } set { if (autoGenGuitarMedium != value) { autoGenGuitarMedium = value; this.IsUpdated = true; } } }

        bool autoGenGuitarEasy;
        public bool AutoGenGuitarEasy { get { return autoGenGuitarEasy; } set { if (autoGenGuitarEasy != value) { autoGenGuitarEasy = value; this.IsUpdated = true; } } }

        bool autoGenBassHard;
        public bool AutoGenBassHard { get { return autoGenBassHard; } set { if (autoGenBassHard != value) { autoGenBassHard = value; this.IsUpdated = true; } } }

        bool autoGenBassMedium;
        public bool AutoGenBassMedium { get { return autoGenBassMedium; } set { if (autoGenBassMedium != value) { autoGenBassMedium = value; this.IsUpdated = true; } } }

        bool autoGenBassEasy;
        public bool AutoGenBassEasy { get { return autoGenBassEasy; } set { if (autoGenBassEasy != value) { autoGenBassEasy = value; this.IsUpdated = true; } } }

        public override string ToString()
        {
            return (Description.Length > 0 ? Description : SongName) + " [" + CacheSongID.ToString() + "]";
        }

        public int CompareTo(SongCacheItem other)
        {
            return string.Compare(ToString(), other.ToString());
        }

    }
}
