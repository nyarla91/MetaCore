using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Enemies.Species;
using Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Enemies.Species
{
    public class EnemyCharger : EnemySpecie
    {
        private static List<EnemyCharger> _chargers = new List<EnemyCharger>();
        
        [Header("General")]
        [SerializeField] private float _farSpeed;
        [SerializeField] private float _closeSpeed;
        [SerializeField] private Material _regularMaterial;
        [SerializeField] private Material _chargingMaterial;
        [Header("Charge")]
        [SerializeField] private float _chargeMinCooldown; 
        [SerializeField] private float _chargeMaxCooldown; 
        [SerializeField] private float _chargeActivationDistance;
        [SerializeField] private float _chargeDelay;
        [SerializeField] private float _chargeDistance;
        [SerializeField] private float _chargeSpeed;

        [Inject]
        private PlayerMarker _playerMarker;
        
        [Inject]
        private void Construct(PlayerMarker playerMarker)
        {
            Movement.CurrentFacingType = EnemyMovement.FacingType.Movement;
            _playerMarker = playerMarker;
            Movement.Destination = playerMarker.transform;
        }

        private void Awake()
        {
            ReloadCharge();
            _chargers.Add(this);
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(transform.position, _playerMarker.transform.position) < _chargeActivationDistance)
            {
                Movement.MaxSpeed = _closeSpeed;
            }
            else
            {
                Movement.MaxSpeed = _farSpeed;
            }
        }

        private IEnumerator Charge()
        {
            Movement.CurrentFacingType = EnemyMovement.FacingType.Destination;
            yield return new WaitForSeconds(0.1f);
            Movement.CurrentFacingType = EnemyMovement.FacingType.None;
            
            Status.ForceExitStun();
            Status.CannotBeStunned = true;
            Animator.SetBool("Charge", true);

            Movement.Freeze();
            yield return new WaitForSeconds(_chargeDelay);

            AttackArea.Active = true;
            for (float i = 0; i < _chargeDistance; i += _chargeSpeed * Time.fixedDeltaTime)
            {
                Vector3 direction = Movement.FacingDirection;
                Rigidbody.position += direction * _chargeSpeed * Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            Animator.SetBool("Charge", false);

            Status.CannotBeStunned = false;
            AttackArea.Active = false;
            Movement.CurrentFacingType = EnemyMovement.FacingType.Movement;
            Movement.Unfreeze();
            ReloadCharge();
        }

        private void ReloadCharge() =>
            StartCoroutine(ReloadCharge(Random.Range(_chargeMinCooldown, _chargeMaxCooldown)));

        private IEnumerator ReloadCharge(float duration)
        {
            yield return new WaitForSeconds(duration);
            yield return new WaitUntil(() =>
                Vector3.Distance(_playerMarker.transform.position, transform.position) < _chargeActivationDistance);
            StartCoroutine(Charge());
        }

        private void OnDestroy()
        {
            _chargers.Remove(this);
        }
    }
}
