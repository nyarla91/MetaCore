﻿using System;
using System.Collections;
using Gameplay.Weapon;
using NyarlaEssentials;
using Player;
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

        public Action OnPerfectHitSucceed;
        public Action OnPerfectHitFailed;
        public Action OnPerfectHitWaiting;

        public float PerfectHitTimeLeft => _perfectHitTimeLeft;

        public void InterruptAttack()
        {
            if (_attackCoroutine == null)
                return;

            _meshRenderer.material = _regularMaterial;
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
            Movement.Unfreeze();
            Thrust.Force = Vector3.zero;
            Marker.Animator.SetInteger("AttackNumber", 0);
        }

        public void DiscardSeries()
        {
            _perfectHitStage = PerfectHitStage.None;
            _attacksLeft = _class.AttacksCount;
            _seriesRestorationTimer.Reset();
        }
        
        private void Awake()
        {
            _seriesRestorationTimer = new Timer(this, _class.SeriesRestorationTime);
            Input.OnAttack += AttackPressed;
            _seriesRestorationTimer.OnExpired += DiscardSeries;
            _attacksLeft = _class.AttacksCount;
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
            Vector3 direction = Input.RelativeAimVector;
            WeaponAttack attack = _class.GetAttack(_class.AttacksCount - _attacksLeft);
            int attackNumber = 4 - _attacksLeft;
            print(attack.SwingTime);
            
            _seriesRestorationTimer.Restart();
            Movement.Freeze();
            _meshRenderer.material = _swingMaterial;
            if (_attacksLeft == 2)
            {
                _perfectHitStage = PerfectHitStage.Waiting;
                _perfectHitTimeLeft = attack.RestorationTime + attack.SwingTime;
                _perfectHitTiming = _perfectHitTimeLeft * 0.2f;
                OnPerfectHitWaiting?.Invoke();
            }
            
            yield return new WaitForSeconds(attack.SwingTime);
            
            _attacksLeft--;
            Marker.Animator.SetInteger("AttackNumber", attackNumber);
            _meshRenderer.material = _restorationMaterial;
            
            Vector3 thrustForce = direction * attack.ThrustForce;
            Thrust.Force = thrustForce;
            _attackArea.Activate(_damage * attack.DamageModifier, thrustForce * attack.PushModifier);
            
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
}