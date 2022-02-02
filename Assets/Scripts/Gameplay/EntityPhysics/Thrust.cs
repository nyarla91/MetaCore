using System;
using UnityEngine;

namespace Gameplay.EntityPhysics
{
    
    [RequireComponent(typeof(Rigidbody))]
    public class Thrust : MonoBehaviour
    {
        public const float Drag = 5;

        private Rigidbody _rigidbody;
        
        public Vector3 Force { get; set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _rigidbody.position += Force * Time.fixedDeltaTime;
            Force = Vector3.Lerp(Force, Vector3.zero, Time.fixedDeltaTime * Drag);
        }
    }
}