using System.Collections;
using System.Collections.Generic;
using NyarlaEssentials;
using Player;
using Project;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerMovement : PlayerComponent
    {
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _dashDistance;
        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _dashCooldown;
        [SerializeField] private float _dashAnimationModifier;

        private Vector3 _velocity;
        private Dictionary<string, float> _speedModifiers = new Dictionary<string, float>();

        public bool Freezed { get; private set; }
        private bool _dashReady = true;
        private float _dashCurrentAnimationModifier = 1;

        public float MaxSpeed
        {
            get => _maxSpeed;
            private set => _maxSpeed = value;
        }

        public void AddSpeedModifier(string id, float modifier)
        {
            _speedModifiers.Add(id, modifier);
        }

        public void RemoveSpeedModifier(string id)
        {
            if (_speedModifiers.ContainsKey(id))
                _speedModifiers.Remove(id);
        }
        
        public void Freeze()
        {
            Freezed = true;
        }

        public void Unfreeze()
        {
            Freezed = false;
        }

        private void Awake()
        {
            Controls.OnDash += StartDash;
        }

        private void FixedUpdate()
        {
            Move();
            UpdateAnimation();
        }

        private void Move()
        {
            if (Freezed)
            {
                _velocity = Vector3.zero;
                return;
            }

            float speed = _maxSpeed;
            foreach (var speedModifier in _speedModifiers.Values)
            {
                speed *= speedModifier;
            }
            Vector3 targetVelocity = Controls.RelativeMoveVector * speed * Time.deltaTime;
            _velocity = 
                Vector3.Lerp(_velocity, targetVelocity, _acceleration * Time.fixedDeltaTime);

            Rigidbody.position += _velocity;
            
        }

        private void UpdateAnimation()
        {
            Vector2 originDirection = Freezed ? Controls.AimVector : Controls.MoveVector;
            if (originDirection.magnitude > 0)
                transform.rotation = Quaternion.Euler(0, -originDirection.ToDegrees() + 110, 0);

            Marker.Animator.SetBool("Run", !Freezed && Controls.MoveVector.magnitude > 0);
            Marker.Animator.SetFloat("RunSpeed", Movement.MaxSpeed / 3.5f *
                                                 Controls.MoveVector.magnitude * _dashCurrentAnimationModifier);
        }

        private void StartDash()
        {
            if (!_dashReady || Controls.MoveVector.magnitude == 0)
                return;
            
            Attack.InterruptAttack();
            Core.InterruptAiming();
            StartCoroutine(Dash());
        }
        
        private IEnumerator Dash()
        {
            _dashReady = false;
            
            Freeze();
            Vector3 direction = Controls.RelativeMoveVector.normalized;
            _dashCurrentAnimationModifier = _dashAnimationModifier;
            gameObject.layer = Layers.PlayerIFrame;
            for (float i = 0; i < _dashDistance; i += _dashSpeed * Time.fixedDeltaTime)
            {
                Rigidbody.velocity= direction * _dashSpeed;
                yield return new WaitForFixedUpdate();
            }

            EndDash();
            
            yield return new WaitForSeconds(_dashCooldown);
            _dashReady = true;
        }

        private void EndDash()
        {
            _dashCurrentAnimationModifier = 1;
            gameObject.layer = Layers.Player;
            Unfreeze();
        }
    }
}