using System.Collections;
using Enemies;
using Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Enemies.Species
{
    public class EnemySpinner : EnemySpecie
    {
        private PlayerMarker _playerMarker;

        [SerializeField] private float _spinningMinCooldown;
        [SerializeField] private float _spinningMaxCooldown;
        [SerializeField] private float _spinningDuration;
        [SerializeField] private float _spinningActivationDistance;
        [SerializeField] private float _spinningDelay;
        [SerializeField] private float _cooldownSpeed;
        [SerializeField] private float _noCooldownSpeed;
        [SerializeField] private float _spinningSpeed;
        
        [Inject]
        protected override void Init()
        {
            Movement.CurrentFacingType = EnemyMovement.FacingType.Movement;
            Movement.Destination = Player.transform;
            StartCoroutine(SpinningCycle());
        }

        private IEnumerator SpinningCycle()
        {
            while (true)
            {
                Movement.MaxSpeed = _cooldownSpeed;
                yield return new WaitForSeconds(Random.Range(_spinningMinCooldown, _spinningMaxCooldown));
                
                Movement.MaxSpeed = _noCooldownSpeed;
                
                yield return new WaitUntil(() => Vector3.Distance(transform.position, 
                    Movement.Destination.position) < _spinningActivationDistance);

                Movement.Freeze();
                Animator.SetInteger("SpinStage", 1);
                yield return new WaitForSeconds(_spinningDelay);
                Movement.Unfreeze();
                
                StatusContainer.ExitStun();
                StatusContainer.CanBeAttackStunned = false;
                Animator.SetInteger("SpinStage", 2);
                Movement.MaxSpeed = _spinningSpeed;
                AttackArea.Active = true;
                StatusContainer.CanBeAttackStunned = true;
                
                yield return new WaitForSeconds(_spinningDuration);
                AttackArea.Active = false;
                
                Animator.SetInteger("SpinStage", 0);
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