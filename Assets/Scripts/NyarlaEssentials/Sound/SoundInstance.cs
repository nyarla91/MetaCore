using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NyarlaEssentials.Sound
{
    public class SoundInstance : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        public void Play(AudioClip clip, float volume)
        {
            _audioSource.clip = clip;
            _audioSource.volume = volume;
            _audioSource.Play();
        }
    }

}