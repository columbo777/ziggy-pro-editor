using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;

using System.Runtime.CompilerServices;
using System.Globalization;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public enum CommandSortEnum
    {
        OnFirst,
        OffFirst,
    }

    [Flags()]
    public enum CompareMidiEventEnum
    {
        Ticks = (1 << 0),
        Data1 = (1 << 1),
        Channel = (1 << 2),
        Command = (1 << 3),
        All = (Ticks | Data1 | Channel | Command),
    }
    public class MidiEventChannelMessageSorter : IComparer<MidiEvent>
    {
        CommandSortEnum commandOnOffSort;
        CompareMidiEventEnum comparers;

        public MidiEventChannelMessageSorter(CompareMidiEventEnum comparers, CommandSortEnum commandOnOffSort)
        {
            this.commandOnOffSort = commandOnOffSort;
            this.comparers = comparers;
        }

        public int Compare(MidiEvent x, MidiEvent y)
        {
            if (x.AbsoluteTicks == y.AbsoluteTicks &&
                x.Data1 == y.Data1 &&
                x.Channel == y.Channel &&
                x.Command != y.Command)
            {
                if (x.IsOff)
                    return -1;
                else
                    return 1;
            }
            else
            {
                if (comparers.HasFlag(CompareMidiEventEnum.Ticks))
                {
                    if (x.AbsoluteTicks < y.AbsoluteTicks)
                        return -1;
                    else if (x.AbsoluteTicks > y.AbsoluteTicks)
                        return 1;
                }
                if (comparers.HasFlag(CompareMidiEventEnum.Data1))
                {
                    if (x.Data1 < y.Data1)
                        return -1;
                    else if (x.Data1 > y.Data1)
                        return 1;
                }
                if (comparers.HasFlag(CompareMidiEventEnum.Channel))
                {
                    if (x.Channel < y.Channel)
                        return -1;
                    else if (x.Channel > y.Channel)
                        return 1;
                }

                if (comparers.HasFlag(CompareMidiEventEnum.Command))
                {
                    if (x.Command != y.Command)
                    {
                        if (x.Command == (commandOnOffSort == CommandSortEnum.OffFirst ? ChannelCommand.NoteOff : ChannelCommand.NoteOn))
                            return -1;
                        else
                            return 1;
                    }
                }

                return 0;
            }
        }
    }
}