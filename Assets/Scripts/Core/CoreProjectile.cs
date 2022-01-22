using System;
using NyarlaEssentials;
using UnityEngine;

namespace Core
{
    public class CoreProjectile : MonoBehaviour
    {
        [SerializeField] private float _startingSpeed;

        private Vector3 _velocity;
        
        private Rigidbody _rigidbody;
        private Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();

        public Action OnCoreDestroy;
        public Action OnReflect;

        public void Init(Vector3 direction)
        {
            _velocity = direction * _startingSpeed;
        }

        public void Collect()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            Rigidbody.velocity = _velocity;
        }

        private void OnCollisionEnter(Collision other)
        {
            Vector3 normal = other.contacts[0].normal.WithY(0).normalized;
            _velocity = Vector3.Reflect(_velocity, normal);
            OnReflect?.Invoke();
        }

        private void OnDestroy() => OnCoreDestroy?.Invoke();
    }
}