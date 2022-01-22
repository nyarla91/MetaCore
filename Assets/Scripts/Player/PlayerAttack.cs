using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Enemies;
using NyarlaEssentials;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : PlayerComponent
    {
        [Header("General")]
        [SerializeField] private float _shortCooldown;
        [SerializeField] private float _longCooldown;
        [SerializeField] private int _attacksInARow;
        [Header("Damage")]
        [SerializeField] private float _damage;
        [SerializeField] private float _cleaveRadius;
        [Header("Thrust")]
        [SerializeField] private float _thrustDrag;
        [SerializeField] private float _thrustForce;
        [SerializeField] [Range(0, 0.3f)] private float _thrustFinishThreshold;
        [SerializeField] [Range(0, 1)] private float _enemyThrustModifier;

        private int _attacksLeft;
        private bool _attackReady;

        private float _attackReadyCooldownLeft;
        private float _attacksRestoreCooldownLeft;
        
        private void Awake()
        {
            Input.OnAttack += StartAttacking;
        }

        private void FixedUpdate()
        {
            _attacksRestoreCooldownLeft -= Time.fixedDeltaTime;
            if (_attacksRestoreCooldownLeft <= 0)
                _attacksLeft = _attacksInARow;
            
            _attackReadyCooldownLeft -= Time.fixedDeltaTime;
            if (_attackReadyCooldownLeft <= 0)
                _attackReady = true;
        }

        private void StartAttacking()
        {
            if (_attacksLeft == 0 || !_attackReady)
                return;
            
            StopAllCoroutines();
            StartCoroutine(Attacking());
        }

        private IEnumerator Attacking()
        {
            _attackReady = false;
            _attackReadyCooldownLeft = _shortCooldown;
            _attacksLeft--;
            _attacksRestoreCooldownLeft = _longCooldown;

            Movement.Freeze();
            Vector3 direction = Input.RelativeAimVector;

            DealDamageInTheArea(direction);
        
            for (float i = 1; i > _thrustFinishThreshold; i = Mathf.Lerp(i, 0, Time.fixedDeltaTime * _thrustDrag))
            {
                Vector3 offset = direction * _thrustForce * i * Time.fixedDeltaTime;
                Rigidbody.position += offset;
                yield return new WaitForFixedUpdate();
            }
            Movement.Unfreeze();
        }

        private void DealDamageInTheArea(Vector3 direction)
        {
            Vector3 sphereCenter = Rigidbody.position + direction * _cleaveRadius;
            Collider[] colliders= Physics.OverlapSphere(sphereCenter.WithY(0.5f),
                _cleaveRadius, LayerMask.GetMask("Enemy"));

            List<EnemyStatus> targets = new List<EnemyStatus>();
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<EnemyStatus>(out EnemyStatus status))
                {
                    targets.Add(status);
                    status.Stun(_shortCooldown * 1.2f);
                    status.TakeDamage(_damage, true);
                    const float PushModifier = 0.6f;
                    status.Specie.Movement.Push(direction, _thrustForce * PushModifier, _thrustDrag * PushModifier);
                }
            }
        }
    }
}