using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using X360.IO;
using System.Diagnostics;
using System.IO;

namespace GPImporter
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethodDebug()
        {
            var data = GPImport.importDebug(File.ReadAllBytes("c:\\dtom.gp3"));
        }

        public class GPImport
        {


            public static GPData importDebug(byte[] data)
            {
                var gpdata = new DJsIO(data, true);
                int word;
                var ret = new GPData();
                ret.VersionDesc = ReadString(gpdata,false);
                var padding = 30 - ret.VersionDesc.Length;
                gpdata.ReadBytes(padding);

                ret.Title = ReadString(gpdata,true);
                ret.SubTitle = ReadString(gpdata, true);
                ret.Artist = ReadString(gpdata, true);
                ret.Album = ReadString(gpdata, true);

                if (ret.Version >= 500)
                {
                    ret.Lyricist = ReadString(gpdata, true);
                }

                ret.Composer = ReadString(gpdata, true);
                ret.Copyright = ReadString(gpdata, true);
                ret.Transcriber = ReadString(gpdata, true);
                ret.Instructions = ReadString(gpdata, true);
                ret.NoticeEntries = ReadStringList(gpdata);

                if (ret.Version < 500)
                {
                    ret.ShuffleRhythmFeel = ReadString(gpdata, true);
                }

                ret.Lyrics = new List<GPLyric>();
                if (ret.Version >= 400)
                {
                    int track = ReadDWORDLE(gpdata);
                    for (int x = 0; x < 5; x++)
                    {
                        int bar = ReadDWORDLE(gpdata);
                        var lyric = ReadString(gpdata, true);

                        ret.Lyrics.Add(new GPLyric() { Track = track, Bar = bar, Lyric = lyric });
                    }
                }

                if (ret.Version > 500)
                {
                    ret.MasterVolume = ReadDWORDLE(gpdata);
                    //gpdata.ReadBytes(4);
                    ret.band32hz = gpdata.ReadByte();
                    ret.band60hz = gpdata.ReadByte();
                    ret.band125hz = gpdata.ReadByte();
                    ret.band250hz = gpdata.ReadByte();
                    ret.band500hz = gpdata.ReadByte();
                    ret.band1khz = gpdata.ReadByte();
                    ret.band2khz = gpdata.ReadByte();
                    ret.band4khz = gpdata.ReadByte();
                    ret.band8khz = gpdata.ReadByte();
                    ret.band16khz = gpdata.ReadByte();
                    ret.bandGain = gpdata.ReadByte();

                }



                if (ret.Version >= 500)
                {
                    var formatLen = ReadDWORDLE(gpdata);
                    var formatWidth = ReadDWORDLE(gpdata);

                    var leftMargin = ReadDWORDLE(gpdata);
                    var rightMargin = ReadDWORDLE(gpdata);

                    var topMargin = ReadDWORDLE(gpdata);
                    var bottomMargin = ReadDWORDLE(gpdata);

                    var scoreSize = ReadDWORDLE(gpdata);

                    var enableHeaderFooter = ReadWORDLE(gpdata);

                    var titleHeaderFooter = ReadString(gpdata, true);
                    var subtitleHeaderFooter = ReadString(gpdata, true);
                    var artistHeaderFooter = ReadString(gpdata, true);
                    var albumHeaderFooter = ReadString(gpdata, true);
                    var lyricistHeaderFooter = ReadString(gpdata, true);
                    var composerHeaderFooter = ReadString(gpdata, true);
                    var wordsHeaderFooter = ReadString(gpdata, true);
                    var copyrightLine1HeaderFooter = ReadString(gpdata, true);
                    var copyrightLine2HeaderFooter = ReadString(gpdata, true);
                    var pageNumber = ReadString(gpdata, true);

                }

                if (ret.Version >= 500)
                {
                    var tempoDesc = ReadString(gpdata, true);
                }
                int tempoBPM = ReadDWORDLE(gpdata);


                if (ret.Version > 500)
                {
                    //There is a byte of unknown data/padding here in versions newer than 5.0 of the format
                    gpdata.ReadByte();
                }

                if (ret.Version >= 400)
                {
                    //Versions 4.0 and newer of the format store key and octave information here
                    var key = gpdata.ReadByte();
                    gpdata.ReadBytes(3);
                    var transpose = gpdata.ReadByte();

                }
                else
                {
                    var flatsSharps = ReadDWORDLE(gpdata);

                }


                for (int ctr = 0; ctr < 64; ctr++)
                {
                    var patch = ReadDWORDLE(gpdata);    //Read the instrument patch number
                    var volume = gpdata.ReadByte();                                 //Read the volume
                    var pan = gpdata.ReadByte();                                 //Read the pan value
                    var chorus = gpdata.ReadByte();                                 //Read the chorus value
                    var reverb = gpdata.ReadByte();                                 //Read the reverb value
                    var phaser = gpdata.ReadByte();                                 //Read the phaser value
                    var tremelo = gpdata.ReadByte();                                 //Read the tremolo value
                    var unk = ReadWORDLE(gpdata);             //Read two bytes of unknown data/padding
                }

                if (ret.Version >= 500)
                {       //Versions 5.0 and newer of the format store musical directional symbols and a master reverb setting here
                    //puts("\tMusical symbols:");
                    //eof_gp_debug_log(inf, "\"Coda\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Double coda\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Segno\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Segno segno\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Fine\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Da capo\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Da capo al coda\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Da capo al double coda\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Da capo al fine\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Da segno\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Da segno al coda\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Da segno al double coda\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Da segno al fine\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Da segno segno\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Da segno segno al coda\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Da segno segno al double coda\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Da segno segno al fine\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Da coda\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "\"Da double coda\" symbol is ");
                    word = ReadWORDLE(gpdata);
                    if (word == 0xFFFF)
                    {
                        //puts("unused");
                    }
                    else
                    {
                        //printf("at beat #%u\n", word);
                    }
                    //eof_gp_debug_log(inf, "Master reverb:  ");
                    word = ReadWORDLE(gpdata);  //Read the master reverb value
                    //printf("%ld\n", dword);
                }
                
                //eof_gp_debug_log(inf, "Number of measures:  ");
                var measures = ReadDWORDLE(gpdata);       //Read the number of measures
                //printf("%ld\n", measures);
                //eof_gp_debug_log(inf, "Number of tracks:  ");
                var tracks = ReadDWORDLE(gpdata); //pack_ReadDWORDLE(inf, &tracks); //Read the number of tracks
                //printf("%ld\n", tracks);

                //Allocate memory for an array to track the number of strings for each track

                var trackStrings = new List<int>();

                for (int ctr = 0; ctr < measures; ctr++)
                {       //For each measure
                    //printf("\tStart of definition for measure #%lu\n", ctr + 1);
                    //eof_gp_debug_log(inf, "\tMeasure bitmask:  ");
                    var bytemask = gpdata.ReadByte();      //Read the measure bitmask
                    //printf("%u\n", (bytemask & 0xFF));
                    if ((bytemask & 1) != 0)
                    {       //Time signature change (numerator)
                        //eof_gp_debug_log(inf, "\tTS numerator:  ");
                        word = gpdata.ReadByte();
                        //printf("%u\n", word);
                    }
                    if ((bytemask & 2) != 0)
                    {       //Time signature change (denominator)
                        //eof_gp_debug_log(inf, "\tTS denominator:  ");
                        word = gpdata.ReadByte();
                        //printf("%u\n", word);
                    }
                    if ((bytemask & 32) != 0)
                    {       //New section
                        //eof_gp_debug_log(inf, "\tNew section:  ");
                        ReadString(gpdata, false); //ReadString(gpdata);//eof_read_gp_string(inf, NULL, buffer, 1);       //Read section string
                        //printf("%s (coloring:  ", buffer);
                        word = gpdata.ReadByte();
                        //printf("R = %u, ", word);
                        word = gpdata.ReadByte();
                        //printf("G = %u, ", word);
                        word = gpdata.ReadByte();
                        //printf("B = %u)\n", word);
                        gpdata.ReadByte(); //Read unused value
                    }
                    if ((bytemask & 64) != 0)
                    {       //Key signature change
                        //eof_gp_debug_log(inf, "\tNew key signature:  ");
                        gpdata.ReadByte();  //Read the key
                        //printf("%d (%u %s, ", byte, abs(byte), (byte < 0) ? "flats" : "sharps");
                        gpdata.ReadByte();  //Read the major/minor byte
                        //printf("%s)\n", !byte ? "major" : "minor");
                    }
                    if ((ret.Version >= 500) && ((bytemask & 1) != 0 || (bytemask & 2) != 0))
                    {       //If either a new TS numerator or denominator was set, read the beam by eight notes values (only for version 5.x and higher of the format.  3.x/4.x are known to not have this info)
                        byte byte1, byte2, byte3, byte4;
                        //eof_gp_debug_log(inf, "\tBeam eight notes by:  ");
                        byte1 = gpdata.ReadByte();
                        byte2 = gpdata.ReadByte();
                        byte3 = gpdata.ReadByte();
                        byte4 = gpdata.ReadByte();
                        //printf("%d + %d + %d + %d = %d\n", byte1, byte2, byte3, byte4, byte1 + byte2 + byte3 + byte4);
                    }
                    if ((bytemask & 8) != 0)
                    {       //End of repeat
                        //eof_gp_debug_log(inf, "\tEnd of repeat:  ");
                        word = gpdata.ReadByte();  //Read number of repeats
                        //printf("%u repeats\n", word);
                    }
                    if ((bytemask & 128) != 0)
                    {       //Double bar
                        //puts("\t\t(Double bar)");
                    }

                    if (ret.Version >= 500)
                    {       //Versions 5.0 and newer of the format store unknown data/padding here
                        if ((bytemask & 16) != 0)
                        {       //Number of alternate ending
                            //eof_gp_debug_log(inf, "\tNumber of alternate ending:  ");
                            word = gpdata.ReadByte();  //Read alternate ending number
                            //printf("%u\n", word);
                        }
                        else
                        {
                            //eof_gp_debug_log(inf, "\t(skipping 1 byte of unused alternate ending data)\n");
                            gpdata.ReadByte();                 //Unknown data
                        }
                        //eof_gp_debug_log(inf, "\tTriplet feel:  ");
                        gpdata.ReadByte();  //Read triplet feel value
                        //printf("%s\n", !byte ? "none" : ((byte == 1) ? "Triplet 8th" : "Triplet 16th"));
                        //eof_gp_debug_log(inf, "\t(skipping 1 byte of unknown data)\n");
                        gpdata.ReadByte();                 //Unknown data
                    }
                }//For each measure

                for (int ctr = 0; ctr < tracks; ctr++)
                {       //For each track
                    //printf("\tStart of definition for track #%lu\n", ctr + 1);
                    //eof_gp_debug_log(inf, "\tTrack bitmask:  ");
                    var bytemask = gpdata.ReadByte();      //Read the measure bitmask
                    //printf("%u\n", (bytemask & 0xFF));
                    if ((bytemask & 1) != 0)
                    {
                        //puts("\t\t\t(Is a drum track)");
                    }
                    if ((bytemask & 2) != 0)
                    {
                        //puts("\t\t\t(Is a 12 string guitar track)");
                    }
                    if ((bytemask & 4) != 0)
                    {
                        //puts("\t\t\t(Is a banjo track)");
                    }
                    if ((bytemask & 16) != 0)
                    {
                        //puts("\t\t\t(Is marked for solo playback)");
                    }
                    if ((bytemask & 32) != 0)
                    {
                        //puts("\t\t\t(Is marked for muted playback)");
                    }
                    if ((bytemask & 64) != 0)
                    {
                        //puts("\t\t\t(Is marked for RSE playback)");
                    }
                    if ((bytemask & 128) != 0)
                    {
                        //puts("\t\t\t(Is set to have the tuning displayed)");
                    }
                    //eof_gp_debug_log(inf, "\tTrack name string:  ");
                    int nameLen;
                    var trackName = ReadString(gpdata,out nameLen);//eof_read_gp_string(inf, &word, buffer, 0);      //Read track name string
                    //puts(buffer);
                    //eof_gp_debug_log(inf, "\t(skipping ");
                    //printf("%d bytes of padding)\n", 40 - word);
                    var b = gpdata.ReadBytes(40 - nameLen); // pack_fseek(inf, 40 - word);                     //Skip the padding that follows the track name string
                    //eof_gp_debug_log(inf, "\tNumber of strings:  ");
                    //pack_ReadDWORDLE(inf, &strings[ctr]);   //Read the number of strings in this track
                    var numstrings = ReadDWORDLE(gpdata);

                    trackStrings.Add(numstrings);


                    //printf("%lu\n", strings[ctr]);
                    for (int ctr2 = 0; ctr2 < 7; ctr2++)
                    {       //For each of the 7 possible usable strings
                        if (ctr2 < numstrings)
                        {       //If this string is used
                            //eof_gp_debug_log(inf, "\tTuning for string #");
                            word = ReadDWORDLE(gpdata);  //Read the tuning for this string
                            //printf("%lu:  MIDI note %lu (%s)\n", ctr2 + 1, dword, eof_note_names[(dword + 3) % 12]);
                        }
                        else
                        {
                            //eof_gp_debug_log(inf, "\t(skipping definition for unused string)\n");
                            ReadString(gpdata, true);    //Skip this padding
                        }
                    }
                    //eof_gp_debug_log(inf, "\tMIDI port:  ");
                    word = ReadDWORDLE(gpdata);  //Read the MIDI port used for this track
                    //printf("%lu\n", dword);
                    //eof_gp_debug_log(inf, "\tMIDI channel:  ");
                    word = ReadDWORDLE(gpdata);  //Read the MIDI channel used for this track
                    //printf("%lu\n", dword);
                    //eof_gp_debug_log(inf, "\tEffects MIDI channel:  ");
                    word = ReadDWORDLE(gpdata);  //Read the MIDI channel used for this track's effects
                    //printf("%lu\n", dword);
                    //eof_gp_debug_log(inf, "\tNumber of frets:  ");
                    word = ReadDWORDLE(gpdata);  //Read the number of frets used for this track
                    //printf("%lu\n", dword);
                    //eof_gp_debug_log(inf, "\tCapo:  ");
                    word = ReadDWORDLE(gpdata);  //Read the capo position for this track
                    if (word != 0)
                    {       //If there is a capo
                        //printf("Fret %lu\n", dword);
                    }
                    else
                    {       //There is no capo
                        //puts("(none)");
                    }
                    //eof_gp_debug_log(inf, "\tTrack color:  ");
                    word = gpdata.ReadByte();
                    //printf("R = %u, ", word);
                    word = gpdata.ReadByte();
                    //printf("G = %u, ", word);
                    word = gpdata.ReadByte();
                    //printf("B = %u\n", word);
                    gpdata.ReadByte(); //Read unused value

                    if (ret.Version > 500)
                    {
                        //eof_gp_debug_log(inf, "\tTrack properties 1 bitmask:  ");
                        bytemask = gpdata.ReadByte();
                        //printf("%u\n", (bytemask & 0xFF));
                        //eof_gp_debug_log(inf, "\tTrack properties 2 bitmask:  ");
                        var bytemask2 = gpdata.ReadByte();
                        //printf("%u\n", (bytemask2 & 0xFF));
                        //eof_gp_debug_log(inf, "\t(skipping 1 byte of unknown data)\n");
                        gpdata.ReadByte();                 //Unknown data
                        //eof_gp_debug_log(inf, "\tMIDI bank:  ");
                        word = gpdata.ReadByte();
                        //printf("%u\n", word);
                        //eof_gp_debug_log(inf, "\tHuman playing value:  ");
                        word = gpdata.ReadByte();
                        //printf("%u\n", word);
                        //eof_gp_debug_log(inf, "\tAuto accentuation on the beat:  ");
                        word = gpdata.ReadByte();
                        //printf("%u\n", word);
                        //eof_gp_debug_log(inf, "\t(skipping 31 bytes of unknown data)\n");
                        gpdata.ReadBytes(31);//pack_fseek(inf, 31);            //Unknown data

                        //eof_gp_debug_log(inf, "\tSelected sound bank option:  ");
                        word = gpdata.ReadByte();
                        //printf("%u\n", word);
                        //eof_gp_debug_log(inf, "\t(skipping 7 bytes of unknown data)\n");
                        gpdata.ReadBytes(7);//pack_fseek(inf, 7);             //Unknown data
                        //eof_gp_debug_log(inf, "\tLow frequency band lowered ");
                        gpdata.ReadByte();
                        //printf("%d increments of .1dB\n", byte);
                        //eof_gp_debug_log(inf, "\tMid frequency band lowered ");
                        gpdata.ReadByte();
                        //printf("%d increments of .1dB\n", byte);
                        //eof_gp_debug_log(inf, "\tHigh frequency band lowered ");
                        gpdata.ReadByte();
                        //printf("%d increments of .1dB\n", byte);
                        //eof_gp_debug_log(inf, "\tGain lowered ");
                        gpdata.ReadByte();
                        //printf("%d increments of .1dB\n", byte);
                        //eof_gp_debug_log(inf, "\tTrack instrument effect 1:  ");
                        ReadString(gpdata, true);//eof_read_gp_string(inf, NULL, buffer, 1);       //Read track instrument effect 1
                        //puts(buffer);
                        //eof_gp_debug_log(inf, "\tTrack instrument effect 2:  ");
                        ReadString(gpdata, true);//eof_read_gp_string(inf, NULL, buffer, 1);       //Read track instrument effect 2
                        //puts(buffer);
                    }
                    else if (ret.Version == 500)
                    {
                        //eof_gp_debug_log(inf, "\t(skipping 45 bytes of unknown data)\n");
                        gpdata.ReadBytes(45); //pack_fseek(inf, 45);            //Unknown data
                    }
                }//For each track

                if (ret.Version >= 500)
                {
                    //eof_gp_debug_log(inf, "\t(skipping 1 byte of unknown data)\n");
                    var unk = gpdata.ReadByte();
                }

                //puts("\tStart of beat definitions:");
                for (int ctr = 0; ctr < measures; ctr++)
                {       //For each measure
                    //printf("\t-> Measure # %lu\n", ctr + 1);
                    for (int ctr2 = 0; ctr2 < tracks; ctr2++)
                    {       //For each track
                        int voice, maxvoices = 1;
                        if (ret.Version >= 500)
                        {       //Version 5.0 and higher of the format stores two "voices" (lead and bass) for each track
                            maxvoices = 2;
                        }
                        for (voice = 0; voice < maxvoices; voice++)
                        {       //For each voice
                            //printf("\t-> M#%lu -> Track # %lu (voice %u)\n", ctr + 1, ctr2 + 1, voice + 1);
                            //eof_gp_debug_log(inf, "\tNumber of beats:  ");
                            var beats = ReadDWORDLE(gpdata); // pack_ReadDWORDLE(inf, &beats);
                            //printf("%lu\n", beats);
                            for (int ctr3 = 0; ctr3 < beats; ctr3++)
                            {       //For each beat
                                //printf("\t-> M#%lu -> T#%lu -> Beat # %lu\n", ctr + 1, ctr2 + 1, ctr3 + 1);
                                //eof_gp_debug_log(inf, "\tBeat bitmask:  ");
                                var bytemask = gpdata.ReadByte();      //Read beat bitmask
                                //printf("%u\n", (bytemask & 0xFF));
                                if ((bytemask & 64) != 0)
                                {       //Beat is a rest
                                    //eof_gp_debug_log(inf, "\t(Rest beat type:  ");
                                    word = gpdata.ReadByte();
                                    //printf("%s)\n", !word ? "empty" : "rest");
                                }
                                //eof_gp_debug_log(inf, "\tBeat duration:  ");
                                gpdata.ReadByte();  //Read beat duration
                                //printf("%d\n", byte);
                                if ((bytemask & 1) != 0)
                                {       //Dotted note
                                    //puts("\t\t(Dotted note)");
                                }
                                if ((bytemask & 32) != 0)
                                {       //Beat is an N-tuplet
                                    //eof_gp_debug_log(inf, "\t(N-tuplet:  ");
                                    word = ReadDWORDLE(gpdata);
                                    //printf("%lu)\n", dword);
                                }
                                if ((bytemask & 2) != 0)
                                {       //Beat has a chord diagram
                                    //eof_gp_debug_log(inf, "\t(Chord diagram, ");
                                    word = gpdata.ReadByte();  //Read chord diagram format
                                    //printf("format %u)\n", word);
                                    if (word == 0)
                                    {       //Chord diagram format 0, ie. GP3
                                        //eof_gp_debug_log(inf, "\t\tChord name:  ");
                                        ReadString(gpdata, true);//eof_read_gp_string(inf, NULL, buffer, 1);       //Read chord name
                                        //puts(buffer);
                                        //eof_gp_debug_log(inf, "\t\tDiagram begins at fret #");
                                        word = ReadDWORDLE(gpdata);  //Read the diagram fret position
                                        //printf("%lu\n", dword);
                                        for (int ctr4 = 0; ctr4 < trackStrings[ctr2]; ctr4++)
                                        {       //For each string defined in the track
                                            //eof_gp_debug_log(inf, "\t\tString #");
                                            word = ReadDWORDLE(gpdata);  //Read the fret played on this string
                                            if (word == -1)
                                            {       //String not played
                                                //printf("%lu:  X\n", ctr4 + 1);
                                            }
                                            else
                                            {
                                                //printf("%lu:  %lu\n", ctr4 + 1, dword);
                                            }
                                        }
                                    }//Chord diagram format 0, ie. GP3
                                    else if (word == 1)
                                    {       //Chord diagram format 1, ie. GP4
                                        //eof_gp_debug_log(inf, "\t\tDisplay as ");
                                        gpdata.ReadByte();  //Read sharp/flat indicator
                                        //printf("%s\n", !word ? "flat" : "sharp");
                                        //eof_gp_debug_log(inf, "\t\t(skipping 3 bytes of unknown data)\n");
                                        gpdata.ReadBytes(3);             //Unknown data
                                        //eof_gp_debug_log(inf, "\t\tChord root:  ");
                                        gpdata.ReadByte();  //Read chord root
                                        //printf("%u\n", word);
                                        if (ret.Version / 100 == 3)
                                        {       //If it is a GP 3.x file
                                            //eof_gp_debug_log(inf, "\t\t(skipping 3 bytes of unknown data)\n");
                                            gpdata.ReadBytes(3);             //Unknown data
                                        }
                                        //eof_gp_debug_log(inf, "\t\tChord type:  ");
                                        gpdata.ReadByte();  //Read chord type
                                        //printf("%u\n", word);
                                        if (ret.Version / 100 == 3)
                                        {       //If it is a GP 3.x file
                                            //eof_gp_debug_log(inf, "\t\t(skipping 3 bytes of unknown data)\n");
                                            gpdata.ReadBytes(3);             //Unknown data
                                        }
                                        //eof_gp_debug_log(inf, "\t\t9th/11th/13th option:  ");
                                        gpdata.ReadByte();
                                        //printf("%u\n", word);
                                        if (ret.Version / 100 == 3)
                                        {       //If it is a GP 3.x file
                                            //eof_gp_debug_log(inf, "\t\t(skipping 3 bytes of unknown data)\n");
                                            gpdata.ReadBytes(3);             //Unknown data
                                        }
                                        //eof_gp_debug_log(inf, "\t\tBass note:  ");
                                        ReadDWORDLE(gpdata);  //Read lowest note played in string
                                        //printf("%lu (%s)\n", dword, eof_note_names[(dword + 3) % 12]);
                                        //eof_gp_debug_log(inf, "\t\t+/- option:  ");
                                        gpdata.ReadByte();
                                        //printf("%u\n", word);
                                        //eof_gp_debug_log(inf, "\t\t(skipping 4 bytes of unknown data)\n");
                                        gpdata.ReadBytes(4);             //Unknown data
                                        //eof_gp_debug_log(inf, "\t\tChord name:  ");
                                        //var chordName = ReadString(gpdata, true);
                                        word = gpdata.ReadByte();  //Read chord name string length
                                        var name = gpdata.ReadString(StringForm.ASCII, 20, true);

                                        //pack_fread(buffer, 20, inf);    //Read chord name (which is padded to 20 bytes)
                                        //buffer[word] = '\0';    //Ensure string is terminated to be the right length
                                        //puts(buffer);
                                        //eof_gp_debug_log(inf, "\t\t(skipping 2 bytes of unknown data)\n");
                                        gpdata.ReadBytes(2);             //Unknown data
                                        //eof_gp_debug_log(inf, "\t\tTonality of the fifth:  ");
                                        gpdata.ReadByte();
                                        //printf("%s\n", !byte ? "perfect" : ((byte == 1) ? "augmented" : "diminished"));
                                        if (ret.Version / 100 == 3)
                                        {       //If it is a GP 3.x file
                                            //eof_gp_debug_log(inf, "\t\t(skipping 3 bytes of unknown data)\n");
                                            //gpdata.ReadBytes(3);             //Unknown data
                                            gpdata.ReadBytes(3);
                                        }
                                        //eof_gp_debug_log(inf, "\t\tTonality of the ninth:  ");
                                        gpdata.ReadByte();
                                        //printf("%s\n", !byte ? "perfect" : ((byte == 1) ? "augmented" : "diminished"));
                                        if (ret.Version / 100 == 3)
                                        {       //If it is a GP 3.x file
                                            //eof_gp_debug_log(inf, "\t\t(skipping 3 bytes of unknown data)\n");
                                            //gpdata.ReadBytes(3);             //Unknown data
                                            gpdata.ReadBytes(3);
                                        }
                                        //eof_gp_debug_log(inf, "\t\tTonality of the eleventh:  ");
                                        gpdata.ReadByte();
                                        //printf("%s\n", !byte ? "perfect" : ((byte == 1) ? "augmented" : "diminished"));
                                        if (ret.Version / 100 == 3)
                                        {       //If it is a GP 3.x file
                                            //eof_gp_debug_log(inf, "\t\t(skipping 3 bytes of unknown data)\n");
                                            //gpdata.ReadBytes(3);             //Unknown data
                                            gpdata.ReadBytes(3);
                                        }
                                        //eof_gp_debug_log(inf, "\t\tBase fret for diagram:  ");
                                        word = ReadDWORDLE(gpdata);
                                        //printf("%lu\n", dword);
                                        for (int ctr4 = 0; ctr4 < 7; ctr4++)
                                        {       //For each of the 7 possible usable strings
                                            if (ctr4 < trackStrings[ctr2])
                                            {       //If this string is used in the track
                                                //eof_gp_debug_log(inf, "\t\tString #");
                                                word = ReadDWORDLE(gpdata);
                                                if (word == -1)
                                                {       //String not used in diagram
                                                    //printf("%lu:  (String unused)\n", ctr4 + 1);
                                                }
                                                else
                                                {
                                                    //printf("%lu:  Fret #%lu\n", ctr4 + 1, dword);
                                                }
                                            }
                                            else
                                            {
                                                //eof_gp_debug_log(inf, "\t\t(skipping definition for unused string)\n");
                                                ReadDWORDLE(gpdata);    //Skip this padding
                                            }
                                        }
                                        var barres = gpdata.ReadByte();        //Read the number of barres in this chord
                                        for (int ctr4 = 0; ctr4 < 5; ctr4++)
                                        {       //For each of the 5 possible barres
                                            if (ctr4 < barres)
                                            {       //If this barre is defined
                                                //eof_gp_debug_log(inf, "\t\tBarre #");
                                                word = gpdata.ReadByte();  //Read the barre position
                                                //printf("%lu:  Fret #%u\n", ctr4 + 1, word);
                                            }
                                            else
                                            {
                                                //eof_gp_debug_log(inf, "\t\t(skipping fret definition for undefined barre)\n");
                                                gpdata.ReadByte();
                                            }
                                        }
                                        for (int ctr4 = 0; ctr4 < 5; ctr4++)
                                        {       //For each of the 5 possible barres
                                            if (ctr4 < barres)
                                            {               //If this barre is defined
                                                //eof_gp_debug_log(inf, "\t\tBarre #");
                                                word = gpdata.ReadByte();  //Read the barre start string
                                                //printf("%lu starts at string #%u\n", ctr4 + 1, word);
                                            }
                                            else
                                            {
                                                //eof_gp_debug_log(inf, "\t\t(skipping start definition for undefined barre)\n");
                                                gpdata.ReadByte();
                                            }
                                        }
                                        for (int ctr4 = 0; ctr4 < 5; ctr4++)
                                        {       //For each of the 5 possible barres
                                            if (ctr4 < barres)
                                            {       //If this barre is defined
                                                //eof_gp_debug_log(inf, "\t\tBarre #");
                                                word = gpdata.ReadByte();  //Read the barre stop string
                                                //printf("%lu ends at string #%u\n", ctr4 + 1, word);
                                            }
                                            else
                                            {
                                                //eof_gp_debug_log(inf, "\t\t(skipping stop definition for undefined barre)\n");
                                                gpdata.ReadByte();
                                            }
                                        }
                                        //eof_gp_debug_log(inf, "\t\tChord includes first interval:  ");
                                        gpdata.ReadByte();
                                        //printf("%s\n", byte ? "no" : "yes");    //These booleans define whether the interval is EXCLUDED
                                        //eof_gp_debug_log(inf, "\t\tChord includes third interval:  ");
                                        gpdata.ReadByte();
                                        //printf("%s\n", byte ? "no" : "yes");
                                        //eof_gp_debug_log(inf, "\t\tChord includes fifth interval:  ");
                                        gpdata.ReadByte();
                                        //printf("%s\n", byte ? "no" : "yes");
                                        //eof_gp_debug_log(inf, "\t\tChord includes seventh interval:  ");
                                        gpdata.ReadByte();
                                        //printf("%s\n", byte ? "no" : "yes");
                                        //eof_gp_debug_log(inf, "\t\tChord includes ninth interval:  ");
                                        gpdata.ReadByte();
                                        //printf("%s\n", byte ? "no" : "yes");
                                        //eof_gp_debug_log(inf, "\t\tChord includes eleventh interval:  ");
                                        gpdata.ReadByte();
                                        //printf("%s\n", byte ? "no" : "yes");
                                        //eof_gp_debug_log(inf, "\t\tChord includes thirteenth interval:  ");
                                        gpdata.ReadByte();
                                        //printf("%s\n", byte ? "no" : "yes");
                                        //eof_gp_debug_log(inf, "\t\t(skipping 1 byte of unknown data)\n");
                                        gpdata.ReadByte(); //Unknown data
                                        for (int ctr4 = 0; ctr4 < 7; ctr4++)
                                        {       //For each of the 7 possible usable strings
                                            if (ctr4 < trackStrings[ctr2])
                                            {       //If this string is used in the track
                                                //eof_gp_debug_log(inf, "\t\tString #");
                                                gpdata.ReadByte();
                                                //printf("%lu is played with finger %d\n", ctr4 + 1, byte);
                                            }
                                            else
                                            {
                                                //eof_gp_debug_log(inf, "\t\t(skipping definition for unused string)\n");
                                                gpdata.ReadByte();         //Skip this padding
                                            }
                                        }
                                        //eof_gp_debug_log(inf, "\t\tChord fingering displayed:  ");
                                        gpdata.ReadByte();
                                        //printf("%s\n", !byte ? "no" : "yes");
                                    }//Chord diagram format 1, ie. GP4
                                }//Beat has a chord diagram
                                if ((bytemask & 4) != 0)
                                {       //Beat has text
                                    //eof_gp_debug_log(inf, "\tBeat text:  ");
                                    ReadString(gpdata, true);//eof_read_gp_string(inf, NULL, buffer, 1);       //Read beat text string
                                    //puts(buffer);
                                }
                                if ((bytemask & 8) != 0)
                                {       //Beat has effects
                                    byte byte1 = 0, byte2 = 0;

                                    //eof_gp_debug_log(inf, "\tBeat effects bitmask:  ");
                                    byte1 = gpdata.ReadByte(); //Read beat effects 1 bitmask
                                    //printf("%u\n", (byte1 & 0xFF));
                                    if (ret.Version >= 400)
                                    {       //Versions 4.0 and higher of the format have a second beat effects bitmask
                                        //eof_gp_debug_log(inf, "\tExtended beat effects bitmask:  ");
                                        byte2 = gpdata.ReadByte();
                                        //printf("%u\n", (byte2 & 0xFF));
                                        if ((byte2 & 1) != 0)
                                        {
                                            //puts("\t\tRasguedo");
                                        }
                                    }
                                    if ((byte1 & 4) != 0)
                                    {
                                        //puts("\t\t(Natural harmonic)");
                                    }
                                    if ((byte1 & 8) != 0)
                                    {
                                        //puts("\t\t(Artificial harmonic)");
                                    }
                                    if ((byte1 & 32) != 0)
                                    {       //Tapping/popping/slapping
                                        //eof_gp_debug_log(inf, "\tString effect:  ");
                                        var indic = gpdata.ReadByte();  //Read tapping/popping/slapping indicator
                                        if (indic == 0)
                                        {
                                            //puts("Tremolo");
                                        }
                                        else if (indic == 1)
                                        {
                                            //puts("Tapping");
                                        }
                                        else if (indic == 2)
                                        {
                                            //puts("Slapping");
                                        }
                                        else if (indic == 3)
                                        {
                                            //puts("Popping");
                                        }
                                        else
                                        {
                                            //puts("Unknown");
                                        }
                                        if (ret.Version < 400)
                                        {
                                            //eof_gp_debug_log(inf, "\t\tString effect value:  ");
                                            word = ReadDWORDLE(gpdata);
                                            //printf("%lu\n", dword);
                                        }
                                    }
                                    if ((byte2 & 4) != 0)
                                    {       //Tremolo bar
                                        eof_gp_parse_bend(gpdata);
                                    }
                                    if ((byte1 & 64) != 0)
                                    {       //Stroke effect
                                        //eof_gp_debug_log(inf, "\tDownstroke speed:  ");
                                        gpdata.ReadByte();
                                        //printf("%u\n", (byte & 0xFF));
                                        //eof_gp_debug_log(inf, "\tUpstroke speed:  ");
                                        gpdata.ReadByte();
                                        //printf("%u\n", (byte & 0xFF));
                                    }
                                    if ((byte2 & 2) != 0)
                                    {       //Pickstroke effect
                                        //eof_gp_debug_log(inf, "\tPickstroke effect:  ");
                                        gpdata.ReadByte();
                                        //printf("%s\n", !byte ? "none" : ((byte == 1) ? "up" : "down"));
                                    }
                                }//Beat has effects
                                if ((bytemask & 16) != 0)
                                {       //Beat has mix table change
                                    int volume_change = 0, pan_change = 0, chorus_change = 0, reverb_change = 0, phaser_change = 0, tremolo_change = 0, tempo_change = 0;

                                    //puts("\t\tBeat mix table change:");
                                    //eof_gp_debug_log(inf, "\t\tNew instrument number:  ");
                                    var newInst = gpdata.ReadByte();
                                    if (newInst == 0xFF)
                                    {
                                        //puts("(no change)");
                                    }
                                    else
                                    {
                                        //printf("%d\n", byte);
                                    }
                                    if (ret.Version >= 500)
                                    {       //These fields are only in version 5.x files
                                        //eof_gp_debug_log(inf, "\t\tRSE related number:  ");
                                        ReadDWORDLE(gpdata);
                                        //printf("%lu\n", dword);
                                        //eof_gp_debug_log(inf, "\t\tRSE related number:  ");
                                        ReadDWORDLE(gpdata);
                                        //printf("%lu\n", dword);
                                        //eof_gp_debug_log(inf, "\t\tRSE related number:  ");
                                        ReadDWORDLE(gpdata);
                                        //printf("%lu\n", dword);
                                        //eof_gp_debug_log(inf, "\t\t(skipping 4 bytes of unknown data)\n");
                                        //gpdata.ReadBytes(4);             //Unknown data
                                        gpdata.ReadBytes(4);
                                    }
                                    //eof_gp_debug_log(inf, "\t\tNew volume:  ");
                                    var newVolume = gpdata.ReadByte();
                                    if (newVolume == 0xFF)
                                    {
                                        //puts("(no change)");
                                    }
                                    else
                                    {
                                        volume_change = 1;
                                        //printf("%d\n", byte);
                                    }
                                    //eof_gp_debug_log(inf, "\t\tNew pan value:  ");
                                    var newPan = gpdata.ReadByte();
                                    if (newPan == 0xFF)
                                    {
                                        //puts("(no change)");
                                    }
                                    else
                                    {
                                        pan_change = 1;
                                        //printf("%d\n", byte);
                                    }
                                    //eof_gp_debug_log(inf, "\t\tNew chorus value:  ");
                                    var newChorus = gpdata.ReadByte();
                                    if (newChorus == 0xFF)
                                    {
                                        //puts("(no change)");
                                    }
                                    else
                                    {
                                        chorus_change = 1;
                                        //printf("%d\n", byte);
                                    }
                                    //eof_gp_debug_log(inf, "\t\tNew reverb value:  ");
                                    var newReverb = gpdata.ReadByte();
                                    if (newReverb == 0xFF)
                                    {
                                        //puts("(no change)");
                                    }
                                    else
                                    {
                                        reverb_change = 1;
                                        //printf("%d\n", byte);
                                    }
                                    //eof_gp_debug_log(inf, "\t\tNew phaser value:  ");
                                    var newPhaser = gpdata.ReadByte();
                                    if (newPhaser == 0xFF)
                                    {
                                        //puts("(no change)");
                                    }
                                    else
                                    {
                                        phaser_change = 1;
                                        //printf("%d\n", byte);
                                    }
                                    //eof_gp_debug_log(inf, "\t\tNew tremolo value:  ");
                                    var newTremelo = gpdata.ReadByte();
                                    if (newTremelo == 0xFF)
                                    {
                                        //puts("(no change)");
                                    }
                                    else
                                    {
                                        tremolo_change = 1;
                                        //printf("%d\n", byte);
                                    }

                                    if (ret.Version >= 500)
                                    {       //These fields are only in version 5.x files
                                        //eof_gp_debug_log(inf, "\t\tTempo text string:  ");
                                        ReadString(gpdata, true);//eof_read_gp_string(inf, NULL, buffer, 1);       //Read the tempo text string
                                        //puts(buffer);
                                    }
                                    //eof_gp_debug_log(inf, "\t\tNew tempo:  ");
                                    var newTempo = ReadDWORDLE(gpdata);
                                    if (newTempo == -1)
                                    {
                                        //puts("(no change)");
                                    }
                                    else
                                    {
                                        tempo_change = 1;
                                        //printf("%ld\n", (long)dword);
                                    }
                                    if (volume_change != -1)
                                    {       //This field only exists if a new volume was defined
                                        //eof_gp_debug_log(inf, "\t\tNew volume change transition:  ");
                                        gpdata.ReadByte();
                                        //printf("%d bars\n", byte);
                                    }
                                    if (pan_change != 0)
                                    {       //This field only exists if a new pan value was defined
                                        //eof_gp_debug_log(inf, "\t\tNew pan change transition:  ");
                                        gpdata.ReadByte();
                                        //printf("%d bars\n", byte);
                                    }
                                    if (chorus_change != 0)
                                    {       //This field only exists if a new  chorus value was defined
                                        //eof_gp_debug_log(inf, "\t\tNew chorus change transition:  ");
                                        gpdata.ReadByte();
                                        //printf("%d bars\n", byte);
                                    }
                                    if (reverb_change != 0)
                                    {       //This field only exists if a new reverb value was defined
                                        //eof_gp_debug_log(inf, "\t\tNew reverb change transition:  ");
                                        gpdata.ReadByte();
                                        //printf("%d bars\n", byte);
                                    }
                                    if (phaser_change != 0)
                                    {       //This field only exists if a new phaser value was defined
                                        //eof_gp_debug_log(inf, "\t\tNew phaser change transition:  ");
                                        gpdata.ReadByte();
                                        //printf("%d bars\n", byte);
                                    }
                                    if (tremolo_change != 0)
                                    {       //This field only exists if a new tremolo value was defined
                                        //eof_gp_debug_log(inf, "\t\tNew tremolo change transition:  ");
                                        gpdata.ReadByte();
                                        //printf("%d bars\n", byte);
                                    }
                                    if (tempo_change != 0)
                                    {       //These fields only exists if a new tempo was defined
                                        //eof_gp_debug_log(inf, "\t\tNew tempo change transition:  ");
                                        gpdata.ReadByte();
                                        //printf("%d bars\n", byte);
                                        if (ret.Version > 500)
                                        {       //This field only exists in versions newer than 5.0 of the format
                                            //eof_gp_debug_log(inf, "\t\tTempo text string hidden:  ");
                                            gpdata.ReadByte();
                                            //printf("%s\n", !byte ? "no" : "yes");
                                        }
                                    }
                                    if (ret.Version >= 400)
                                    {       //This field is not in version 3.0 files, assume 4.x or higher
                                        //eof_gp_debug_log(inf, "\t\tMix table change applied tracks bitmask:  ");
                                        gpdata.ReadByte();
                                        //printf("%u\n", (byte & 0xFF));
                                    }
                                    if (ret.Version >= 500)
                                    {       //This unknown byte is only in version 5.x files
                                        //eof_gp_debug_log(inf, "\t\t(skipping 1 byte of unknown data)\n");
                                        gpdata.ReadByte();             //Unknown data
                                    }
                                    if (ret.Version > 500)
                                    {       //These strings are only in versions newer than 5.0 of the format
                                        //eof_gp_debug_log(inf, "\t\tEffect 2 string:  ");
                                        ReadString(gpdata, true);//eof_read_gp_string(inf, NULL, buffer, 1);       //Read the Effect 2 string
                                        //puts(buffer);
                                        //eof_gp_debug_log(inf, "\t\tEffect 1 string:  ");
                                        ReadString(gpdata, true);//eof_read_gp_string(inf, NULL, buffer, 1);       //Read the Effect 1 string
                                        //puts(buffer);
                                    }
                                }//Beat has mix table change
                                //eof_gp_debug_log(inf, "\tUsed strings bitmask:  ");
                                var usedstrings = gpdata.ReadByte();
                                //printf("%u\n", (usedstrings & 0xFF));

                                for (byte ctr4 = 0, bitmask = 64; ctr4 < 7; ctr4++, bitmask >>= 1)
                                {       //For each of the 7 possible usable strings
                                    if ((bitmask & usedstrings) != 0)
                                    {       //If this string is used
                                        //printf("\t\t\tString %lu:\n", ctr4 + 1);
                                        //eof_gp_debug_log(inf, "\t\tNote bitmask:  ");
                                        bytemask = gpdata.ReadByte();
                                        //printf("%u\n", (bytemask & 0xFF));
                                        if ((bytemask & 32) != 0)
                                        {       //Note type is defined
                                            //eof_gp_debug_log(inf, "\t\tNote type:  ");
                                            gpdata.ReadByte();
                                            //printf("%s\n", (byte == 1) ? "normal" : ((byte == 2) ? "tie" : "dead"));
                                        }
                                        if ((bytemask & 1) != 0 && (ret.Version < 500))
                                        {       //Time independent duration (for versions of the format older than 5.x)
                                            //eof_gp_debug_log(inf, "\t\tTime independent duration values:  ");
                                            gpdata.ReadByte();
                                            //printf("%u ", (byte & 0xFF));
                                            gpdata.ReadByte();
                                            //printf("%u\n", (byte & 0xFF));
                                        }
                                        if ((bytemask & 16) != 0)
                                        {       //Note dynamic
                                            //eof_gp_debug_log(inf, "\t\tNote dynamic:  ");
                                            //var word = pack_getc(inf) - 1;      //Get the dynamic value and remap its values from 0 to 7
                                            gpdata.ReadByte();
                                            //printf("%s\n", note_dynamics[word % 8]);
                                        }
                                        if ((bytemask & 32) != 0)
                                        {       //Note type is defined
                                            //eof_gp_debug_log(inf, "\t\tFret number:  ");
                                            gpdata.ReadByte();
                                            //printf("%u\n", (byte & 0xFF));
                                        }
                                        if ((bytemask & 128) != 0)
                                        {       //Right/left hand fingering
                                            //eof_gp_debug_log(inf, "\t\tLeft hand fingering:  ");
                                            gpdata.ReadByte();
                                            //printf("%d\n", byte);
                                            //eof_gp_debug_log(inf, "\t\tRight hand fingering:  ");
                                            gpdata.ReadByte();
                                            //printf("%d\n", byte);
                                        }
                                        if ((bytemask & 1) != 0 && (ret.Version >= 500))
                                        {       //Time independent duration (for versions of the format 5.x or newer)
                                            //eof_gp_debug_log(inf, "\t\t(skipping 8 bytes of unknown time independent duration data)\n");
                                            //pack_fseek(inf, 8);             //Unknown data
                                            gpdata.ReadBytes(8);
                                        }
                                        if (ret.Version >= 500)
                                        {       //This padding isn't in version 3.x and 4.x files
                                            //eof_gp_debug_log(inf, "\t\t(skipping 1 byte of unknown data)\n");
                                            gpdata.ReadByte();             //Unknown data
                                        }
                                        if ((bytemask & 8) != 0)
                                        {       //Note effects
                                            byte byte1 = 0, byte2 = 0;
                                            //eof_gp_debug_log(inf, "\t\tNote effect bitmask:  ");
                                            byte1 = gpdata.ReadByte();
                                            //printf("%u\n", (byte1 & 0xFF));
                                            if (ret.Version >= 400)
                                            {       //Version 4.0 and higher of the file format has a second note effect bitmask
                                                //eof_gp_debug_log(inf, "\t\tNote effect 2 bitmask:  ");
                                                byte2 = gpdata.ReadByte();
                                                //printf("%u\n", (byte2 & 0xFF));
                                            }
                                            if ((byte1 & 1) != 0)
                                            {       //Bend
                                                //eof_gp_parse_bend(inf);
                                            }
                                            if ((byte1 & 2) != 0)
                                            {
                                                //puts("\t\t\t\t(Hammer on/pull off from current note)");
                                            }
                                            if ((byte1 & 4) != 0)
                                            {
                                                //puts("\t\t\t\t(Slide from current note)");
                                            }
                                            if ((byte1 & 8) != 0)
                                            {
                                                //puts("\t\t\t\t(Let ring)");
                                            }
                                            if ((byte1 & 16) != 0)
                                            {       //Grace note
                                                //puts("\t\t\t\t(Grace note)");
                                                //eof_gp_debug_log(inf, "\t\t\tGrace note fret number:  ");
                                                gpdata.ReadByte();
                                                //printf("%u\n", (byte & 0xFF));
                                                //eof_gp_debug_log(inf, "\t\t\tGrace note dynamic:  ");
                                                word = (gpdata.ReadByte() - 1) % 8;        //Get the dynamic value and remap its values from 0 to 7
                                                //printf("%s\n", note_dynamics[word]);
                                                if (ret.Version >= 500)
                                                {       //If the file version is 5.x or higher (this byte verified not to be in 3.0 and 4.06 files)
                                                    //eof_gp_debug_log(inf, "\t\t\tGrace note transition type:  ");
                                                    var btt = gpdata.ReadByte();
                                                    if (btt == 0)
                                                    {
                                                        //puts("none");
                                                    }
                                                    else if (btt == 1)
                                                    {
                                                        //puts("slide");
                                                    }
                                                    else if (btt == 2)
                                                    {
                                                        //puts("bend");
                                                    }
                                                    else if (btt == 3)
                                                    {
                                                        //puts("hammer");
                                                    }
                                                }
                                                else
                                                {       //The purpose of this field in 4.x or older files is unknown
                                                    //eof_gp_debug_log(inf, "\t\t(skipping 1 byte of unknown data)\n");
                                                    gpdata.ReadByte();             //Unknown data
                                                }
                                                //eof_gp_debug_log(inf, "\t\t\tGrace note duration:  ");
                                                gpdata.ReadByte();
                                                //printf("%u\n", (byte & 0xFF));
                                                if (ret.Version >= 500)
                                                {       //If the file version is 5.x or higher (this byte verified not to be in 3.0 and 4.06 files)
                                                    //eof_gp_debug_log(inf, "\t\t\tGrace note position:  ");
                                                    var bt = gpdata.ReadByte();
                                                    //printf("%u\n", (byte & 0xFF));
                                                    if ((bt & 1) != 0)
                                                    {
                                                        //puts("\t\t\t\t\t(dead note)");
                                                    }
                                                    //printf("\t\t\t\t\t%s\n", (byte & 2) ? "(on the beat)" : "(before the beat)");
                                                }
                                            }
                                            if ((byte2 & 1) != 0)
                                            {
                                                //puts("\t\t\t\t(Note played staccato");
                                            }
                                            if ((byte2 & 2) != 0)
                                            {
                                                //puts("\t\t\t\t(Palm mute");
                                            }
                                            if ((byte2 & 4) != 0)
                                            {       //Tremolo picking
                                                //eof_gp_debug_log(inf, "\t\t\tTremolo picking speed:  ");
                                                gpdata.ReadByte();
                                                //printf("%u\n", (byte & 0xFF));
                                            }
                                            if ((byte2 & 8) != 0)
                                            {       //Slide
                                                //eof_gp_debug_log(inf, "\t\t\tSlide type:  ");
                                                gpdata.ReadByte();
                                                //printf("%u\n", (byte & 0xFF));
                                            }
                                            if ((byte2 & 16) != 0)
                                            {       //Harmonic
                                                //eof_gp_debug_log(inf, "\t\t\tHarmonic type:  ");
                                                var bt = gpdata.ReadByte();
                                                //printf("%u\n", (byte & 0xFF));
                                                if (bt == 2)
                                                {       //Artificial harmonic
                                                    //puts("\t\t\t\t\t(Artificial harmonic)");
                                                    //eof_gp_debug_log(inf, "\t\t\t\tHarmonic note:  ");
                                                    gpdata.ReadByte();  //Read harmonic note
                                                    //printf("%s", eof_note_names[(byte + 3) % 12]);
                                                    var bto = gpdata.ReadByte();  //Read sharp/flat status
                                                    if (bto == 0xFF)
                                                    {
                                                        //putchar('b');
                                                    }
                                                    else if (bto == 1)
                                                    {
                                                        //putchar('#');
                                                    }
                                                    var b = gpdata.ReadByte();  //Read octave status
                                                    if (b == 0)
                                                    {
                                                        //printf(" loco");
                                                    }
                                                    else if (b == 1)
                                                    {
                                                        //printf(" 8va");
                                                    }
                                                    else if (b == 2)
                                                    {
                                                        //printf(" 15ma");
                                                    }
                                                    //putchar('\n');
                                                }
                                                else if (bt == 3)
                                                {       //Tapped harmonic
                                                    //puts("\t\t\t\t\t(Tapped harmonic)");
                                                    //eof_gp_debug_log(inf, "\t\t\t\tRight hand fret:  ");
                                                    gpdata.ReadByte();
                                                    //printf("%u\n", (byte & 0xFF));
                                                }
                                            }
                                            if ((byte2 & 32) != 0)
                                            {       //Trill
                                                //eof_gp_debug_log(inf, "\t\t\tTrill with fret:  ");
                                                var bt = gpdata.ReadByte();
                                                //printf("%u\n", (byte & 0xFF));
                                                //eof_gp_debug_log(inf, "\t\t\tTrill duration:  ");
                                                bt = gpdata.ReadByte();
                                                //printf("%u\n", (byte & 0xFF));
                                            }
                                            if ((byte2 & 64) != 0)
                                            {
                                                //puts("\t\t\t\t(Vibrato)");
                                            }
                                        }//Note effects
                                    }//If this string is used
                                }//For each of the 7 possible usable strings
                                if (ret.Version >= 500)
                                {       //Version 5.0 and higher of the file format stores a note transpose mask and unknown data here
                                    //eof_gp_debug_log(inf, "\tTranspose bitmask:  ");
                                    word = ReadWORDLE(gpdata);
                                    //printf("%u\n", word);
                                    if ((word & 0x800) != 0)
                                    {       //If bit 11 of the transpose bitmask was set, there is an additional byte of unknown data
                                        //eof_gp_debug_log(inf, "\t\t(skipping 1 byte of unknown transpose data)\n");
                                        gpdata.ReadByte();     //Unknown data
                                    }
                                }
                            }//For each beat
                        }//For each voice
                        if (ret.Version >= 500)
                        {
                            //eof_gp_debug_log(inf, "\t(skipping 1 byte of unknown data)\n");
                            gpdata.ReadByte();
                        }
                    }//For each track
                }//For each measure

                return ret;

            }

            static int ReadWORDLE(DJsIO io)
            {
                var ret = (int)io.ReadInt16(true);

                Debug.WriteLine("WORD: " + ret.ToString());
                return ret;
            }
            static int ReadDWORDLE(DJsIO io)
            {
                var ret = (int)io.ReadInt32(true);

                Debug.WriteLine("DWORD: " + ret.ToString());

                return ret;
            }

            static List<string> ReadStringList(DJsIO io)
            {
                var ret = new List<string>();
                var len = io.ReadInt16();
                Debug.WriteLine("StringListLen: " + len.ToString());
                for (int x = 0; x < len; x++)
                {
                    ret.Add(ReadString(io, true));
                }
                return ret;
            }


            static string ReadString(DJsIO io, out int len)
            {
                
                len = io.ReadByte();
                var ret = io.ReadString(StringForm.ASCII, len, StringRead.Defined, true);

                Debug.WriteLine("ReadString Len: " + len.ToString());
                Debug.WriteLine("ReadString: " + ret);
                return ret;
            }

            static string ReadString(DJsIO io, bool readLen)
            {
                if (readLen)
                {
                    var dw = ReadDWORDLE(io);

                    Debug.WriteLine("ReadString DW: " + dw.ToString()); 
                }
                var len = io.ReadByte();
                var ret = io.ReadString(StringForm.ASCII, len, StringRead.Defined, true);

                Debug.WriteLine("ReadString Len: " + len.ToString());
                Debug.WriteLine("ReadString: " + ret);
                return ret;
            }

            static void eof_gp_parse_bend(DJsIO io)
            {

                var word = io.ReadByte();
                //word = pack_getc(inf);  //Read bend type
                if (word == 1)
                {
                    //puts("Bend");
                }
                else if (word == 2)
                {
                    //puts("Bend and release");
                }
                else if (word == 3)
                {
                    // puts("Bend, release and bend");
                }
                else if (word == 4)
                {
                    // puts("Pre bend");
                }
                else if (word == 5)
                {
                    //puts("Pre bend and release");
                }
                else if (word == 6)
                {
                    //puts("Tremolo dip");
                }
                else if (word == 7)
                {
                    // puts("Tremolo dive");
                }
                else if (word == 8)
                {
                    // puts("Tremolo release up");
                }
                else if (word == 9)
                {
                    // puts("Tremolo inverted dip");
                }
                else if (word == 10)
                {
                    // puts("Tremolo return");
                }
                else if (word == 11)
                {
                    // puts("Tremolo release down");
                }
                //eof_gp_debug_log(inf, "\t\tHeight:  ");
                var height = ReadDWORDLE(io);
                //pack_ReadDWORDLE(inf, &height); //Read bend height
                //printf("%lu cents\n", height);
                //eof_gp_debug_log(inf, "\t\tNumber of points:  ");
                var points = ReadDWORDLE(io);
                //pack_ReadDWORDLE(inf, &points); //Read number of bend points
                //printf("%lu points\n", points);
                for (int ctr = 0; ctr < points; ctr++)
                {

                    //eof_gp_debug_log(inf, "\t\t\tTime relative to previous point:  ");
                    var timeRel = ReadDWORDLE(io); //pack_ReadDWORDLE(inf, &dword);
                    //printf("%lu sixtieths\n", dword);
                    //eof_gp_debug_log(inf, "\t\t\tVertical position:  ");
                    var vartPos = ReadDWORDLE(io);
                    //pack_ReadDWORDLE(inf, &dword);
                    //printf("%ld * 25 cents\n", (long)dword);
                    //eof_gp_debug_log(inf, "\t\t\tVibrato type:  ");
                    //word = pack_getc(inf);
                    var vibrato = io.ReadByte();
                    if (vibrato == 0)
                    {
                        //puts("none");
                    }
                    else if (word == 1)
                    {
                        //puts("fast");
                    }
                    else if (word == 2)
                    {
                        // puts("average");
                    }
                    else if (word == 3)
                    {
                        //  puts("slow");
                    }
                }
            }

        }

        public class GPLyric
        {
            public int Track { get; set; }
            public int Bar { get; set; }
            public string Lyric { get; set; }

        }

        public class GPData
        {
            public string VersionDesc { get; set; }
            public string Title { get; set; }
            public string SubTitle { get; set; }
            public string Artist { get; set; }
            public string Album { get; set; }

            public string Lyricist { get; set; }


            public string Composer { get; set; }
            public string Copyright { get; set; }
            public string Transcriber { get; set; }
            public string Instructions { get; set; }
            public List<string> NoticeEntries { get; set; }

            public string ShuffleRhythmFeel { get; set; }

            public List<GPLyric> Lyrics { get; set; }

            public int MasterVolume { get; set; }
            public int band32hz { get; set; }
            public int band60hz { get; set; }
            public int band125hz { get; set; }
            public int band250hz { get; set; }
            public int band500hz { get; set; }
            public int band1khz { get; set; }
            public int band2khz { get; set; }
            public int band4khz { get; set; }
            public int band8khz { get; set; }
            public int band16khz { get; set; }
            public int bandGain { get; set; }

            public int Version
            {
                get
                {
                    int fileversion = -1;
                    if (VersionDesc == "FICHIER GUITARE PRO v1.01")
                    {
                        fileversion = 101;
                    }
                    else if (VersionDesc == "FICHIER GUITARE PRO v1.02")
                    {
                        fileversion = 102;
                    }
                    else if (VersionDesc == "FICHIER GUITARE PRO v1.03")
                    {
                        fileversion = 103;
                    }
                    else if (VersionDesc == "FICHIER GUITARE PRO v1.04")
                    {
                        fileversion = 104;
                    }
                    else if (VersionDesc == "FICHIER GUITAR PRO v2.20")
                    {
                        fileversion = 220;
                    }
                    else if (VersionDesc == "FICHIER GUITAR PRO v2.21")
                    {
                        fileversion = 221;
                    }
                    else if (VersionDesc == "FICHIER GUITAR PRO v3.00")
                    {
                        fileversion = 300;
                    }
                    else if (VersionDesc == "FICHIER GUITAR PRO v4.00")
                    {
                        fileversion = 400;
                    }
                    else if (VersionDesc == "FICHIER GUITAR PRO v4.06")
                    {
                        fileversion = 406;
                    }
                    else if (VersionDesc == "FICHIER GUITAR PRO L4.06")
                    {
                        fileversion = 406;
                    }
                    else if (VersionDesc == "FICHIER GUITAR PRO v5.00")
                    {
                        fileversion = 500;
                    }
                    else if (VersionDesc == "FICHIER GUITAR PRO v5.10")
                    {
                        fileversion = 510;
                    }
                    return fileversion;
                }
            }
        }


    }
}
