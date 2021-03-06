using System;
using NyarlaEssentials;
using Player;
using Project;
using UnityEngine;
using UnityEngine.InputSystem;
using View;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerControls : PlayerComponent
    {
        [SerializeField] [Range(0,1)] private float _innerDeadZone;
        [SerializeField] [Range(0,1)] private float _outerDeadZone;
        
        private Controls _controls;
        private ControlsType _controlsType;
        private CameraMouseCast _cameraMouseCast;
        private CameraView _cameraView;
        private Vector2 _moveVector;

        public Vector2 MoveVector
        {
            get
            {
                if (_moveVector.magnitude < _innerDeadZone)
                    return Vector2.zero;
                if (_moveVector.magnitude > _outerDeadZone)
                    return _moveVector.normalized;
                return _moveVector;
            }
        }

        public Vector3 RelativeMoveVector => CameraView.Vector2ToRelative(MoveVector);

        private Vector2 _aimVector;
        public Vector2 AimVector
        {
            get
            {
                if (_controlsType.IsGamepad)
                {
                    return _aimVector;
                }
                return RelativeAimVector.XZtoXY().Rotated(CameraProperties.YRotation).normalized;
            }
        }

        public Vector3 RelativeAimVector
        {
            get
            {
                if (_controlsType.IsGamepad)
                {
                    return CameraView.Vector2ToRelative(AimVector);
                }
                return (_cameraMouseCast.MousePosition - transform.position).WithY(0).normalized;
            }
        }

        public event Action OnStartCoreAim;
        public event Action OnEndCoreAim;
        public event Action OnCancelCoreAim;
        public event Action OnCoreReturn;
        public event Action OnTeleportToCore;
        public event Action OnAttack;
        public event Action OnDash;
        public event Action OnInteract;
        public event Action OnUseMaskAbility;
        public event Action OnUseHealing;

        public void DisabeControls() => _controls.Gameplay.Disable();

        [Inject]
        private void Construct(Controls controls, ControlsType controlsType, CameraMouseCast cameraMouseCast)
        {
            _controlsType = controlsType;
            _controls = controls;
            _cameraMouseCast = cameraMouseCast;
            _controls.Gameplay.Enable();
        }

        private void Awake()
        {
            SubscribeInputEvents();
        }

        private void Update()
        {
            _moveVector = _controls.Gameplay.Move.ReadValue<Vector2>();
            
            Vector2 rightStickVector = _controls.Gameplay.AimDirection.ReadValue<Vector2>();
            if (rightStickVector.magnitude > _innerDeadZone)
                _aimVector = rightStickVector.normalized;
            else if (_moveVector.magnitude > _innerDeadZone)
                _aimVector = _moveVector.normalized;
        }

        private void OnValidate()
        {
            if (_outerDeadZone < _innerDeadZone)
                _outerDeadZone = _innerDeadZone;
        }

        private void SubscribeInputEvents()
        {
            _controls.Gameplay.CoreShoot.started += OnStartCoreAimInvoke;
            _controls.Gameplay.CoreShoot.canceled += OnEndCoreAimInvoke;
            _controls.Gameplay.CancelAim.performed += OnCancelAimInvoke;
            _controls.Gameplay.CoreReturn.performed += OnCoreReturnInvoke;
            _controls.Gameplay.TeleportToCore.performed += OnTeleportToCoreInvoke;
            _controls.Gameplay.Attack.performed += OnAttackInvoke;
            _controls.Gameplay.Dash.started += OnDashInvoke;
            _controls.Gameplay.Interact.performed += OnInteractInvoke;
            _controls.Gameplay.UseMaskAbility.performed += OnUseMaskAbilityInvoke;
            _controls.Gameplay.UseHealing.performed += OnUseHealingInvoke;
        }

        private void OnStartCoreAimInvoke(InputAction.CallbackContext context) => OnStartCoreAim?.Invoke();
        private void OnEndCoreAimInvoke(InputAction.CallbackContext context) => OnEndCoreAim?.Invoke();
        private void OnCancelAimInvoke(InputAction.CallbackContext context) => OnCancelCoreAim?.Invoke();
        private void OnCoreReturnInvoke(InputAction.CallbackContext context) => OnCoreReturn?.Invoke();
        private void OnTeleportToCoreInvoke(InputAction.CallbackContext context) => OnTeleportToCore?.Invoke();
        private void OnAttackInvoke(InputAction.CallbackContext context) => OnAttack?.Invoke();
        private void OnDashInvoke(InputAction.CallbackContext context) => OnDash?.Invoke();
        private void OnInteractInvoke(InputAction.CallbackContext context) => OnInteract?.Invoke();
        private void OnUseMaskAbilityInvoke(InputAction.CallbackContext context) => OnUseMaskAbility?.Invoke();
        private void OnUseHealingInvoke(InputAction.CallbackContext context) => OnUseHealing?.Invoke();

        private void OnDestroy()
        {
            UnsubscribeInputEvents();
        }

        private void UnsubscribeInputEvents()
        {
            _controls.Gameplay.CoreShoot.started -= OnStartCoreAimInvoke;
            _controls.Gameplay.CoreShoot.canceled -= OnEndCoreAimInvoke;
            _controls.Gameplay.CancelAim.canceled -= OnCancelAimInvoke;
            _controls.Gameplay.CoreReturn.performed -= OnCoreReturnInvoke;
            _controls.Gameplay.TeleportToCore.performed -= OnTeleportToCoreInvoke;
            _controls.Gameplay.Attack.performed -= OnAttackInvoke;
            _controls.Gameplay.Dash.performed -= OnDashInvoke;
            _controls.Gameplay.Interact.performed -= OnInteractInvoke;
            _controls.Gameplay.UseMaskAbility.performed -= OnUseMaskAbilityInvoke;
            _controls.Gameplay.UseHealing.performed -= OnUseHealingInvoke;
        }
    }
}