using System;
using UnityEngine;

namespace Player
{
    public class PlayerStatus : PlayerComponent
    {
        [SerializeField] private float _damageImmunityDuration;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _maxShields;
        [SerializeField] private float _shieldRestorationRate;

        private float _damageImmunityTimeLeft;
        [SerializeField] private float _health;
        public float Health
        {
            get => _health;
            private set
            {
                _health = Mathf.Min(value, _maxHealth);
                OnHealthPercentChanged?.Invoke(Mathf.Max(_health / _maxHealth, 0));
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

        public void TakeDamage(float damage)
        {
            if (_damageImmunityTimeLeft > 0)
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

        private void Awake()
        {
            Health = _maxHealth;
            Shields = _maxShields;
        }

        private void FixedUpdate()
        {
            _damageImmunityTimeLeft -= Time.fixedDeltaTime;
            if (_damageImmunityTimeLeft <= 0)
                Shields += _shieldRestorationRate * Time.fixedDeltaTime;
        }
    }
}