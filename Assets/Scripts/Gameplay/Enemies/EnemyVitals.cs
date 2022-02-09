using System;
using System.Collections;
using Enemies;
using Gameplay.World;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyVitals : EnemyComponent
    {
        [SerializeField] private GameObject _explosionPrefab;
        [SerializeField]  private float _maxHealth;

        private float _health;
        public float Health
        {
            get => _health;
            private set
            {
                _health = value;
                if (_health < 0)
                    Die();
                OnHealthPercentChanged?.Invoke(Mathf.Max(_health / _maxHealth, 0));
            }
        }

        public Action<float> OnHealthPercentChanged;


        public Action<float, PlayerDamageSource> OnTakeDamage;
        public Action OnDeath;

        public void TakeDamage(float damage, PlayerDamageSource source)
        {
            OnTakeDamage?.Invoke(damage, source);
            Health -= damage;
        }

        private void Awake()
        {
            Health = _maxHealth;
            OnDeath += () =>
            {
            };
        }

        private void Die()
        {
            OnDeath?.Invoke();
            Specie.Player.Attack.OnEnemyKilled(this);
            Progression.Kills++;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity); 
            Destroy(gameObject);
        }
    }

    public enum PlayerDamageSource
    {
        Melee,
        Core
    }
}