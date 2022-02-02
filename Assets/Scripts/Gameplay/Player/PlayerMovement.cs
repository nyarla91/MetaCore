using System.Collections;
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

        public bool Freezed { get; private set; }
        private bool _dashReady = true;
        private float _dashCurrentAnimationModifier = 1;

        public float MaxSpeed
        {
            get => _maxSpeed;
            set => _maxSpeed = value;
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
            Input.OnDash += StartDash;
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
            
            Vector3 targetVelocity = Input.RelativeMoveVector * _maxSpeed * Time.deltaTime;
            _velocity = 
                Vector3.Lerp(_velocity, targetVelocity, _acceleration * Time.fixedDeltaTime);

            Rigidbody.position += _velocity;
            
        }

        private void UpdateAnimation()
        {
            Vector2 originDirection = Freezed ? Input.AimVector : Input.MoveVector;
            if (originDirection.magnitude > 0)
                transform.rotation = Quaternion.Euler(0, -originDirection.ToDegrees() + 110, 0);

            Marker.Animator.SetBool("Run", !Freezed && Input.MoveVector.magnitude > 0);
            Marker.Animator.SetFloat("RunSpeed", Movement.MaxSpeed / 3.5f *
                                                 Input.MoveVector.magnitude * _dashCurrentAnimationModifier);
        }

        private void StartDash()
        {
            if (!_dashReady || Input.MoveVector.magnitude == 0)
                return;
            
            Attack.InterruptAttack();
            Core.InterruptAiming();
            StartCoroutine(Dash());
        }
        
        private IEnumerator Dash()
        {
            _dashReady = false;
            
            Freeze();
            Vector3 direction = Input.RelativeMoveVector.normalized;
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