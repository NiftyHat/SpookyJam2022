using UnityEngine;

namespace UI.Audio.Data
{
    [CreateAssetMenu(fileName = "AudioGuessData", menuName = "Audio/AudioGuessData", order = 2)]
    public class AudioGuessData : ScriptableObject
    {
        [SerializeField] private AudioClip _yes;
        [SerializeField] private AudioClip _no;
        [SerializeField] private AudioClip _none;

        public AudioClip Yes => _yes;
        public AudioClip No => _no;
        public AudioClip None => _none;
    }
}