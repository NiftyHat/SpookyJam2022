using NiftyFramework.Core.Utils;
using UI.Audio.Data;
using UnityEngine;

namespace UI.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class TurnChangeAudio : MonoBehaviour
    {
        [SerializeField] private TurnAudioData _data;
        [SerializeField][NonNull] private AudioSource _audioSource;

        public void PlayTurnEnd()
        {
            _audioSource.PlayOneShot(_data.TurnEnd);
        }

        public void PlayPhaseChange()
        {
            _audioSource.PlayOneShot(_data.PhaseChange);
        }
    }
}