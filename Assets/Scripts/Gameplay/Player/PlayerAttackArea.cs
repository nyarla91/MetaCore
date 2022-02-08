using System;
using System.Collections.Generic;
using Enemies;
using Gameplay.Enemies;
using Gameplay.EntityPhysics;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerAttackArea : MonoBehaviour
    {
        [SerializeField] private Collider _attackTrigger;
        private Vector3 Force { get; set; }
        private float Damage { get; set; }

        private List<Collider> _affectedEnemies = new List<Collider>();

        public void Activate(float damage, Vector3 force)
        {
            Force = force;
            Damage = damage;
            _affectedEnemies = new List<Collider>();
            _attackTrigger.enabled = true;
            
        }

        public void Deactivate()
        {
            _affectedEnemies = new List<Collider>();
            Force = Vector3.zero;
            Damage = 0;
            _attackTrigger.enabled = false;
        }

        private void Awake()
        {
            Deactivate();
        }

        private void FixedUpdate()
        {
            Force = Vector3.Lerp(Force, Vector3.zero, Time.fixedDeltaTime * Thrust.Drag);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyStatus status) && !_affectedEnemies.Contains(other))
            {
                _affectedEnemies.Add(other);
                status.TakeDamage(Damage, PlayerDamageSource.Melee);
                status.Specie.Thrust.Force = Force;
            }
        }
    }
}