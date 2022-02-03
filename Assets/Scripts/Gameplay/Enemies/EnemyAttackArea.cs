using System;
using Gameplay.Player;
using Player;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Collider))]
    public class EnemyAttackArea : MonoBehaviour
    {
        [SerializeField] private float _damage;
        
        public bool Active { get; set; }

        private void OnTriggerStay(Collider other)
        {
            if (!Active)
                return;
            
            if (other.TryGetComponent(out PlayerStatus status))
            {
                status.TakeDamage(_damage, 0.3f);
            }
        }
    }
}