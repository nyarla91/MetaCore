using System;
using System.Collections.Generic;
using Enemies;
using NyarlaEssentials;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
    [RequireComponent(typeof(Collider))]
    public class CoreDamageArea : Transformer
    {
        [SerializeField] private CoreProjectile _core;
        [SerializeField] private float _damagePerSecond;
        [SerializeField] private float _damageBonus;
        [SerializeField] private float _radiusBonus;
        [SerializeField] private int _bonuses;

        private readonly List<EnemyStatus> _damagedEnemies = new List<EnemyStatus>();
        
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
            _damagePerSecond += _damageBonus;
            transform.localScale += new Vector3(_radiusBonus, 0, _radiusBonus);
        }

        private void Update()
        {
            foreach (var damagedEnemy in _damagedEnemies)
            {
                damagedEnemy?.TakeDamage(_damagePerSecond * Time.deltaTime, false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyStatus status))
            {
                _damagedEnemies.Add(status);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out EnemyStatus status) && _damagedEnemies.Contains(status))
            {
                _damagedEnemies.Remove(status);
            }
        }
    }
}