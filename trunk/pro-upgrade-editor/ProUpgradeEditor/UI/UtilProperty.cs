using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;
using System.Threading;
using System.Globalization;
using XPackage;
using System.Diagnostics;


namespace ProUpgradeEditor.UI
{
    public class UtilProperty
    {

        public string Description;
        public object Data;
        public object OriginalData;
        public PropertyType Type;
        public Func<UtilProperty, bool> OnUpdate;


        public UtilProperty(string desc, int data, Func<UtilProperty, bool> OnUpdate = null)
        {
            this.Description = desc;
            this.Data = OriginalData = data;
            this.Type = PropertyType.Integer;
            this.OnUpdate = OnUpdate;
        }
        public UtilProperty(string desc, bool data, Func<UtilProperty, bool> OnUpdate = null)
        {
            this.OnUpdate = OnUpdate;
            this.Description = desc;
            this.Data = OriginalData = data;
            this.Type = PropertyType.Bool;
        }
        public UtilProperty(string desc, string data, Func<UtilProperty, bool> OnUpdate = null)
        {
            this.OnUpdate = OnUpdate;
            this.Description = desc;
            this.Data = OriginalData = data;
            this.Type = PropertyType.String;
        }
        public UtilProperty(string desc, double data, Func<UtilProperty, bool> OnUpdate = null)
        {
            this.OnUpdate = OnUpdate;
            this.Description = desc;
            this.Data = OriginalData = data;
            this.Type = PropertyType.Double;
        }

        public UtilProperty(string desc, Pen data, Func<UtilProperty, bool> OnUpdate = null)
        {
            this.OnUpdate = OnUpdate;
            this.Description = desc;
            this.Data = OriginalData = data;
            this.Type = PropertyType.Pen;
        }
        public UtilProperty(string desc, SolidBrush data, Func<UtilProperty, bool> OnUpdate = null)
        {
            this.OnUpdate = OnUpdate;
            this.Description = desc;
            this.Data = OriginalData = data;
            this.Type = PropertyType.Brush;
        }

