using System;
using UnityEngine;
using World;

namespace Player
{
    public class PlayerMarker : PlayerComponent
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _cameraCenter;
        [SerializeField] [Range(0, 1)] private float _cameraT;
        public Transform CameraCenter => _cameraCenter;
        
        public Room CurrentRoom { get; set; }
        public Animator Animator => _animator;

        private void Awake()
        {
            _cameraCenter.parent = null;
        }

        private void Update()
        {
            if (CurrentRoom == null)
                _cameraCenter.transform.position = transform.position;
            else
                _cameraCenter.transform.position = Vector3.Lerp(transform.position, CurrentRoom.transform.position, _cameraT);
        }
    }
}