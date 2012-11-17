using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProUpgradeEditor.Common;

namespace ProUpgradeEditor.DataLayer
{
    public class GuitarMessageList : List<GuitarMessage>
    {
        public GuitarMessageList()
        {
        }

        [Flags()]
        enum GMDirtyState
        {
            
            Chords = (1 << 1),
            Powerup = (1 << 2),
            Solo = (1 << 3),
            Tempo = (1 << 4),
            Timesig = (1 << 5),
            Arpeggio = (1 << 6),
            BigRock = (1 << 7),
            SingleNote = (1 << 8),
            MultiNote = (1 << 9),
            TextEvent = (1 << 10),
            Trainer = (1<<11),
            HandPosition = (1<<12),
            All = (Chords|Powerup|Solo|Tempo|Timesig|Arpeggio|BigRock|SingleNote|MultiNote|TextEvent|Trainer|HandPosition),
        }
        
        GMDirtyState dirtyState = GMDirtyState.All;

        void FlagDirty(GMessage mess)
        {
            if(mess is GuitarHandPosition)
            {
                dirtyState |= GMDirtyState.HandPosition;
            }
            if (mess is GuitarTextEvent)
            {
                dirtyState |= GMDirtyState.TextEvent;
            }
            if (mess is GuitarTrainer)
            {
                dirtyState |= GMDirtyState.Trainer;
            }
            if(mess is GuitarChord)
            {
                dirtyState |= GMDirtyState.Chords;
            }
            if(mess is GuitarPowerup)
            {
                dirtyState |= GMDirtyState.Powerup;
            }
            if(mess is GuitarSolo)
            {
                dirtyState |= GMDirtyState.Solo;
            }
            if(mess is GuitarTempo)
            {
                dirtyState |= GMDirtyState.Tempo;
            }
            if(mess is GuitarTimeSignature)
            {
                dirtyState |= GMDirtyState.Timesig;
            }
            if(mess is GuitarArpeggio)
            {
                dirtyState |= GMDirtyState.Arpeggio;
            }
            if(mess is GuitarBigRockEnding)
            {
                dirtyState |= GMDirtyState.BigRock;
            }
            if(mess is GuitarSingleStringTremelo)
            {
                dirtyState |= GMDirtyState.SingleNote;
            }
            if(mess is GuitarMultiStringTremelo)
            {
                dirtyState |= GMDirtyState.MultiNote;
            }
        }

        public new void AddRange(IEnumerable<GuitarMessage> mess)
        {
            if (mess != null && mess.Any())
            {
                foreach (var msg in mess)
                {
                    base.Add(msg);
                    FlagDirty(msg);
                }
            }
        }

        public new void Add(GuitarMessage mess)
        {
            base.Add(mess);
            FlagDirty(mess);
        }

        public new void Remove(GuitarMessage mess)
        {
            base.Remove(mess);
            FlagDirty(mess);
        }

        public new void RemoveAt(int idx)
        {
            Remove(base[idx]);
        }

        public new void Insert(int idx, GuitarMessage mess)
        {
            base.Insert(idx, mess);
            FlagDirty(mess);
        }


        GuitarChord[] chords = new GuitarChord[0];
        public GuitarChord[] Chords
        {
            get
            {
                if(dirtyState.HasFlag(GMDirtyState.Chords))
                {
                    chords = this.Where(x => x is GuitarChord).Cast<GuitarChord>().SortTicks().ToArray();
                    dirtyState ^= GMDirtyState.Chords;
                }
                return chords;
            }
        }

        GuitarPowerup[] powerups = new GuitarPowerup[0];
        public GuitarPowerup[] Powerups
        {
            get
            {
                if (dirtyState.HasFlag(GMDirtyState.Powerup))
                {
                    powerups = this.Where(x => x is GuitarPowerup).SortTicks().Cast<GuitarPowerup>().ToArray();
                    dirtyState ^= GMDirtyState.Powerup;
                }
                return powerups;
            }
        }

        GuitarSolo[] solos = new GuitarSolo[0];
        public GuitarSolo[] Solos
        {
            get
            {
                if ((dirtyState & GMDirtyState.Solo) == GMDirtyState.Solo)
                {
                    solos = this.Where(x => x is GuitarSolo).SortTicks().Cast<GuitarSolo>().ToArray();
                    dirtyState ^= GMDirtyState.Solo;
                }
                return solos;
            }
        }

        GuitarTempo[] tempos = new GuitarTempo[0];
        public GuitarTempo[] Tempos
        {
            get
            {
                if ((dirtyState & GMDirtyState.Tempo) == GMDirtyState.Tempo)
                {
                    tempos = this.Where(x => x is GuitarTempo).SortTicks().Cast<GuitarTempo>().ToArray();
                    dirtyState ^= GMDirtyState.Tempo;
                }
                return tempos;
            }
        }

        GuitarTimeSignature[] timeSignatures = new GuitarTimeSignature[0];
        public GuitarTimeSignature[] TimeSignatures
        {
            get
            {
                if ((dirtyState & GMDirtyState.Timesig) == GMDirtyState.Timesig)
                {
                    timeSignatures = this.Where(x => x is GuitarTimeSignature).SortTicks().Cast<GuitarTimeSignature>().ToArray();
                    dirtyState ^= GMDirtyState.Timesig;
                }
                return timeSignatures;
            }
        }

