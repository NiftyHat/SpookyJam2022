using UnityEngine;

namespace UI.Audio.Data
{
    [CreateAssetMenu(fileName = "TurnAudioData", menuName = "Audio/TurnAudioData", order = 3)]
    public class TurnAudioData : ScriptableObject
    {
        [SerializeField] private AudioClip _turnEnd;
        [SerializeField] private AudioClip _phaseChange;

        public AudioClip TurnEnd => _turnEnd;
        public AudioClip PhaseChange => _phaseChange;
    }
}