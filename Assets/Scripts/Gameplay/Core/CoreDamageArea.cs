using System;
using System.Collections.Generic;
using Enemies;
using Gameplay.Enemies;
using NyarlaEssentials;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
    [RequireComponent(typeof(Collider))]
    public class CoreDamageArea : Transformer
    {
        [SerializeField] private CoreProjectile _core;
        [SerializeField] private float _damage;
        [SerializeField] private float _damageBonus;
        [SerializeField] private int _bonuses;
        
        public void FullyCharge()
        {
            for (int i = 0; i < _bonuses; i++)
            {
                UseBonus();
            }
        }
        
        private void Awake()
        {
            _core.OnReflect += UseBonus;
        }

        private void UseBonus()
        {
            if (_bonuses <= 0)
                return;
            _bonuses--;
            _damage += _damageBonus;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyVitals status))
            {
                status.TakeDamage(_damage, PlayerDamageSource.Core);
            }
        }
    }
}