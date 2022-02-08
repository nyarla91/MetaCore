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
        private PlayerVitals _vitals;
        private PlayerInventory _inventory;
        private PlayerMarker _marker;
        private PlayerAbilities _abilities;
        private PlayerStatusContainer _statusContainer;
        private Thrust _thrust;
        
        public PlayerControls Controls => _controls ??= GetComponent<PlayerControls>();
        public PlayerMovement Movement => _movement ??= GetComponent<PlayerMovement>();
        public PlayerCore Core => _core ??= GetComponent<PlayerCore>();
        public Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();
        public PlayerAttack Attack => _attack ??= GetComponent<PlayerAttack>();
        public PlayerVitals Vitals => _vitals ??= GetComponent<PlayerVitals>();
        public PlayerInventory Inventory => _inventory ??= GetComponent<PlayerInventory>();
        public PlayerMarker Marker => _marker ??= GetComponent<PlayerMarker>();
        public Thrust Thrust => _thrust ??= GetComponent<Thrust>();
        public PlayerAbilities Abilities => _abilities ??= GetComponent<PlayerAbilities>();

        public PlayerStatusContainer StatusContainer => _statusContainer ??= GetComponent<PlayerStatusContainer>();
    }
}