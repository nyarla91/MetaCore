using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Project
{
    public class ControlsType : MonoBehaviour
    {
        private Controls _controls;

        private bool _isGamepad;
        
        public bool IsGamepad => _isGamepad;
        public bool IsKeyboardMouse => !_isGamepad;

        public Action OnSwitchToKeyboardMouse;
        public Action OnSwitchToGamepad;

        [Inject]
        private void Construct(Controls controls)
        {
            _controls = controls;
            _controls.System.Enable();
            _controls.System.SwitchToKeyboardMouse.performed += SwitchToKeyboardMouse;
            _controls.System.SwitchToGamepad.performed += SwitchToGamepad;
        }

        private void SwitchToKeyboardMouse(InputAction.CallbackContext context)
        {
            _isGamepad = false;
            OnSwitchToKeyboardMouse?.Invoke();
        }

        private void SwitchToGamepad(InputAction.CallbackContext context)
        {
            _isGamepad = true;
            OnSwitchToGamepad?.Invoke();
        }
    }
}