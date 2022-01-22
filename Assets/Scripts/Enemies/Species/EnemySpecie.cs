using NyarlaEssentials;
using UnityEngine;

namespace Enemies.Species
{
    [RequireComponent(typeof(EnemyMovement))]
    public class EnemySpecie : Transformer
    {
        [SerializeField] private EnemyAttackArea _attackArea;
        private EnemyMovement _movement;
        private EnemyStatus _status;
        private Rigidbody _rigidbody;
        private EnemyUI _ui;

        public EnemyMovement Movement => _movement ??= GetComponent<EnemyMovement>();
        public EnemyStatus Status => _status ??= GetComponent<EnemyStatus>();
        public Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();
        public EnemyAttackArea AttackArea => _attackArea;

        public EnemyUI UI => _ui ??= GetComponent<EnemyUI>();
    }
}