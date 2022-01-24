using System;
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

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out EnemyStatus status))
            {
                status.TakeDamage(_damagePerSecond * Time.deltaTime, false);
            }
        }
    }
}