using System.Collections;
using System.Collections.Generic;
using Enemies;
using NyarlaEssentials;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Enemies.Species
{
    public class EnemyCharger : EnemySpecie
    {
        private static List<EnemyCharger> _chargers = new List<EnemyCharger>();
        
        [Header("General")]
        [SerializeField] private float _farSpeed;
        [SerializeField] private float _closeSpeed;
        [Header("Charge")]
        [SerializeField] private float _chargeMinCooldown; 
        [SerializeField] private float _chargeMaxCooldown; 
        [SerializeField] private float _chargeActivationDistance;
        [SerializeField] private float _chargeDelay;
        [SerializeField] private float _chargeDistance;
        [SerializeField] private float _chargeSpeed;

        private Coroutine _currentStateCoroutine;
        
        protected override void Init()
        {
            Movement.CurrentFacingType = EnemyMovement.FacingType.Movement;
            Movement.Destination = Player.transform;
        }

        private void Awake()
        {
            ReturnToNormalState();
            StartCooldown();
            _chargers.Add(this);
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < _chargeActivationDistance)
            {
                Movement.MaxSpeed = _closeSpeed;
            }
            else
            {
                Movement.MaxSpeed = _farSpeed;
            }
        }

        private void ReturnToNormalState()
        {
            print("Normal state");
            _currentStateCoroutine?.StopThisCoroutine(this);
            _currentStateCoroutine = null;
            
            Animator.SetBool("Charge", false);
            StatusContainer.CanBeAttackStunned = true;
            AttackArea.Active = false;
            Movement.CurrentFacingType = EnemyMovement.FacingType.Movement;
            Movement.Unfreeze();
        }

        private void StartCooldown() =>
            _currentStateCoroutine = StartCoroutine(StartCooldown(Random.Range(_chargeMinCooldown, _chargeMaxCooldown)));

        private IEnumerator StartCooldown(float duration)
        {
            print("Cooldown");
            yield return new WaitForSeconds(duration);
            yield return new WaitUntil(() =>
                Vector3.Distance(Player.transform.position, transform.position) < _chargeActivationDistance);
            _currentStateCoroutine = StartCoroutine(Swing());
        }

        private IEnumerator Swing()
        {
            print("Swing");
            Movement.CurrentFacingType = EnemyMovement.FacingType.Destination;
            
            StatusContainer.ExitStun();
            StatusContainer.CanBeAttackStunned = false;
            Animator.SetBool("Charge", true);
            Movement.Freeze();
            yield return new WaitForSeconds(_chargeDelay);
            _currentStateCoroutine = StartCoroutine(Charge());
        }

        private IEnumerator Charge()
        {
            print("Charge");
            Movement.CurrentFacingType = EnemyMovement.FacingType.None;
            
            Vector3 direction = Movement.FacingDirection;
            AttackArea.Active = true;
            for (float i = 0; i < _chargeDistance; i += _chargeSpeed * Time.fixedDeltaTime)
            {
                Rigidbody.position += direction * _chargeSpeed * Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            ReturnToNormalState();
            StartCooldown();
        }

        private void OnDestroy()
        {
            _chargers.Remove(this);
        }

        public override void OnStun()
        {
            ReturnToNormalState();
        }

        public override void OnExitStun()
        {
            print("gh");
            StartCooldown();
        }
    }
}
