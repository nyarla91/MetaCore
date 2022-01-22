using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NyarlaEssentials.Sound
{
    public class SoundPlayer : MonoBehaviour
    {
        private const float MASTER_VOLUME = 0.1f;
    
        private static SoundPlayer _instance;
    
        [SerializeField] private GameObject _soundPrefab;
        [SerializeField] private List<SoundPair> _soundLibary;

        private Dictionary<string, AudioClip> _soundDictionary = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            _instance = this;
            foreach (var pair in _soundLibary)
            {
                _soundDictionary.Add(pair.Name, pair.Sound);
            }
        }

        public static void Play(string clip, float volume)
        {
            if (_instance._soundDictionary.ContainsKey(clip))
            {
                SoundInstance newInstance =
                    Instantiate(_instance._soundPrefab, CameraProperties.Instance.transform.position, Quaternion.identity)
                        .GetComponent<SoundInstance>();
                newInstance.Play(_instance._soundDictionary[clip], volume * MASTER_VOLUME);
            }
        }
    
    }

    [Serializable]
    public class SoundPair
    {
        [SerializeField] private string _name;
        public string Name => _name;
        [SerializeField] private AudioClip _sound;
        public AudioClip Sound => _sound;
    }
}