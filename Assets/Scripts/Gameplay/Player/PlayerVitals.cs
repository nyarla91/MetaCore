using System;
using Gameplay.World;
using Player;
using Project;
using UnityEngine;
using World;

namespace Gameplay.Player
{
    public class PlayerVitals : PlayerComponent
    {
        [SerializeField] private float _damageImmunityDuration;
        [SerializeField] private float _totalHealth;

        private float _maxHealth;
        private float _health;
        private float _damageImmunityTimeLeft;
        public float ImmortaliityTimeLeft { get; set; }

        public float TotalHealth
        {
            get => _totalHealth;
            private set
            {
                _totalHealth = value;
                ValidateHealth();
                OnHealthChangedInvoke();
            }
        }

        public float MaxHealth
        {
            get => _maxHealth;
            private set
            {
                _maxHealth = value;
                ValidateHealth();
                OnHealthChangedInvoke();
            }
        }

        public float Health
        {
            get => _health;
            private set
            {
                _health = Mathf.Min(value, _totalHealth);
                ValidateHealth();
                OnHealthChangedInvoke();

                if (_health < 0 && !IsDead)
                {
                    IsDead = true;
                    OnDeath?.Invoke();
                }
            }
        }
        
        public event Action<float, float, float> OnHealthChanged;
        
        public bool IsInCombat { get; set; }
        public bool IsDead { get; private set; }
        public event Action OnDeath;

        public void TakeDamage(float damage, float maxHealthDamageModifier)
        {
            if (_damageImmunityTimeLeft > 0 || ImmortaliityTimeLeft > 0)
                return;

            maxHealthDamageModifier = Mathf.Min(1, maxHealthDamageModifier);
            Health -= damage;
            MaxHealth -= damage * maxHealthDamageModifier;
            _damageImmunityTimeLeft = _damageImmunityDuration;
        }

        public void RestoreHealth(float healthRestored)
        {
            Health += healthRestored;
        }

        public void StoreHealthToProgression() => Progression.Health = Health;

        private void ValidateHealth()
        {
            if (_maxHealth > _totalHealth)
                _maxHealth = _totalHealth;
            if (_health > _maxHealth)
                _health = _maxHealth;
        }
        
        private void OnHealthChangedInvoke()
        {
            OnHealthChanged?.Invoke(_health, _maxHealth, _totalHealth);
        }

        private void Awake()
        {
            OnDeath += () =>
            {
                Music.Instance.TargetVolume = 0.1f;
            };
        }

        private void Start()
        {
            MaxHealth = TotalHealth;
            Health = MaxHealth;
        }

        private void FixedUpdate()
        {
            _damageImmunityTimeLeft -= Time.fixedDeltaTime;
            ImmortaliityTimeLeft -= Time.fixedDeltaTime;
        }
    }
}