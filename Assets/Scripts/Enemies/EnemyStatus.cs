using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Enemies
{
    public class EnemyStatus : EnemyComponent
    {
        [SerializeField]  private float _maxHealth;
        [SerializeField] private TextMeshPro _healthCounter;

        private bool _isStunned;
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

        public bool IsStunned => _isStunned;
        public bool CannotBeStunned { get; set; }

        public Action<float, bool> OnTakeDamage;
        public Action OnDeath;

        public void TakeDamage(float damage, bool isMelee)
        {
            OnTakeDamage?.Invoke(damage, isMelee);
            Health -= damage;
        }

        public void Stun(float duration)
        {
            if (CannotBeStunned)
                return;
            
            StopAllCoroutines();
            _isStunned = true;
            StartCoroutine(ExitStun(duration));
        }

        public void ForceExitStun()
        {
            StopAllCoroutines();
            _isStunned = false;
        }

        private IEnumerator ExitStun(float duration)
        {
            yield return new WaitForSeconds(duration);
            _isStunned = false;
        }

        private void Awake()
        {
            Health = _maxHealth;
        }

        private void Die()
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}