using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

namespace ProUpgradeEditor.Common
{
    [Flags()]
    public enum GuitarDifficulty
    {
        None = 0,
        All = 1 << 0,
        Easy = 1 << 1,
        Medium = 1 << 2,
        Hard = 1 << 3,
        Expert = 1 << 4,
        EasyMediumHardExpert = Easy | Medium | Hard | Expert,
        EasyMediumHard = Easy | Medium | Hard,
        EasyMedium = Easy | Medium,
        HardExpert = Hard | Expert,
        Unknown = 1 << 5
    }

    public class GenDiffConfig
    {
        public bool EnableProGuitarHard { get; set; }
        public bool EnableProGuitarMedium { get; set; }
        public bool EnableProGuitarEasy { get; set; }
        public bool EnableProBassHard { get; set; }
        public bool EnableProBassMedium { get; set; }
        public bool EnableProBassEasy { get; set; }

        public bool CopyGuitarToBass { get; set; }
        public bool CopyTextEvents { get; set; }

        public bool ProcessingSong { get; set; }
        public bool SelectedDifficultyOnly { get; set; }
        public bool SelectedTrackOnly { get; set; }

        public bool Generate108Events { get; set; }


        public GenDiffConfig(SongCacheItem item,
            bool copyTextEvents, bool copyGuitarToBass,
            bool selectedDiffOnly, bool selectedTrackOnly,
            bool generate108Events)
        {
            Generate108Events = generate108Events;
            CopyTextEvents = copyTextEvents;

            if (item != null)
            {
                ProcessingSong = true;
                EnableProGuitarHard = item.AutoGenGuitarHard;
                EnableProGuitarMedium = item.AutoGenGuitarMedium;
                EnableProGuitarEasy = item.AutoGenGuitarEasy;

                EnableProBassHard = item.AutoGenBassHard;
                EnableProBassMedium = item.AutoGenBassMedium;
                EnableProBassEasy = item.AutoGenBassEasy;

                CopyGuitarToBass = item.CopyGuitarToBass;
            }
            else
            {
                ProcessingSong = false;
                CopyGuitarToBass = copyGuitarToBass;
            }

            SelectedDifficultyOnly = selectedDiffOnly;
            SelectedTrackOnly = selectedTrackOnly;
        }
    }
}
