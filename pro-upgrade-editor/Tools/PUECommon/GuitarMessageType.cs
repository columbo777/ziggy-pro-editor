using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

using System.Collections;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public enum GuitarMessageType
    {
        Unknown,
        GuitarHandPosition,
        GuitarChordName,
        GuitarTextEvent,
        GuitarTrainer,
        GuitarChord,
        GuitarChordStrum,
        GuitarNote,
        GuitarPowerup,
        GuitarSolo,
        GuitarTempo,
        GuitarTimeSignature,
        GuitarArpeggio,
        GuitarBigRockEnding,
        GuitarBigRockEndingSubMessage,
        GuitarSingleStringTremelo,
        GuitarMultiStringTremelo,
        GuitarSlide,
        GuitarHammeron
    }
}