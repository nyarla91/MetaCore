using Gameplay.EntityPhysics;
using NyarlaEssentials;
using Player;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerComponent : Transformer
    {
        private PlayerControls _controls;
        private PlayerMovement _movement;
        private PlayerCore _core;
        private Rigidbody _rigidbody;
        private PlayerAttack _attack;
        private PlayerStatus _status;
        private PlayerUI _ui;
        private PlayerInventory _inventory;
        private PlayerMarker _marker;
        private PlayerAbilities _abilities;
        private Thrust _thrust;
        
        public PlayerControls Controls => _controls ??= GetComponent<PlayerControls>();
        public PlayerMovement Movement => _movement ??= GetComponent<PlayerMovement>();
        public PlayerCore Core => _core ??= GetComponent<PlayerCore>();
        public Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();
        public PlayerAttack Attack => _attack ??= GetComponent<PlayerAttack>();
        public PlayerStatus Status => _status ??= GetComponent<PlayerStatus>();
        public PlayerUI UI => _ui ??= GetComponent<PlayerUI>();
        public PlayerInventory Inventory => _inventory ??= GetComponent<PlayerInventory>();
        public PlayerMarker Marker => _marker ??= GetComponent<PlayerMarker>();
        public Thrust Thrust => _thrust ??= GetComponent<Thrust>();
        public PlayerAbilities Abilities => _abilities ??= GetComponent<PlayerAbilities>();
    }
}