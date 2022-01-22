using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : PlayerComponent
    {
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _dashDistance;
        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _dashCooldown;

        private Vector3 _velocity;

        private bool _freezed;
        private bool _dashReady = true;
        public void Freeze() => _freezed = true;
        public void Unfreeze() => _freezed = false;

        private void Awake()
        {
            Input.OnDash += StartDash;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (_freezed)
            {
                _velocity = Vector3.zero;
                return;
            }
            
            Vector3 targetVelocity = Input.RelativeMoveVector * _maxSpeed * Time.deltaTime;
            _velocity = 
                Vector3.Lerp(_velocity, targetVelocity, _acceleration * Time.fixedDeltaTime);

            Rigidbody.position += _velocity;
        }

        private void StartDash()
        {
            if (!_dashReady || _freezed || Input.MoveVector.magnitude == 0)
                return;
            
            StartCoroutine(Dash());
        }
        
        private IEnumerator Dash()
        {
            _dashReady = false;
            
            Freeze();
            Vector3 direction = Input.RelativeMoveVector.normalized;
            for (float i = 0; i < _dashDistance; i += _dashSpeed * Time.fixedDeltaTime)
            {
                Rigidbody.position += direction * _dashSpeed * Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            Unfreeze();

            yield return new WaitForSeconds(_dashCooldown);
            _dashReady = true;
        }
    }
}