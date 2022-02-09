using System;
using System.Collections;
using NyarlaEssentials;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyStatusContainer : EnemyComponent
    {
        private Timer _stunExitTimer;
        
        private bool IsStunAttack { get; set; }
        public bool IsStunned { get; private set; }

        public bool CanBeAttackStunned { get; set; } = true;
        public bool CanBeStunned { get; set; } = true;
        
        public void Stun(bool attack, float duration)
        {
            if (!CanBeStunned || (attack && CanBeAttackStunned) || _stunExitTimer.TimeLeft > duration)
                return;
            
            if (!attack)
                Specie.OnStun();

            IsStunAttack = attack;
            StopAllCoroutines();
            IsStunned = true;
            Specie.Animator.SetBool("Stun", false);
            _stunExitTimer.Length = duration;
            _stunExitTimer.Restart();
        }

        public void ExitStun()
        {
            if (!IsStunned)
                return;
            
            StopAllCoroutines();
            IsStunned = false;
            if (!IsStunAttack)
                Specie.OnExitStun();
            Specie.Animator.SetBool("Stun", IsStunned);
        }

        private void Awake()
        {
            _stunExitTimer = new Timer(this, 0);
            _stunExitTimer.OnExpired += ExitStun;
        }
    }
}