        GuitarArpeggio[] arpeggios = new GuitarArpeggio[0];
        public GuitarArpeggio[] Arpeggios
        {
            get
            {
                if ((dirtyState & GMDirtyState.Arpeggio) == GMDirtyState.Arpeggio)
                {
                    arpeggios = this.Where(x => x is GuitarArpeggio).SortTicks().Cast<GuitarArpeggio>().ToArray();
                    dirtyState ^= GMDirtyState.Arpeggio;
                }
                return arpeggios;
            }
        }

        GuitarBigRockEnding[] bigRockEndings = new GuitarBigRockEnding[0];
        public GuitarBigRockEnding[] BigRockEndings
        {
            get
            {
                if ((dirtyState & GMDirtyState.BigRock) == GMDirtyState.BigRock)
                {
                    bigRockEndings = this.Where(x => x is GuitarBigRockEnding).SortTicks().Cast<GuitarBigRockEnding>().ToArray();
                    dirtyState ^= GMDirtyState.BigRock;
                }
                return bigRockEndings;
            }
        }

        GuitarSingleStringTremelo[] singleStringTremelos = new GuitarSingleStringTremelo[0];
        public GuitarSingleStringTremelo[] SingleStringTremelos
        {
            get
            {
                if ((dirtyState & GMDirtyState.SingleNote) == GMDirtyState.SingleNote)
                {
                    singleStringTremelos = this.Where(x => x is GuitarSingleStringTremelo).SortTicks().Cast<GuitarSingleStringTremelo>().ToArray();
                    dirtyState ^= GMDirtyState.SingleNote;
                }
                return singleStringTremelos;
            }
        }

        GuitarMultiStringTremelo[] multiStringTremelos = new GuitarMultiStringTremelo[0];
        public GuitarMultiStringTremelo[] MultiStringTremelos
        {
            get
            {
                if (dirtyState.HasFlag(GMDirtyState.MultiNote))
                {
                    multiStringTremelos = this.Where(x => x is GuitarMultiStringTremelo).SortTicks().Cast<GuitarMultiStringTremelo>().ToArray();
                    dirtyState ^= GMDirtyState.MultiNote;
                }
                return multiStringTremelos;
            }
        }

        GuitarTextEvent[] textEvents = new GuitarTextEvent[0];
        public GuitarTextEvent[] TextEvents
        {
            get
            {
                if (dirtyState.HasFlag(GMDirtyState.TextEvent))
                {
                    textEvents = this.Where(x => x is GuitarTextEvent).SortTicks().Cast<GuitarTextEvent>().ToArray();
                    dirtyState ^= GMDirtyState.TextEvent;
                }
                return textEvents;
            }
        }

        GuitarTrainer[] trainers = new GuitarTrainer[0];
        public GuitarTrainer[] Trainers
        {
            get
            {
                if (dirtyState.HasFlag(GMDirtyState.Trainer))
                {
                    trainers = this.Where(x => x is GuitarTrainer).SortTicks().Cast<GuitarTrainer>().ToArray();
                    dirtyState ^= GMDirtyState.Trainer;
                }
                return trainers;
            }
        }


        GuitarHandPosition[] handPositions = new GuitarHandPosition[0];
        public GuitarHandPosition[] HandPositions
        {
            get
            {
                if (dirtyState.HasFlag(GMDirtyState.HandPosition))
                {
                    handPositions = this.Where(x => x is GuitarHandPosition).SortTicks().Cast<GuitarHandPosition>().ToArray();
                    dirtyState ^= GMDirtyState.HandPosition;
                }
                return handPositions;
            }
        }

        public GuitarChord LastChord
        {
            get
            {
                return Chords.LastOrDefault() as GuitarChord;
            }
        }

        public GuitarChord FirstChord
        {
            get
            {
                return Chords.FirstOrDefault() as GuitarChord;
            }
        }

        public GuitarPowerup LastPowerup
        {
            get
            {
                return Powerups[Powerups.Length - 1] as GuitarPowerup;
            }
        }

        public GuitarArpeggio LastArpeggio
        {
            get
            {
                return Arpeggios[Arpeggios.Length - 1] as GuitarArpeggio;
            }
        }

        public GuitarBigRockEnding LastBigRockEnding
        {
            get
            {
                return BigRockEndings[BigRockEndings.Length - 1] as GuitarBigRockEnding;
            }
        }

        public GuitarSingleStringTremelo LastSingleStringTremelo
        {
            get
            {
                return SingleStringTremelos[SingleStringTremelos.Length - 1] as GuitarSingleStringTremelo;
            }
        }

        public GuitarMultiStringTremelo LastMultiStringTremelo
        {
            get
            {
                return MultiStringTremelos[MultiStringTremelos.Length - 1] as GuitarMultiStringTremelo;
            }
        }

        public GuitarSolo LastSolo
        {
            get
            {
                return Solos[Solos.Length - 1] as GuitarSolo;
            }
        }
    }
}
