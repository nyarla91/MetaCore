using System;
using NyarlaEssentials;
using Player;
using UnityEngine;
using Zenject;

namespace View
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private float _followSpeed;
        private Transform _target;
        
        [Inject]
        private void Construct(PlayerMarker player)
        {
            _target = player.transform;
        }

        public static Vector3 Vector2ToRelative(Vector2 vector) => vector.Rotated(-CameraProperties.YRotation).XYtoXZ();
        
        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, _target.position, _followSpeed * Time.deltaTime);
        }
    }
}