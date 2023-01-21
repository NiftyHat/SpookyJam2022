using System;
using NiftyFramework.Core.Context;
using UI.Audio;

namespace Context
{
    public class AudioPlayerContext : IContext
    {
        public event Action<AmbientAudioPlayer.AudioState> OnAmbientStateChange;
        public event Action<float> OnGuestVolumeChange;
        
        public void Dispose()
        {
        }

        public void SetAmbient(AmbientAudioPlayer.AudioState audioState)
        {
            OnAmbientStateChange?.Invoke(audioState);
        }

        public void SetGuestVolume(float percentage)
        {
            OnGuestVolumeChange?.Invoke(percentage);
        }
    }
}