using System;
using NyarlaEssentials.Sound;
using Project;
using UnityEngine;
using World;

namespace Player
{
    public class PlayerStatus : PlayerComponent
    {
        [SerializeField] private float _damageImmunityDuration;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _maxShields;
        [SerializeField] private float _shieldRestorationRate;

        private float _damageImmunityTimeLeft;
        public float ImmortaliityTimeLeft { get; set; }
        [SerializeField] private float _health;

        public float MaxHealth => _maxHealth;
        public float Health
        {
            get => _health;
            private set
            {
                _health = Mathf.Min(value, _maxHealth);
                OnHealthPercentChanged?.Invoke(Mathf.Max(_health / _maxHealth, 0));

                if (_health < 0 && !IsDead)
                {
                    IsDead = true;
                    OnDeath?.Invoke();
                }
            }
        }
        public Action<float> OnHealthPercentChanged;

        [SerializeField] private float _shields;
        public float Shields
        {
            get => _shields;
            private set
            {
                _shields = Mathf.Min(value, _maxShields);
                OnShieldsPercentChanged?.Invoke(Mathf.Max(_shields / _maxShields, 0));
            }
        }
        public Action<float> OnShieldsPercentChanged;
        
        public bool IsInCombat { get; set; }
        public bool IsDead { get; private set; }
        public Action OnDeath;

        public void TakeDamage(float damage)
        {
            if (_damageImmunityTimeLeft > 0 || ImmortaliityTimeLeft > 0)
                return;

            if (Core.IsCoreOut)
            {
                Health -= damage;
            }
            else
            {
                Shields -= damage;
                if (Shields < 0)
                {
                    Health += Shields;
                    Shields = 0;
                }   
            }
            _damageImmunityTimeLeft = _damageImmunityDuration;
        }

        public void RestoreHealth(float healthRestored)
        {
            Health += healthRestored;
        }

        public void StoreHealthToProgression() => Progression.Health = Health;

        private void Awake()
        {
            _maxHealth *= Progression.FloorDifficultiModifier;
            Health = Progression.Health > 0 ? Progression.Health : _maxHealth;
            Shields = _maxShields;
            OnDeath += () =>
            {
                Music.Instance.TargetVolume = 0.1f;
            };
        }

        private void FixedUpdate()
        {
            _damageImmunityTimeLeft -= Time.fixedDeltaTime;
            ImmortaliityTimeLeft -= Time.fixedDeltaTime;
            if (_damageImmunityTimeLeft <= 0)
                Shields += _shieldRestorationRate * Time.fixedDeltaTime;
        }
    }
}