using Data.Audio;
using NiftyScriptableSet;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioChannel
{
    public class AudioChannelDataSet : ScriptableSet<AudioChannelData>
    {
        [SerializeField] private AudioMixer _audioMixer;
    }
}