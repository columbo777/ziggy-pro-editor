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
using System.Runtime.InteropServices;
using NAudio;
using NAudio.Wave;


namespace ProUpgradeEditor.UI
{

    partial class MainForm
    {
        int midiDeviceIndex = 0;


        private const int SysExBufferSize = 128;
        private InputDevice inDevice = null;




        bool ConnectMidiDevice(int device, bool silent)
        {


            bool ret = false;
            try
            {
                if (InputDevice.DeviceCount == 0)
                {
                    if (!silent)
                        MessageBox.Show("No MIDI input devices available.");
                }
                else
                {
                    try
                    {

                        inDevice = new InputDevice(device, SynchronizationContext.Current);
                        inDevice.SysExMessageReceived += new EventHandler<SysExMessageEventArgs>(inDevice_SysExMessageReceived);
                        inDevice.StartRecording();

                        ret = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToLower().Contains("in use"))
                        {

                            ShutDownMidiDevice();

                            try
                            {
                                inDevice = new InputDevice(device, SynchronizationContext.Current);
                                inDevice.SysExMessageReceived += new EventHandler<SysExMessageEventArgs>(inDevice_SysExMessageReceived);
                                inDevice.StartRecording();
                            }
                            catch { }
                        }

                        if (!silent)
                            MessageBox.Show(ex.Message, "Error!");
                    }
                }
            }
            catch { }

            return ret;
        }
        private void ConnectMidiDevice()
        {
            var item = comboBoxMidiInput.SelectedItem as MidiInputListItem;
            if (item != null)
            {
                ConnectMidiDevice(item.index, true);
            }
        }

        int[] guitar6Notes = new int[6] { 0, 0, 0, 0, 0, 0 };
        int resetTime = 1;
        int resetCount = 0;
        DateTime dtLastStrum = DateTime.Now;

