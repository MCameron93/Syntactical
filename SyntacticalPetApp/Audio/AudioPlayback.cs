using NAudio.Extras;
using NAudio.Wave;
using System;
using System.Windows;

namespace SyntacticalPetApp
{
    public class AudioPlayback : IDisposable
    {
        private WaveStream fileStream;
        private IWavePlayer playbackDevice;

        public event EventHandler<FftEventArgs> FftCalculated;

        public event EventHandler<MaxSampleEventArgs> MaximumCalculated;

        public float Volume
        { 
            get => playbackDevice.Volume; 
            set => playbackDevice.Volume = value;
        }

        public void Dispose()
        {
            Stop();
            CloseFile();
            playbackDevice?.Dispose();
            playbackDevice = null;
        }

        public void Load(string fileName)
        {
            Stop();
            CloseFile();
            EnsureDeviceCreated();
            OpenFile(fileName);
        }

        public void Pause()
        {
            playbackDevice?.Pause();
        }

        public void Play()
        {
            if (playbackDevice != null && fileStream != null && playbackDevice.PlaybackState != PlaybackState.Playing)
            {
                playbackDevice.Play();
            }
        }

        public void Stop()
        {
            playbackDevice?.Stop();
            if (fileStream != null)
            {
                fileStream.Position = 0;
            }
        }

        private void CloseFile()
        {
            fileStream?.Dispose();
            fileStream = null;
        }

        private void CreateDevice()
        {
            playbackDevice = new WaveOut { DesiredLatency = 200 };
        }

        private void EnsureDeviceCreated()
        {
            if (playbackDevice == null)
            {
                CreateDevice();
            }
        }

        private void OpenFile(string fileName)
        {
            try
            {
                var inputStream = new AudioFileReader(fileName);
                fileStream = inputStream;
                var aggregator = new SampleAggregator(inputStream);
                aggregator.NotificationCount = inputStream.WaveFormat.SampleRate / 100;
                aggregator.PerformFFT = true;
                aggregator.FftCalculated += (s, a) => FftCalculated?.Invoke(this, a);
                aggregator.MaximumCalculated += (s, a) => MaximumCalculated?.Invoke(this, a);
                playbackDevice.Init(aggregator);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Problem opening file");
                CloseFile();
            }
        }
    }
}