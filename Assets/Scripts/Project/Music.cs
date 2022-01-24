using System;
using UnityEngine;

namespace Project
{
    public class Music : MonoBehaviour
    {
        private const float VolumeModifier = 0.2f;
        
        private static Music _instance;
        public static Music Instance => _instance;
        
        [SerializeField] private AudioSource _audioSource;

        private float _previousTargetVolume;
        
        private float _targetVolume = 0.3f;

        public float TargetVolume
        {
            get => _targetVolume;
            set
            {
                if (value.Equals(_targetVolume))
                    return;
                
                _previousTargetVolume = _targetVolume;
                _targetVolume = value;
            }
        }

        public void RevertVolumeToPrevious() => TargetVolume = _previousTargetVolume;

        private void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            _audioSource.volume = Mathf.Lerp(_audioSource.volume, TargetVolume * VolumeModifier, Time.deltaTime * 4);
        }
    }
}