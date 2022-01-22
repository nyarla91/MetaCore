using System;
using System.Collections;
using Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemies.Species
{
    public class RegualrEnemy : EnemySpecie
    {
        [Header("General")]
        [SerializeField] private float _farSpeed;
        [SerializeField] private float _closeSpeed;
        [SerializeField] private Material _regularMaterial;
        [SerializeField] private Material _chargingMaterial;
        [SerializeField] private MeshRenderer _meshRenderer;
        [Header("Charge")]
        [SerializeField] private float _chargeMinCooldown; 
        [SerializeField] private float _chargeMaxCooldown; 
        [SerializeField] private float _chargeActivationDistance;
        [SerializeField] private float _chargeDelay;
        [SerializeField] private float _chargeDistance;
        [SerializeField] private float _chargeSpeed;
        [SerializeField] private float _chargeDamage;

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
            
            Status.ForceExitStun();
            Status.CannotBeStunned = true;
            _meshRenderer.material = _chargingMaterial;

            Movement.Freeze();
            yield return new WaitForSeconds(_chargeDelay);

            AttackArea.Active = true;
            for (float i = 0; i < _chargeDistance; i += _chargeSpeed * Time.fixedDeltaTime)
            {
                Vector3 direction = Movement.FacingDirection;
                Rigidbody.position += direction * _chargeSpeed * Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            Status.CannotBeStunned = false;
            AttackArea.Active = false;
            Movement.CurrentFacingType = EnemyMovement.FacingType.Movement;
            Movement.Unfreeze();
            _meshRenderer.material = _regularMaterial;
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
    }
}