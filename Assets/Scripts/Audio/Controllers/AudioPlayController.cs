using System;
using UniRx;

namespace Audio
{
    public interface IAudioPlayController : IDisposable
    {
        void Resume();
        void Pause();
        void Stop();
    }
    
    public class NullAudioPlayController : IAudioPlayController
    {
        public void Resume() { }
        public void Pause() { }
        public void Stop() { }
        public void Dispose() { }
    }
    
    public class AudioPlayController : IAudioPlayController
    {
        private AudioSpeaker _speaker;
        private IDisposable _playRoutine;

        public void Initialize(IDisposable playRoutine)
        {
            _playRoutine = playRoutine;
        }
        
        public void Initialize(AudioSpeaker speaker)
        {
            _speaker = speaker;
        }

        public void Pause() => _speaker?.Pause();
        
        public void Resume() => _speaker?.Resume();

        public void Stop()
        {
            if (_playRoutine == null || _speaker == null) return;

            _playRoutine?.Dispose();
            _playRoutine = null;
            Observable.FromCoroutine(() => _speaker.Stop()).Subscribe(_ => { _speaker?.Dispose(); Dispose(); });
        }

        public void Dispose()
        {
            _speaker = null;
            _playRoutine = null;
        }
    }
}