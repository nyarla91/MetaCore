using System;
using System.Collections;
using NyarlaEssentials;
using Player;
using Projectiles;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemies.Species
{
    public class EnemyShooter : EnemySpecie
    {
        [Header("Shoot")]
        [SerializeField] private Transform _bulletOrigin;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private int _bulletsPerRound;
        [SerializeField] private float _shootingDelay;
        [SerializeField] private float _shootingPeriod;
        [SerializeField] private float _minCooldown;
        [SerializeField] private float _maxCooldown;
        [Header("Walk")]
        [SerializeField] private float _maxApproachDistance;
        [SerializeField] private float _minFleeDistance;
        [SerializeField] private float _fleeSpeed;
        [SerializeField] private float _approachSpeed;

        private PlayerMarker _playerMarker;
        private bool _isShooting;
        
        [Inject]
        private void Construct(PlayerMarker playerMarker)
        {
            Movement.CurrentFacingType = EnemyMovement.FacingType.Destination;
            _playerMarker = playerMarker;
            Movement.Destination = playerMarker.transform;
            StartCoroutine(Shooting());
        }

        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(_playerMarker.transform.position, transform.position);
            if (!_isShooting && distanceToPlayer > _maxApproachDistance)
            {
                Movement.Unfreeze();
                Movement.MaxSpeed = _approachSpeed;
                Movement.FleeFromDestination = false;
            }
            else if (!_isShooting && distanceToPlayer < _minFleeDistance)
            {
                Movement.Unfreeze();
                Movement.MaxSpeed = _fleeSpeed;
                Movement.FleeFromDestination = true;
            }
            else
            {
                Movement.Freeze();
            }
        }

        private IEnumerator Shooting()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(_minCooldown, _maxCooldown));
                
                _isShooting = true;
                Movement.Freeze();
                Status.ForceExitStun();
                Status.CannotBeStunned = true;
                
                Animator.SetBool("Shoot", true);
                yield return new WaitForSeconds(_shootingDelay);

                for (int i = 0; i < _bulletsPerRound; i++)
                {
                    Vector3 direction = _playerMarker.transform.position - transform.position;
                    direction = direction.WithY(0).normalized;
                    EnemyProjectile.CreateProjectile(_bulletPrefab, _bulletOrigin, direction);
                    yield return new WaitForSeconds(_shootingPeriod);
                }
                Animator.SetBool("Shoot", false);
                Status.CannotBeStunned = false;

                _isShooting = false;
                Movement.Unfreeze();
            }
        }
    }
}