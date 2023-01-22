using UnityEngine;

namespace UI.Audio.Data
{
    [CreateAssetMenu(fileName = "AudioButtonData", menuName = "Audio/AudioButtonData", order = 1)]
    public class AudioButtonData : ScriptableObject
    {
        [SerializeField] private AudioClip _click;

        public AudioClip Click => _click;
    }
}