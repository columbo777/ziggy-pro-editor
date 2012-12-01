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
using ProUpgradeEditor.DataLayer;
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
                            DisconnectMidiDevice();
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
                            if (checkBoxChordStrum.Checked == true)
                            {

                            }
                            else
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

        Sequence midiPlaybackSequence = new Sequence(FileType.Pro);
        Sequencer midiPlaybackSequencer = new Sequencer();
        OutputDevice midiPlaybackDevice = null;
        public bool MidiPlaybackInProgress
        {
            get { return EditorPro.InPlayback; }
            set {  EditorPro.InPlayback = value; }
        }

        public int MidiPlaybackPosition
        {
            get { return midiPlaybackSequencer == null ? 0 : midiPlaybackSequencer.Position; }
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
                    catch 
                    { 
                        
                    }
                }

            }
            catch { }
        }

        void ShutDownMidiDevice()
        {
            try
            {
                timerMidiPlayback.Enabled = false;
                StopMidiPlayback();
            }
            catch{}
            try{
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

        void CreateMidiPlaybackDeviceIfNull()
        {
            try
            {
                if (midiPlaybackDevice == null)
                {
                    midiPlaybackDevice = new OutputDevice(midiDeviceIndex, SynchronizationContext.Current);
                }
            }
            catch { }
        }

        class naudioPlayer
        {
            public string FileName { get; set; }
            WaveStream mainOutputStream;
            WaveChannel32 volumeStream;

            IWavePlayer waveOutDevice;

            public bool LoadedMP3File
            {
                get { return mainOutputStream != null && volumeStream != null && waveOutDevice != null; }
            }

            public bool PlayingMP3File
            {
                get { return mainOutputStream != null && waveOutDevice.PlaybackState == PlaybackState.Playing; }
            }

            float volume = 1.0f;
            public int Volume 
            {
                get 
                { 
                    return (int)(volume * 100.0f); 
                }
                set
                {
                    volume = (float)value / 100.0f;

                    applyVolume();
                }
            }

            private void applyVolume()
            {
                if (volumeStream != null)
                {
                    volumeStream.Volume = volume;
                }
            }

            public static WaveStream LoadMP3(string fileName)
            {
                WaveStream ret = null;
                try
                {
                    ret = new Mp3FileReader(fileName);
                }
                catch { }
                return ret;
            }

            public void Load(string fileName, IntPtr handle)
            {
                this.FileName = fileName;

                try
                {
                    if (mainOutputStream != null)
                    {
                        Stop();
                        CloseCurrentFile();
                    }

                    if (waveOutDevice == null)
                    {
                        waveOutDevice = new WaveOut(handle);
                    }

                    mainOutputStream = new Mp3FileReader(fileName);
                    volumeStream = new WaveChannel32(mainOutputStream);
                    mainOutputStream = volumeStream;

                    waveOutDevice.Init(volumeStream);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("LoadMP3 " + ex.Message);

                    CloseCurrentFile();
                }
            }

            void foo()
            {
                
            }

            public void Play(double startTime)
            {
                try
                {
                    if (waveOutDevice != null)
                    {
                        mainOutputStream.CurrentTime = TimeSpan.FromSeconds(startTime);
                        applyVolume();
                        if (waveOutDevice.PlaybackState != PlaybackState.Playing)
                        {
                            waveOutDevice.Play();
                        }
                    }
                }
                catch { }
            }

            public void Stop()
            {
                try
                {
                    if (waveOutDevice != null)
                    {
                        if (waveOutDevice.PlaybackState == PlaybackState.Playing ||
                            waveOutDevice.PlaybackState == PlaybackState.Paused)
                        {
                            waveOutDevice.Stop();
                        }
                    }
                }
                catch { }
            }

            public void Cleanup()
            {
                Stop();
                
                if (mainOutputStream != null)
                {
                    CloseCurrentFile();
                }
                if (waveOutDevice != null)
                {
                    try
                    {
                        waveOutDevice.Dispose();
                    }
                    finally
                    {
                        waveOutDevice = null;
                    }
                }
            }

            private void CloseCurrentFile()
            {
                try
                {
                    // this one really closes the file and ACM conversion
                    volumeStream.Close();
                    volumeStream.Dispose();
                }
                finally { volumeStream = null; }
                try
                {
                    // this one does the metering stream
                    mainOutputStream.Close();
                    mainOutputStream.Dispose();
                }
                finally { mainOutputStream = null; }
            }
        }
        
        naudioPlayer mp3Player = new naudioPlayer();
        

        bool playbackSequencerAttached = false;
        
        void PlayMidiFromSelection()
        {
            try
            {
                if (EditorPro.Sequence == null)
                    return;
                
                if (midiPlaybackSequencer == null)
                {
                    midiPlaybackSequencer = new Sequencer();
                }
                CreateMidiPlaybackDeviceIfNull();
                

                if (playbackSequencerAttached == false)
                {
                    this.midiPlaybackSequencer.PlayingCompleted += new EventHandler(midiPlaybackSequencer_PlayingCompleted);
                    this.midiPlaybackSequencer.ChannelMessagePlayed += new EventHandler<ChannelMessageEventArgs>(midiPlaybackSequencer_ChannelMessagePlayed);
                    
                    this.midiPlaybackSequencer.SysExMessagePlayed += new EventHandler<SysExMessageEventArgs>(midiPlaybackSequencer_SysExMessagePlayed);
                    this.midiPlaybackSequencer.Chased += new EventHandler<ChasedEventArgs>(midiPlaybackSequencer_Chased);
                    playbackSequencerAttached = true;
                }




                if (MidiPlaybackInProgress == true)
                {
                    midiPlaybackSequencer.Stop();
                    MidiPlaybackInProgress = false;

                }




                midiPlaybackSequence = new Sequence(FileType.Pro, EditorPro.Sequence.Division);
                midiPlaybackSequence.Format = EditorPro.Sequence.Format;



                midiPlaybackSequencer.Sequence = midiPlaybackSequence;



                int pos = 0;
                if (SelectedChord != null)
                {
                    pos = SelectedChord.DownTick;
                }

                var mainTrack = EditorPro.GuitarTrack.GetTrack();
                if (mainTrack != null)
                {
                    
                    midiPlaybackSequence.AddTempo(EditorPro.GuitarTrack.FindTempoTrack());
                    Track t = new Track(FileType.Pro, mainTrack.Name);

                    var cb = new ChannelMessageBuilder();
                    foreach (var gc in EditorPro.GuitarTrack.Messages.Chords)
                    {
                        foreach(var cn in gc.Notes)
                        {
                            var ut = gc.UpTick - 1 - Utility.ScollToSelectionOffset;
                            var dt = gc.DownTick - Utility.ScollToSelectionOffset;
                            if (ut < dt)
                            {
                                dt = ut - 1;
                            }

                            cb.Command = ChannelCommand.NoteOn;
                            cb.Data1 = cn.Data1;
                            cb.Data2 = cn.Data2;

                            cb.MidiChannel = 0;
                            cb.Build();

                            t.Insert(dt, cb.Result);

                            cb.Command = ChannelCommand.NoteOff;
                            
                            cb.Build();
                            t.Insert(ut, cb.Result);
                        }
                    }
                    midiPlaybackSequence.Add(t);
                }

                if (SelectedSong != null)
                {
                    if (SelectedSong.EnableSongMP3Playback &&
                        File.Exists(SelectedSong.SongMP3Location))
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
            catch {
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
        double currentMp3PlaybackTime=0;
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
                    
                    currentMp3PlaybackTime = EditorPro.GuitarTrack.TickToTime( MidiPlaybackPosition);

                    var d = currentMp3PlaybackTime + waitSecs;

                    if (d >= 0)
                    {
                        currentMp3PlaybackMidiStartTime = d;

                        mp3Player.Play(currentMp3PlaybackMidiStartTime);

                        started = true;
                    }
                }
            }
            catch {  }
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

                if (Utility.GetDifficulty(e.Message.Data1, true) == EditorPro.CurrentDifficulty)
                {
                    e.Message.ConvertToPro();
                    if (Utility.GetStringDifficulty6(e.Message.Data1) == EditorPro.CurrentDifficulty)
                    {
                        if (e.Message.MidiChannel == Utility.ChannelArpeggio)
                            return;


                        var cb = new ChannelMessageBuilder(e.Message);

                        //E3/68 - B2/63 - G2/59 - D2/54 - A1/49 - E1/44
                        var ns = Utility.GetNoteString(e.Message.Data1);
                        var note = 0;
                        switch (ns)
                        {
                            case 0: note = 44; break;
                            case 1: note = 49; break;
                            case 2: note = 54; break;
                            case 3: note = 59; break;
                            case 4: note = 63; break;
                            case 5: note = 68; break;
                        }
                        var data2 = e.Message.Data2;
                       
                        {
                            var outNote = note + (data2 - 100);

                            int tunedNote = outNote;

                            var sci = SelectedSong;
                            if (sci != null)
                            {
                                int tuning = sci.GuitarTuning[ns].ToInt();
                                if (!tuning.IsNull())
                                {
                                    tunedNote += tuning;
                                }
                            }
                            cb.MidiChannel = 0;

                            if (e.Message.Command == ChannelCommand.NoteOn)
                            {
                                if (midiPlaybackEnabled)
                                {
                                    cb.Data2 = midiPlaybackDeviceVolume;
                                }
                                else
                                {
                                    cb.Data2 = 0;
                                }
                                cb.Data1 = tunedNote;
                                cb.Command = ChannelCommand.NoteOn;
                            }
                            else
                            {
                                cb.Data2 = 0;
                                cb.Data1 = tunedNote;
                                cb.Command = ChannelCommand.NoteOff;
                            }
                            
                            cb.Build();
                            var res = cb.Result;
                            
                            midiPlaybackDevice.Send(cb.Result);

                        }
                    }
                }
            }
            catch { }
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

        void midiPlaybackSequencer_Chased(object sender, ChasedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    foreach (ChannelMessage message in e.Messages)
                    {
                        midiPlaybackDevice.Send(message);
                    }
                }));
            }
            else
            {
                foreach (ChannelMessage message in e.Messages)
                {
                    midiPlaybackDevice.Send(message);
                }
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

                                var cs = new ChannelStopper();
                                cs.AllSoundOff();
                            }));
                        }
                        else
                        {
                            midiPlaybackSequencer.Stop();

                            var cs = new ChannelStopper();
                            cs.AllSoundOff();
                        }

                        
                    }
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

            
            try
            {
                ShutDownMidiDevice();
            }
            catch { }

            var itm = comboMidiDevice.SelectedItem as MidiOutputListItem;
            if (itm == null)
                return;

            midiDeviceIndex = itm.index;
            try
            {
                CreateMidiPlaybackDeviceIfNull();
            }
            catch
            {
                try
                {
                    midiDeviceIndex = 0;
                    CreateMidiPlaybackDeviceIfNull();
                }
                catch { }
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
                CreateMidiPlaybackDeviceIfNull();

                var cb = new ChannelMessageBuilder();

                cb.Command = ChannelCommand.ProgramChange;
                cb.MidiChannel = 0;
                cb.Data1 = instrument;
                cb.Data2 = 100;
                cb.Build();
                midiPlaybackDevice.Send(cb.Result);
            }
            catch { }
        }

        private void PreviewInstrument(int instrument, int oldInstrument)
        {
            try
            {
                CreateMidiPlaybackDeviceIfNull();

                var cb = new ChannelMessageBuilder();

                cb.Command = ChannelCommand.ProgramChange;
                cb.MidiChannel = 0;
                cb.Data1 = instrument;
                cb.Data2 = 100;
                cb.Build();
                midiPlaybackDevice.Send(cb.Result);

                cb.Command = ChannelCommand.NoteOn;
                cb.MidiChannel = 0;
                cb.Data1 = 44;
                cb.Data2 = 100;
                cb.Build();
                midiPlaybackDevice.Send(cb.Result);


                Thread.Sleep(500);

                cb.Command = ChannelCommand.NoteOff;
                cb.Build();
                midiPlaybackDevice.Send(cb.Result);


                cb.Command = ChannelCommand.ProgramChange;
                cb.MidiChannel = 0;
                cb.Data1 = oldInstrument;
                cb.Data2 = 100;
                cb.Build();
                midiPlaybackDevice.Send(cb.Result);
            }
            catch { }

        }




        private void PlayHoldBoxMidi()
        {
            try
            {
                CreateMidiPlaybackDeviceIfNull();

                var cb = new ChannelMessageBuilder();
                var messages = new List<ChannelMessage>();
                var note = -1;
                var hb = GetHoldBoxes();
                for (int x = 0; x < 6; x++)
                {
                    int fret = hb[5 - x].Text.ToInt();
                    if (!fret.IsNull())
                    {

                        switch (x)
                        {
                            case 0: note = 44; break;
                            case 1: note = 49; break;
                            case 2: note = 54; break;
                            case 3: note = 59; break;
                            case 4: note = 63; break;
                            case 5: note = 68; break;
                        }
                        note += fret;

                        var sci = SelectedSong;
                        if (sci != null)
                        {
                            int tuning = sci.GuitarTuning[x].ToInt();
                            if (!tuning.IsNull())
                            {
                                note += tuning;
                            }
                        }

                        cb.Command = ChannelCommand.NoteOn;
                        cb.MidiChannel = 0;
                        cb.Data1 = note;
                        cb.Data2 = 100;
                        cb.Build();
                        messages.Add(cb.Result);
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
                CreateMidiPlaybackDeviceIfNull();

                var cs = new ChannelStopper();
                cs.AllSoundOff();
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


        private void RefreshMidiOutputList()
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
    }
}
