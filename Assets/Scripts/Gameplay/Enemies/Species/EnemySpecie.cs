using Enemies;
using Gameplay.EntityPhysics;
using NyarlaEssentials;
using Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies.Species
{
    [RequireComponent(typeof(EnemyMovement))]
    public abstract class EnemySpecie : Transformer
    {
        [SerializeField] private EnemyAttackArea _attackArea;
        [SerializeField] private Material _beaconMaterial;
        [SerializeField] private Animator _animator;
        private EnemyMovement _movement;
        private EnemyVitals _vitals;
        private EnemyStatusContainer _statusContainer;
        private EnemyUI _ui;
        private Rigidbody _rigidbody;
        private Thrust _thrust;
        private PlayerMarker _player;

        [Inject]
        private void Construct(PlayerMarker player)
        {
            _player = player;
            Init();
        }

        protected abstract void Init();
        public abstract void OnStun();
        public abstract void OnExitStun();

        public Material BeaconMaterial => _beaconMaterial;
        public EnemyMovement Movement => _movement ??= GetComponent<EnemyMovement>();
        public EnemyVitals Vitals => _vitals ??= GetComponent<EnemyVitals>();
        public Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();
        public EnemyAttackArea AttackArea => _attackArea;

        public EnemyStatusContainer StatusContainer => _statusContainer ??= GetComponent<EnemyStatusContainer>();
        public EnemyUI UI => _ui ??= GetComponent<EnemyUI>();
        public Animator Animator => _animator;
        public Thrust Thrust => _thrust ??= GetComponent<Thrust>();
        public PlayerMarker Player => _player;
    }
}