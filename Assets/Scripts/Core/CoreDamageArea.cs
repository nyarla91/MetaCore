using System;
using Enemies;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Collider))]
    public class CoreDamageArea : MonoBehaviour
    {
        [SerializeField] private float _damagePerSecond;
        
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out EnemyStatus status))
            {
                status.TakeDamage(_damagePerSecond * Time.deltaTime, false);
            }
        }
    }
}