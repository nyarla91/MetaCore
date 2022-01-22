using System;
using NyarlaEssentials;
using Player;
using UnityEngine;
using Zenject;

namespace View
{
    public class CameraView : Transformer
    {
        [SerializeField] private float _followSpeed;
        private Transform _target;
        
        [Inject]
        private void Construct(PlayerMarker player)
        {
            _target = player.CameraCenter;
        }

        public static Vector3 Vector2ToRelative(Vector2 vector) => vector.Rotated(-CameraProperties.YRotation).XYtoXZ();
        
        private void Update()
        {
            if (_target != null)
            {
                transform.position =
                    Vector3.Lerp(transform.position, _target.position, _followSpeed * Time.deltaTime).WithY(0);
            }
        }
    }
}