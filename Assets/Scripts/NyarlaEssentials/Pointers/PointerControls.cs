using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace NyarlaEssentials.Pointers
{
    public static class PointerControls
    {
        private static PointerActions _actions;
        public static PointerActions Actions => _actions ?? Init();

        private static PointerActions Init()
        {
            _actions = new PointerActions();
            _actions.Enable();
            return _actions;
        }
    }
}