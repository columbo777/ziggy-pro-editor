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
                if (fileName.EndsWithEx("ogg"))
                {
                    ret = new OggFileReader(fileName);
                }
                else
                {
                    ret = new Mp3FileReader(fileName);
                }
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

                if (fileName.EndsWithEx("ogg"))
                {
                    mainOutputStream = new OggFileReader(fileName);
                }
                else
                {
                    mainOutputStream = new Mp3FileReader(fileName);
                }

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
}