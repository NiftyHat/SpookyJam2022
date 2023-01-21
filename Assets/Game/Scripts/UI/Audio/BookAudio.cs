using System.Collections.Generic;
using BookCurlPro;
using NiftyFramework.Scripts;
using UnityEngine;

namespace UI.Audio
{
    public class BookAudio : MonoBehaviour
    {
        [SerializeField] private BookPro _book;
        [SerializeField] private List<AudioClip> _flipClipList;
        [SerializeField] private AudioSource _audioSource;

        protected void Start()
        {
            _book.OnFlip.AddListener(HandlePageFlip);
        }

        private void HandlePageFlip()
        {
            if (_audioSource != null)
            {
                AudioClip randomClip = _flipClipList.RandomItem();
                _audioSource.PlayOneShot(randomClip);
            }
        }
    }
}