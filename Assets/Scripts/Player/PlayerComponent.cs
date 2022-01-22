using NyarlaEssentials;
using UnityEngine;

namespace Player
{
    public class PlayerComponent : Transformer
    {
        private PlayerInput _input;
        private PlayerMovement _movement;
        private PlayerCore _core;
        private Rigidbody _rigidbody;
        private PlayerAttack _attack;
        private PlayerStatus _status;
        private PlayerUI _ui;
        
        public PlayerInput Input => _input ??= GetComponent<PlayerInput>();
        
        public PlayerMovement Movement => _movement ??= GetComponent<PlayerMovement>();
        public PlayerCore Core => _core ??= GetComponent<PlayerCore>();
        public Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();
        public PlayerAttack Attack => _attack ??= GetComponent<PlayerAttack>();
        public PlayerStatus Status => _status ??= GetComponent<PlayerStatus>();
        public PlayerUI UI => _ui ??= GetComponent<PlayerUI>();
    }
}