        void inDevice_SysExMessageReceived(object sender, SysExMessageEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    CheckInstrumentNote(e);
                }));
            }
            else
            {
                CheckInstrumentNote(e);
            }

        }

        void CheckInstrumentNote(SysExMessageEventArgs e)
        {
            if (labelMidiInputDeviceState.Text == "None Connected")
            {
                if (e.Message.Length == 18)
                {
                    if (e.Message.GetBytes()[3] == 8)
                    {
                        labelMidiInputDeviceState.Text = "Squier";
                    }
                    else if (e.Message.GetBytes()[3] == 0x0A)
                    {
                        labelMidiInputDeviceState.Text = "Mustang";
                    }
                }
            }
            else
            {
                TextBox[] tb = GetNoteBoxes();

                if (e.Message.Length == 18)
                {
                    if (checkBoxEnableClearTimer.Checked)
                    {
                        resetCount--;
                        if (resetCount <= 0)
                        {
                            bool allzero = true;
                            for (int x = 0; x < 6; x++)
                            {
                                if (tb[x].Text.Length > 0 &&
                                    tb[x].Text != "0")
                                {
                                    allzero = false;
                                    break;
                                }
                            }
                            if (allzero)
                            {
                                foreach (var tbox in tb)
                                    tbox.Text = "";
                                resetCount = resetTime;
                            }
                        }
                    }
                }
                if (e.Message.Length == 8)
                {
                    resetCount = resetTime;

                    byte[] b = e.Message.GetBytes();

                    int noteString = b[5] - 1;

                    byte[] offsets = new byte[] { 0x40, 0x3B, 0x37, 0x32, 0x2D, 0x28 };

                    bool isStrum = b[4] == 5;


                    if (isStrum)
                    {
                        var dtNowStrum = DateTime.Now;

                        var dtStrumTime = (dtNowStrum - dtLastStrum);


                        if (checkChordMode.Checked == false ||
                            (checkChordMode.Checked == true && checkBoxChordStrum.Checked == true))
                        {
                            if (!checkBoxChordStrum.Checked)
                            {
                                ClearNoteBoxes();
                                ClearHoldBoxes();
                            }
                        }


                        tb[noteString].Text = (guitar6Notes[noteString]).ToString();
                        if (checkTwoNotePowerChord.Checked || checkThreeNotePowerChord.Checked)
                        {
                            if (checkTwoNotePowerChord.Checked)
                            {
                                if (noteString > 0)
                                {
                                    int noteP1 = tb[noteString].Text.ToInt();
                                    if (!noteP1.IsNull())
                                    {
                                        int offset = 2;
                                        if (noteString == 3)
                                            offset = 3;
                                        tb[noteString - 1].Text = (noteP1 + offset).ToString();
                                    }
                                }
                            }
                            if (checkThreeNotePowerChord.Checked)
                            {
                                if (noteString > 1)
                                {
                                    int noteP1 = tb[noteString].Text.ToInt();
                                    if (!noteP1.IsNull())
                                    {
                                        int offset = 2;
                                        if (noteString == 3 || noteString == 2)
                                            offset = 3;
                                        tb[noteString - 2].Text = (noteP1 + offset).ToString();
                                    }
                                }
                            }
                        }

                        if (checkChordMode.Checked == false ||
                                (checkChordMode.Checked == true && checkBoxChordStrum.Checked == true))
                        {
                            if (checkBoxChordStrum.Checked == true)
                            {
                                GetHoldBoxes()[noteString].Text = tb[noteString].Text;
                            }
                            else
                            {
                                PlaceHeldNotesIntoHoldBoxes();
                            }
                        }
                        else
                        {
                            PlaceHeldNotesIntoHoldBoxes();
                        }

                        if (checkRealtimeNotes.Checked)
                        {
                            if ((checkChordMode.Checked == true && checkBoxChordStrum.Checked == true) == false)
                            {
                                EditorPro.BackupSequence();
                                PlaceNote(SelectNextEnum.UseConfiguration);
                                ClearNoteBoxes();
                                ClearHoldBoxes();
                            }
                        }

                        if (checkBoxPlayMidiStrum.Checked)
                        {
                            if (dtStrumTime.TotalMilliseconds > 10)
                            {
                                PlayHoldBoxMidi();
                            }
                        }

                        dtLastStrum = DateTime.Now;
                    }
                    else
                    {
                        guitar6Notes[noteString] = b[6] - offsets[noteString];
                        tb[noteString].Text = (guitar6Notes[noteString]).ToString();
                    }


                    if (checkBoxClearIfNoFrets.Checked == true)
                    {
                        bool allzero = true;
                        for (int x = 0; x < 6; x++)
                        {
                            if (tb[x].Text.Length > 0 &&
                                tb[x].Text != "0")
                            {
                                allzero = false;
                                break;
                            }
                        }
                        if (allzero)
                        {
                            StopHoldBoxMidi();
                            ClearHoldBoxes();
                            ClearNoteBoxes();
                        }
                    }
                }
            }
        }



        private void DisconnectMidiDevice()
        {
            if (inDevice != null)
            {
                try
                {
                    inDevice.StopRecording();
                }
                catch { }
                try
                {
                    inDevice.Reset();
                }
                catch { }
                try
                {
                    inDevice.Close();
                }
                catch { }
                try
                {
                    inDevice = null;
                }
                catch { }

                labelMidiInputDeviceState.Text = "None Connected";
            }
        }

        bool midiPlaybackEnabled = true;
        bool mp3PlaybackEnabled = true;

        int midiPlaybackDeviceVolume = 100;

        Sequence midiPlaybackSequence = null;
        Sequencer midiPlaybackSequencer = new Sequencer();
        OutputDevice midiPlaybackDevice = null;
        public bool MidiPlaybackInProgress
        {
            get { return EditorPro.InPlayback; }
            set { EditorPro.InPlayback = value; }
        }

        public int MidiPlaybackPosition
        {
            get { return midiPlaybackSequencer == null ? EditorPro.MidiPlaybackPosition : midiPlaybackSequencer.Position; }
            set
            {
                if (midiPlaybackSequencer != null)
                {
                    midiPlaybackSequencer.Position = value;
                }
                EditorPro.MidiPlaybackPosition = value;
                EditorG5.MidiPlaybackPosition = value;
            }
        }


        private void timerMidiPlayback_Tick(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            try
            {
                int ipos = 0;

                if (midiPlaybackSequencer == null)
                {
                    midiPlaybackSequencer = new Sequencer();
                }

                if (!MidiPlaybackInProgress)
                {
                    if (checkChordMode.Checked &&
                        checkBoxChordStrum.Checked &&
                        (DateTime.Now - dtLastStrum).TotalMilliseconds > 500 &&
                        labelMidiInputDeviceState.Text != "None Connected")
                    {
                        var hb = GetHoldBoxes();
                        for (int x = 0; x < hb.Length; x++)
                        {
                            if (hb[x].Text.Length > 0)
                            {
                                if (checkRealtimeNotes.Checked)
                                {
                                    PlaceNote(SelectNextEnum.UseConfiguration);
                                }
                                ClearNoteBoxes();
                                ClearHoldBoxes();

                                break;
                            }
                        }
                    }
                }

                if (MidiPlaybackInProgress)
                {
                    ipos = (int)MidiPlaybackPosition;

                    var pos = ProGuitarTrack.TickToTime(ipos);

                    EditorPro.PlaybackPosition = pos;
                    EditorPro.MidiPlaybackPosition = MidiPlaybackPosition;


                    if (checkBoxMidiPlaybackScroll.Checked)
                    {
                        EditorPro.ScrollToTick(ipos);
                    }

                    try
                    {
                        if (mp3PlaybackEnabled && MidiPlaybackInProgress)
                        {
                            if (mp3Player.LoadedMP3File && !mp3Player.PlayingMP3File)
                            {
                                BeginSongMP3Playback();
                            }
                        }
                    }
                    catch { }


                    if (MidiPlaybackPosition >= ProGuitarTrack.TotalSongTicks)
                    {
                        StopMidiPlayback();
                    }
                }
            }
            catch { }
        }

        void ShutDownMidiDevice()
        {
            DisconnectMidiDevice();

            try
            {
                timerMidiPlayback.Enabled = false;
                StopMidiPlayback();
            }
            catch { }
            try
            {
                if (midiPlaybackSequencer != null)
                {
                    midiPlaybackSequencer.Dispose();
                    midiPlaybackSequencer = null;
                }
                if (midiPlaybackDevice != null)
                {
                    midiPlaybackDevice.Dispose();
                    midiPlaybackDevice = null;
                }
            }
            catch { }
            MidiPlaybackInProgress = false;
            playbackSequencerAttached = false;
        }

        bool CreateMidiPlaybackDeviceIfNull()
        {
            var ret = false;
            try
            {
                if (midiPlaybackDevice == null)
                {
                    midiPlaybackDevice = OutputDevice.CreateDevice(midiDeviceIndex, SynchronizationContext.Current);
                }
                ret = midiPlaybackDevice != null;
            }
            catch { }
            return ret;
        }



        naudioPlayer mp3Player = new naudioPlayer();


        bool playbackSequencerAttached = false;

        void PlayMidiFromSelection()
        {
            try
            {
                if (EditorPro.Sequence == null)
                    return;

                if (!CreateMidiPlaybackDeviceIfNull())
                    return;

                if (midiPlaybackSequencer == null)
                {
                    midiPlaybackSequencer = new Sequencer();
                }

                if (playbackSequencerAttached == false)
                {
                    this.midiPlaybackSequencer.PlayingCompleted += new EventHandler(midiPlaybackSequencer_PlayingCompleted);
                    this.midiPlaybackSequencer.ChannelMessagePlayed += new EventHandler<ChannelMessageEventArgs>(midiPlaybackSequencer_ChannelMessagePlayed);

                    this.midiPlaybackSequencer.SysExMessagePlayed += new EventHandler<SysExMessageEventArgs>(midiPlaybackSequencer_SysExMessagePlayed);


                    playbackSequencerAttached = true;
                }

                if (MidiPlaybackInProgress == true)
                {
                    midiPlaybackSequencer.Stop();
                    MidiPlaybackInProgress = false;
                }

                if (midiPlaybackSequence != null)
                {
                    midiPlaybackSequence.Dispose();
                }
                midiPlaybackSequence = new Sequence(FileType.Pro, EditorPro.Sequence.Division);
                midiPlaybackSequence.Format = EditorPro.Sequence.Format;

                midiPlaybackSequencer.Sequence = midiPlaybackSequence;

                int pos = 0;
                if (SelectedChord != null)
                {
                    pos = SelectedChord.DownTick - Utility.ScollToSelectionOffset;
                }

                var mainTrack = EditorPro.GuitarTrack.GetTrack();
                if (mainTrack != null)
                {

                    midiPlaybackSequence.AddTempo(EditorPro.GuitarTrack.GetTempoTrack().Clone());
                    Track t = new Track(FileType.Pro, mainTrack.Name);

                    foreach (var gc in EditorPro.GuitarTrack.Messages.Chords.Where(x => x.IsPureArpeggioHelper == false).ToList())
                    {
                        foreach (var cn in gc.Notes.Where(x => x.IsArpeggioNote == false && x.TickLength > 0 && x.Data2 >= 100).ToList())
                        {
                            t.Insert(gc.DownTick - Utility.ScollToSelectionOffset, new ChannelMessage(ChannelCommand.NoteOn, cn.Data1, cn.Data2));
                            t.Insert(gc.UpTick - 1 - Utility.ScollToSelectionOffset, new ChannelMessage(ChannelCommand.NoteOff, cn.Data1, 0));
                        }
                    }
                    midiPlaybackSequence.Add(t);
                }

                if (SelectedSong != null)
                {
                    if (SelectedSong.EnableSongMP3Playback &&
                        SelectedSong.SongMP3Location.FileExists())
                    {
                        LoadSongMP3Playback();
                    }
                }


                midiPlaybackSequencer.Start();

                MidiPlaybackPosition = pos;
                midiPlaybackSequencer.Position = pos;

                MidiPlaybackInProgress = true;

                timerMidiPlayback.Enabled = true;



            }
            catch
            {
                MidiPlaybackInProgress = false;
                timerMidiPlayback.Enabled = false;
            }
        }

        void StopMP3Playback()
        {
            try
            {
                if (mp3Player.PlayingMP3File)
                {
                    mp3Player.Stop();
                }
            }
            catch { }
        }

        private bool LoadSongMP3Playback()
        {

            try
            {
                if (SelectedSong != null)
                {
                    if (SelectedSong.EnableSongMP3Playback &&
                        File.Exists(SelectedSong.SongMP3Location))
                    {
                        mp3Player.Load(SelectedSong.SongMP3Location, this.Handle);

                        currentMp3PlaybackTime = 0;
                    }
                }
            }
            catch { }

            return mp3Player.LoadedMP3File;
        }

        double currentMp3PlaybackMidiStartTime = 0;
        double currentMp3PlaybackTime = 0;
        DateTime timeStartMp3Playback = DateTime.Now;


        private bool BeginSongMP3Playback()
        {
            bool started = false;
            try
            {

                if (mp3Player.LoadedMP3File && mp3Player.PlayingMP3File == false)
                {

                    double waitSecs = 0;
                    if (!SelectedSong.SongMP3PlaybackOffset.IsNull())
                    {
                        waitSecs = ((double)SelectedSong.SongMP3PlaybackOffset) / 1000.0;
                    }

                    currentMp3PlaybackTime = EditorPro.GuitarTrack.TickToTime(MidiPlaybackPosition);

                    var d = currentMp3PlaybackTime + waitSecs;

                    if (d >= 0)
                    {
                        currentMp3PlaybackMidiStartTime = d;

                        mp3Player.Play(currentMp3PlaybackMidiStartTime);

                        started = true;
                    }
                }
            }
            catch { }
            return started;
        }

        void midiPlaybackSequencer_SysExMessagePlayed(object sender, SysExMessageEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    OnSysex(e);
                }));
            }
            else
            {
                OnSysex(e);
            }

        }

        void OnSysex(SysExMessageEventArgs e)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new MethodInvoker(delegate
                    {
                        if (midiPlaybackDevice != null &&
                            midiPlaybackDevice.IsDisposed == false)
                        {
                            midiPlaybackDevice.Send(e.Message);
                        }
                    }));
                }
                else
                {
                    if (midiPlaybackDevice != null &&
                            midiPlaybackDevice.IsDisposed == false)
                    {
                        midiPlaybackDevice.Send(e.Message);
                    }
                }
            }
            catch { }
        }



        void midiPlaybackSequencer_ChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    if (midiPlaybackSequencer != null)
                    {
                        OnChannelMessagePlay(e);
                    }
                }));
            }
            else
            {
                if (midiPlaybackSequencer != null)
                {
                    OnChannelMessagePlay(e);
                }
            }
        }


        void OnChannelMessagePlay(ChannelMessageEventArgs e)
        {
            try
            {
                if (midiPlaybackDevice == null ||
                    midiPlaybackDevice.IsDisposed)
                    return;

                if (e.Message.Data1.GetData1Difficulty(true) ==
                    EditorPro.CurrentDifficulty)
                {
                    if (e.Message.MidiChannel == Utility.ChannelArpeggio)
                        return;

                    int data2 = 0;
                    if (midiPlaybackEnabled && e.Message.Command == ChannelCommand.NoteOn)
                    {
                        data2 = midiPlaybackDeviceVolume;
                    }
                    midiPlaybackDevice.Send(new ChannelMessage(e.Message.Command, GetTunedNote(e.Message.Data1, e.Message.Data2), data2));
                    
                    if (e.Message.Command == ChannelCommand.NoteOff ||
                        e.Message.Data2 == 0)
                    {
                        for (int x = 0; x < 128; x++)
                        {
                            midiPlaybackDevice.Send(new ChannelMessage(e.Message.Command, x, 0));
                        }
                    }
                }
            }
            catch { }
        }

        public int GetTunedNote(int data1, int data2)
        {

            //E3/68 - B2/63 - G2/59 - D2/54 - A1/49 - E1/44
            var ns = data1.GetNoteString6();
            var note = 0;
            switch (ns)
            {
                case 0:
                    note = 40;
                    break;
                case 1:
                    note = 45;
                    break;
                case 2:
                    note = 50;
                    break;
                case 3:
                    note = 55;
                    break;
                case 4:
                    note = 59;
                    break;
                case 5:
                    note = 64;
                    break;
            }

            int tunedNote = note + (data2 - 100);

            var sci = SelectedSong;
            if (sci != null)
            {
                int tuning = sci.GuitarTuning[ns].ToInt();
                if (!tuning.IsNull())
                {
                    tunedNote += tuning;
                }
            }
            return tunedNote;
        }

        void midiPlaybackSequencer_PlayingCompleted(object sender, EventArgs e)
        {

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    MidiPlaybackInProgress = false;
                    timerMidiPlayback.Enabled = false;
                }));
            }
            else
            {
                MidiPlaybackInProgress = false;
                timerMidiPlayback.Enabled = false;
            }
        }

        void StopMidiPlayback()
        {
            bool inProgress = MidiPlaybackInProgress;

            if (inProgress)
            {
                MidiPlaybackInProgress = false;
                timerMidiPlayback.Enabled = false;
            }
            else
            {
                return;
            }

            StopMP3Playback();

            try
            {
                if (midiPlaybackSequencer != null)
                {
                    lock (midiPlaybackSequencer)
                    {
                        if (InvokeRequired)
                        {
                            BeginInvoke(new MethodInvoker(delegate()
                            {
                                midiPlaybackSequencer.Stop();
                            }));
                        }
                        else
                        {
                            midiPlaybackSequencer.Stop();
                        }
                    }
                }
            }
            catch { }

            try
            {
                for (int x = 0; x < 128; x++)
                {
                    midiPlaybackDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, x, 0));
                }
            }
            catch { }

        }


        private void UpdateMidiDevice()
        {
            if (comboMidiDevice.SelectedIndex == -1)
            {
                return;
            }

            ShutDownMidiDevice();

            var itm = comboMidiDevice.SelectedItem as MidiOutputListItem;
            if (itm == null)
                return;

            midiDeviceIndex = itm.index;

            if (!CreateMidiPlaybackDeviceIfNull())
            {
                midiDeviceIndex = 0;
                CreateMidiPlaybackDeviceIfNull();
            }

        }

        private void UpdateMidiInstrument(bool loading)
        {
            if (comboMidiInstrument.SelectedItem == null)
                return;

            try
            {
                var mi = (GeneralMidiInstrument)Enum.Parse(typeof(GeneralMidiInstrument), (string)comboMidiInstrument.SelectedItem);

                ChangeInstrument((int)mi);

                if (!loading)
                {
                    settings.SetValue("MidiDeviceInstrument", (int)mi);
                }
            }
            catch { }
        }
        private void ChangeInstrument(int instrument)
        {
            try
            {
                if (CreateMidiPlaybackDeviceIfNull())
                {
                    midiPlaybackDevice.Send(new ChannelMessage(ChannelCommand.ProgramChange, instrument, 100));
                }
            }
            catch { }
        }

        private void PreviewInstrument(int instrument, int oldInstrument)
        {
            try
            {
                if (CreateMidiPlaybackDeviceIfNull())
                {
                    ChangeInstrument(instrument);

                    midiPlaybackDevice.Send(new ChannelMessage(ChannelCommand.NoteOn, 44, 100));

                    Thread.Sleep(500);

                    midiPlaybackDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, 44, 0));

                    ChangeInstrument(oldInstrument);
                }
            }
            catch { }

        }




        private void PlayHoldBoxMidi()
        {
            try
            {
                if (!CreateMidiPlaybackDeviceIfNull())
                    return;

                var messages = new List<ChannelMessage>();
                var note = Int32.MinValue;
                var hb = GetHoldBoxes();
                for (int x = 0; x < 6; x++)
                {
                    int fret = hb[5 - x].Text.ToInt();
                    if (!fret.IsNull())
                    {
                        
                        note = GetTunedNote(Utility.GetStringLowE(EditorPro.CurrentDifficulty) + x,
                            100 + fret);

                        messages.Add(new ChannelMessage(ChannelCommand.NoteOn, note, 100));
                    }

                }

                foreach (var cm in messages)
                {
                    midiPlaybackDevice.Send(cm);
                }
            }
            catch { }
        }



        private void StopHoldBoxMidi()
        {
            try
            {
                if (CreateMidiPlaybackDeviceIfNull())
                {
                    new ChannelStopper().AllSoundOff();
                }
            }
            catch { }
        }


        void CheckMidiInputVisibility()
        {
            bool vis = checkBoxEnableMidiInput.Checked;
            if (groupBoxMidiInstrument.Tag == null)
            {

                //2,6,12,8
                if (vis == false)
                {
                    groupBoxMidiInstrument.Tag = "hidden";
                    int h = groupBoxMidiInstrument.Height;

                    groupBox36.Visible = vis;
                    groupBoxMidiInstrument.Visible = vis;
                    groupBox37.Visible = vis;

                }
            }
            else
            {
                if (vis == true)
                {
                    groupBoxMidiInstrument.Tag = null;
                    int h = groupBoxMidiInstrument.Height;

                    groupBox36.Visible = vis;
                    groupBoxMidiInstrument.Visible = vis;
                    groupBox37.Visible = vis;

                }
            }
        }


        private bool RefreshMidiOutputList()
        {
            try
            {
                comboMidiDevice.BeginUpdate();
                comboMidiDevice.Items.Clear();
                try
                {
                    for (int i = 0; i < OutputDevice.DeviceCount; i++)
                    {
                        try
                        {
                            var caps = OutputDevice.GetDeviceCapabilities(i);
                            if (caps.voices > 0 || caps.notes > 0)
                            {
                                comboMidiDevice.Items.Add(new MidiOutputListItem() { index = i, Caps = caps });
                            }
                        }
                        catch { }
                    }

                    midiDeviceIndex = settings.GetValueInt("MidiDeviceIndex", 0);

                    var seldev = settings.GetValue("MidiDeviceDesc");
                    if (!string.IsNullOrEmpty(seldev))
                    {
                        try
                        {
                            for (int i = 0; i < OutputDevice.DeviceCount; i++)
                            {
                                var itm = (comboMidiDevice.Items[i] as MidiOutputListItem);
                                if (itm.ToString() == seldev)
                                {
                                    comboMidiDevice.SelectedIndex = i;
                                    midiDeviceIndex = itm.index;
                                    break;
                                }
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        if (midiDeviceIndex < comboMidiDevice.Items.Count && midiDeviceIndex >= 0)
                        {
                            comboMidiDevice.SelectedIndex = midiDeviceIndex;
                        }
                    }
                }
                catch { }

                comboMidiDevice.EndUpdate();
            }
            catch { }
            return comboMidiDevice.Items.Count > 0;
        }
    }
}
