using System;
using System.Collections;
using System.Timers;
using NyarlaEssentials;
using TMPro;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyMovement : EnemyComponent
    {
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _acceleration;

        private bool _freezed;
        private Vector3 _velocity;
        private Transform _destination;

        public float MaxSpeed
        {
            get => _maxSpeed;
            set => _maxSpeed = value;
        }
        public Transform Destination
        {
            set
            {
                _destination = value;
                _moveDirection = Vector3.zero;
            }
            get
            {
                return _destination;
            }
        }

        private Vector3 _moveDirection;
        public Vector3 MoveDirection
        {
            set
            {
                _moveDirection = value;
                _destination = null;
            }
            get
            {
                return _moveDirection;
            }
        }
        
        public Vector3 FacingDirection { get; private set; }
        public FacingType CurrentFacingType { get; set; }
        
        private bool _fleeFromDestination;

        public bool FleeFromDestination
        {
            get => _fleeFromDestination;
            set
            {
                _fleeFromDestination = value;
                Specie.Animator.SetBool("Flee", value);
            }
        }

        public void Freeze() => _freezed = true;
        public void Unfreeze() => _freezed = false;
        
        private void FixedUpdate()
        {
            Move();
            FaceTarget();
        }

        private void Move()
        {
            Specie.Animator.SetFloat("Speed", _velocity.magnitude);
            if (_freezed || Specie.Status.IsStunned)
            {
                _velocity = Vector3.zero;
                return;
            }

            Vector3 targetDirection;
            if (Destination != null)
            {
                targetDirection = (Destination.position - Specie.Rigidbody.position).normalized;
            }
            else
            {
                targetDirection = MoveDirection;
            }

            _velocity = Vector3.Lerp(_velocity, targetDirection.WithY(0) * _maxSpeed * Time.fixedDeltaTime,
                Time.fixedDeltaTime * _acceleration);
            
            Specie.Rigidbody.position += (FleeFromDestination ? -1 : 1) * _velocity;
        }

        private void FaceTarget()
        {
            if (CurrentFacingType == FacingType.Movement && _velocity.magnitude > 0)
                FacingDirection = _velocity.normalized;
            else if (CurrentFacingType == FacingType.Destination)
                FacingDirection = (Destination.position - transform.position).WithY(0).normalized;

            float z = -FacingDirection.XZtoXY().Rotated(-90).ToDegrees();
            transform.rotation = Quaternion.Euler(0, z, 0);
        }

        public void Push(Vector3 direction, float force, float drag)
        {
            if (Specie.Status.CannotBeStunned)
                return;
            StopAllCoroutines();
            StartCoroutine(Pushing(direction, force, drag));
        }

        private IEnumerator Pushing(Vector3 direction, float force, float drag)
        {
            Specie.Status.Stun(int.MaxValue);
            for (float i = force; i > 0.2f; i = Mathf.Lerp(i, 0, drag * Time.fixedDeltaTime))
            {
                Specie.Rigidbody.position += direction * i * Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            Specie.Status.ForceExitStun();
        }

        public enum FacingType
        {
            None,
            Destination,
            Movement
        }
    }
}