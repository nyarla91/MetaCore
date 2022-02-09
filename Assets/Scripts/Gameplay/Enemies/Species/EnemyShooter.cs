using System.Collections;
using Enemies;
using NyarlaEssentials;
using Projectiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Enemies.Species
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

        private bool _isShooting;
        
        protected override void Init()
        {
            Movement.CurrentFacingType = EnemyMovement.FacingType.Destination;
            Movement.Destination = Player.transform;
            StartCoroutine(Shooting());
        }

        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
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
                StatusContainer.ExitStun();
                StatusContainer.CanBeAttackStunned = false;
                
                Animator.SetBool("Shoot", true);
                yield return new WaitForSeconds(_shootingDelay);

                for (int i = 0; i < _bulletsPerRound; i++)
                {
                    Vector3 direction = Player.transform.position - transform.position;
                    direction = direction.WithY(0).normalized;
                    EnemyProjectile.CreateProjectile(_bulletPrefab, _bulletOrigin, direction);
                    yield return new WaitForSeconds(_shootingPeriod);
                }
                Animator.SetBool("Shoot", false);
                StatusContainer.CanBeAttackStunned = true;

                _isShooting = false;
                Movement.Unfreeze();
            }
        }

        public override void OnStun()
        {
            
        }

        public override void OnExitStun()
        {
            
        }
    }
}