        public static UtilProperty[] GetInitialProperties()
        {
            return new UtilProperty[]
                {
                    new UtilProperty("Tick Close Distance", (int)20, (p)=>{
                        Utility.TickCloseWidth = ((int)p.Data);
                        return Utility.TickCloseWidth.IsNull() == false;
                    }),
                    new UtilProperty("Rock Band 3 Title ID", Utility.RockBand3TitleID.ToString(), (p)=>{
                        
                        uint titleID;
                        var ret = uint.TryParse((string)p.Data, out titleID);
                        if(ret)
                        {
                            Utility.RockBand3TitleID = titleID;
                        }
                        else{
                            p.Data = Utility.RockBand3TitleID.ToString();
                        }
                        return ret;
                    }),
                    //new UtilProperty("Enable Render MP3 Wave", false),
                    new UtilProperty( "Save plus.mid with con", false),
                    new UtilProperty( "Save pro backup on save", false),
                    new UtilProperty( "Clear Chord Names", Utility.ClearChordNames),
                    new UtilProperty( "108 Generation Enabled", Utility.HandPositionGenerationEnabled),
                    new UtilProperty( "108 Marker Start Offset", Utility.HandPositionMarkerStartOffset),
                    new UtilProperty( "108 Marker End Offset", Utility.HandPositionMarkerEndOffset),
                    new UtilProperty( "108 Marker Max Fret", Utility.HandPositionMarkerMaxFret),
                    new UtilProperty( "108 Marker Min Fret", Utility.HandPositionMarkerMinFret),
                    new UtilProperty( "108 Marker By Difficulty", Utility.HandPositionMarkerByDifficulty),
                    new UtilProperty( "108 Marker First Begin", Utility.HandPositionMarkerFirstBeginOffset),
                    new UtilProperty( "108 Marker First End", Utility.HandPositionMarkerFirstEndOffset),
                    
                    new UtilProperty( "Show Measure Numbers", true),
                    new UtilProperty( "Show Tempos", true),
                    new UtilProperty( "Show Time Signatures", true),
                    new UtilProperty( "View Lyrics in G5 Editor", true),
                    new UtilProperty( "Keep Midi Playback Selection", false),
                    new UtilProperty( "Keep Auto Gen Difficulty Selection", false),

                    new UtilProperty( "Max Backups", Utility.MaxBackups),
                    

                    new UtilProperty( "Dummy Tempo", Utility.DummyTempo),
                    new UtilProperty( "Default CON File Extension", Utility.DefaultCONFileExtension),
                    new UtilProperty( "Default PRO File Extension", Utility.DefaultPROFileExtension),

                    new UtilProperty( "Background Brush", Utility.BackgroundBrush),
                    
                    new UtilProperty( "G5 Green Note", Utility.G5GreenNoteBrush),
                    new UtilProperty( "G5 Red Note", Utility.G5RedNoteBrush),
                    new UtilProperty( "G5 Yellow Note", Utility.G5YellowNoteBrush),
                    new UtilProperty( "G5 Blue Note", Utility.G5BlueNoteBrush),
                    new UtilProperty( "G5 Orange Note", Utility.G5OragneNoteBrush),
                    new UtilProperty( "Beat Pen", Utility.beatPen),
                    new UtilProperty( "Bar Pen", Utility.barPen),
                    new UtilProperty( "Draw Beat", Utility.DrawBeat),
                    new UtilProperty("Rectangle Select Pen", Utility.rectSelectionPen),

                    new UtilProperty( "G5 Line Pen", Utility.linePen),
                    new UtilProperty( "Pro Line Pen", Utility.linePen22),
                    new UtilProperty( "Selected Pen", Utility.selectedPen),

                    new UtilProperty( "Slide", Utility.slidePen),
                    new UtilProperty( "Hammeron", Utility.hammerOnPen),
                    new UtilProperty( "Note Bounds", Utility.noteBoundPen),
                    new UtilProperty( "Grid Snap Pointer Color", Utility.gridSnapCursorBrush),
                    new UtilProperty( "Grid Snap Pointer Size", Utility.gridSnapCursorSize),

                    new UtilProperty( "Note Background", Utility.noteBGBrush),
                    new UtilProperty( "Selected Note Background", Utility.noteBGBrushSel),
                    new UtilProperty( "Note Background Shadow", Utility.noteBGBrushShadow),
                    new UtilProperty( "Single String Tremelo", Utility.noteSingleStringTremeloBrush),
                    new UtilProperty( "Multi String Tremelo", Utility.noteMultiStringTremeloBrush),

                    new UtilProperty( "Note Tap", Utility.noteTapBrush),
                    new UtilProperty( "Note X", Utility.noteXBrush),
                    new UtilProperty( "Note Arpeggio", Utility.noteArpeggioBrush),
                    new UtilProperty( "Note Powerup", Utility.notePowerupBrush),
                    new UtilProperty( "Note Solo", Utility.noteSoloBrush),
                    new UtilProperty( "Note Big Rock Ending", Utility.noteBREBrush),
                    new UtilProperty( "Note Strum", Utility.noteStrumBrush),
                    //new UtilProperty( "Note Height",Utility.NoteHeight),

                    new UtilProperty( "Note Text Color", Utility.fretBrush),

                    new UtilProperty( "X Note Text", Utility.XNoteText),
                    new UtilProperty( "X Note Text Y Offset", Utility.XNoteTextYOffset),
                    new UtilProperty( "X Note Text X Offset", Utility.XNoteTextXOffset),
                    new UtilProperty( "Note Text X Offset", Utility.NoteTextXOffset),
                    new UtilProperty( "Note Text Y Offset", Utility.NoteTextYOffset),
                    new UtilProperty( "Arpeggio Helper Prefix", Utility.ArpeggioHelperPrefix),
                    new UtilProperty( "Arpeggio Helper Suffix", Utility.ArpeggioHelperSuffix),
                    
                    new UtilProperty( "Stored Chord Prefix", Utility.StoredChordPrefix),
                    new UtilProperty( "Stored Chord Note Separator", Utility.StoredChordNoteSeparator),
                    new UtilProperty( "Stored Chord Empty Note", Utility.StoredChordEmptyNote),
                    new UtilProperty( "Stored Chord Suffix", Utility.StoredChordSuffix),
                    new UtilProperty( "Stored Chord Slide", Utility.StoredChordSlide),
                    new UtilProperty( "Stored Chord Reverse", Utility.StoredChordReverse),
                    new UtilProperty( "Stored Chord Hammeron", Utility.StoredChordHammeron),
                    new UtilProperty( "Stored Chord Tap", Utility.StoredChordTap),
                    new UtilProperty( "Stored Chord X Note", Utility.StoredChordXNote),
                    new UtilProperty( "Stored Chord Strum Low", Utility.StoredChordStrumLow),
                    new UtilProperty( "Stored Chord Strum Med", Utility.StoredChordStrumMed),
                    new UtilProperty( "Stored Chord Strum High", Utility.StoredChordStrumHigh),
                    
                    new UtilProperty( "Trainer Color", Utility.TrainerBrush),
                    new UtilProperty( "Sel. Trainer Color", Utility.SelectedTrainerBrush),
                    new UtilProperty( "Text Event Color", Utility.TextEventBrush),
                    new UtilProperty( "Sel. Text Event Color", Utility.SelectedTextEventBrush),

                    new UtilProperty( "Text Event Begin", Utility.TextEventBeginTag),
                    new UtilProperty( "Text Event End", Utility.TextEventEndTag),
                    new UtilProperty( "ProG Trainer Begin", Utility.SongTrainerBeginPGText),
                    new UtilProperty( "ProB Trainer Begin", Utility.SongTrainerBeginPBText),
                    new UtilProperty( "ProG Trainer Norm", Utility.SongTrainerNormPGText),
                    new UtilProperty( "ProB Trainer Norm", Utility.SongTrainerNormPBText),
                    new UtilProperty( "ProG Trainer End", Utility.SongTrainerEndPGText),
                    new UtilProperty( "ProB Trainer End", Utility.SongTrainerEndPBText),

                    new UtilProperty( "ProG Trainer", Utility.SongTrainerPGText),
                    new UtilProperty( "ProB Trainer", Utility.SongTrainerPBText),
                   
                    new UtilProperty("Trainer Norm Offset", Utility.SongTrainerNormOffset),
                    new UtilProperty("Modify WebTab Scale", Utility.ModifyWebTabScale),
                    
                };
        }
    }
}