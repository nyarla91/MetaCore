using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Effects.Statuses;
using Gameplay.Enemies;
using Gameplay.Weapon;
using NyarlaEssentials;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerAttack : PlayerComponent
    {
        [Header("General")]
        [SerializeField] private WeaponClass _class;
        [SerializeField] private PlayerAttackArea _attackArea;
        [SerializeField] private float _attackBufferTime;
        [SerializeField] private float _perfectHitTiming;
        
        [Header("Damage")]
        [SerializeField] private float _damage;

        [Header("Highlights")]
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material _regularMaterial;
        [SerializeField] private Material _swingMaterial;
        [SerializeField] private Material _restorationMaterial;

        private int _attacksLeft;
        private Timer _seriesRestorationTimer;
        private Coroutine _attackCoroutine;
        private float _attackBuffer;
        
        private PerfectHitStage _perfectHitStage = PerfectHitStage.None;
        private float _perfectHitTimeLeft;

        public Dictionary<string, PlayerAttackDamageBonus> damageModifiers;

        public event Action OnPerfectHitSucceed;
        public event Action OnPerfectHitFailed;
        public event Action OnPerfectHitWaiting;

        public float PerfectHitTimeLeft => _perfectHitTimeLeft;

        private static readonly int AnimationAttackStage = Animator.StringToHash("AttackState");
        private static readonly int AnimationAttackSpeed = Animator.StringToHash("AttackSpeed");

        public void InterruptAttack()
        {
            if (_attackCoroutine == null)
                return;

            _meshRenderer.material = _regularMaterial;
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
            Movement.Unfreeze();
            Thrust.Force = Vector3.zero;
            Marker.Animator.SetInteger(AnimationAttackStage, 0);
        }

        public void DiscardSeries()
        {
            _perfectHitStage = PerfectHitStage.None;
            _attacksLeft = _class.AttacksCount;
            _seriesRestorationTimer.Reset();
        }

        public void OnEnemyKilled(EnemyStatus enemy)
        {
            
        }
        
        private void Awake()
        {
            _seriesRestorationTimer = new Timer(this, _class.SeriesRestorationTime);
            Controls.OnAttack += AttackPressed;
            Controls.OnAttack += () =>
                StatusContainer.AddStatus(new StatusDamageBonusVsStunned(StatusContainer, 3, 50));
            _seriesRestorationTimer.OnExpired += DiscardSeries;
            _attacksLeft = _class.AttacksCount;
            IEnumerable<int> ints = new List<int>();
        }

        private void FixedUpdate()
        {
            _attackBuffer -= Time.fixedDeltaTime;
            _perfectHitTimeLeft -= Time.fixedDeltaTime;
            if (_perfectHitStage == PerfectHitStage.Waiting && _perfectHitTimeLeft < 0)
            {
                _perfectHitStage = 0;
                _perfectHitStage = PerfectHitStage.Failed;
                OnPerfectHitFailed?.Invoke();
            }

            string log = "";
            foreach (var damageModifier in damageModifiers)
            {
                PlayerAttackDamageBonus bonus = damageModifier.Value;
                log += $"{damageModifier.Key} : {bonus.Percents}, {bonus.Percents}";
            }
            
            if (_attackBuffer > 0)
                StartAttacking();
        }

        private void AttackPressed()
        {
            _attackBuffer = _attackBufferTime;
            if (_perfectHitStage == PerfectHitStage.Waiting)
            {
                bool succeed = _perfectHitTimeLeft < _perfectHitTiming && _perfectHitTimeLeft > 0;
                _perfectHitStage = succeed ? PerfectHitStage.Success : PerfectHitStage.Failed;
                if (succeed)
                {
                    OnPerfectHitSucceed?.Invoke();
                }
                else
                {
                    OnPerfectHitFailed?.Invoke();
                }
            }
        }

        private void StartAttacking()
        {
            if (_attacksLeft == 0  || Movement.Freezed || _attackCoroutine != null)
                return;

            _attackBuffer = 0;
            _attackCoroutine = StartCoroutine(Attacking());
        }

        private IEnumerator Attacking()
        {
            Vector3 direction = Controls.RelativeAimVector;
            WeaponAttack attack = _class.GetAttack(_class.AttacksCount - _attacksLeft);
            int attackNumber = 4 - _attacksLeft;
            print(attack.SwingTime);
            
            _seriesRestorationTimer.Restart();
            Movement.Freeze();
            _meshRenderer.material = _swingMaterial;
            if (_attacksLeft == 2 && _class.Name.Equals("spear"))
            {
                _perfectHitStage = PerfectHitStage.Waiting;
                _perfectHitTimeLeft = attack.RestorationTime + attack.SwingTime;
                _perfectHitTiming = _perfectHitTimeLeft * 0.2f;
                OnPerfectHitWaiting?.Invoke();
            }
            
            Marker.Animator.SetInteger(AnimationAttackStage, 1);
            Marker.Animator.SetFloat(AnimationAttackSpeed, 1 / attack.SwingTime);
            yield return new WaitForSeconds(attack.SwingTime);
            
            _attacksLeft--;
            _meshRenderer.material = _restorationMaterial;
            
            Vector3 thrustForce = direction * attack.ThrustForce;
            Thrust.Force = thrustForce;
            _attackArea.Activate(_damage * attack.DamageModifier, thrustForce * attack.PushModifier);
            
            Marker.Animator.SetInteger(AnimationAttackStage, 2);
            Marker.Animator.SetFloat(AnimationAttackSpeed, 1 / attack.RestorationTime);
            yield return new WaitForSeconds(attack.RestorationTime);
            
            _attackArea.Deactivate();
            InterruptAttack();
        }

        private enum PerfectHitStage
        {
            None,
            Waiting,
            Failed,
            Success
        }
    }

    public class PlayerAttackDamageBonus
    {
        private Func<EnemyStatus, bool> _condition;
        private int _percents;

        public int Percents => _percents;
        public bool CheckCondition(EnemyStatus enemy) => _condition.Invoke(enemy);

        public PlayerAttackDamageBonus(Func<EnemyStatus, bool> condition, int percents)
        {
            _condition = condition;
            _percents = percents;
        }
    }
}