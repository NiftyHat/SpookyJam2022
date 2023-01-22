using UI.Audio.Data;
using UnityEngine;

namespace UI.Audio
{
    public class UIGuessAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioGuessData _audioGuessData;

        public void PlayStateAudio(Guess guess)
        {
            switch (guess)
            {
                case Guess.No:
                    _audioSource.PlayOneShot(_audioGuessData.No);
                    break;
                case Guess.Yes:
                    _audioSource.PlayOneShot(_audioGuessData.Yes);
                    break;
                    case Guess.None:
                    _audioSource.PlayOneShot(_audioGuessData.None);
                    break;
            }
        }
    }
}