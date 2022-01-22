using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NyarlaEssentials.Pointers
{
    public class PointerCaster : MonoBehaviour
    {
        private enum PointerAction
        {
            Down,
            Up,
            Click,
            DoubleClick
        
        }
        private enum DetectionType
        {
            Overlap2D,
            Paycast3D
        }


        private static PointerCaster _instance;
        public static PointerCaster Instance => _instance;

        [SerializeField] private DetectionType _detectionType;

        public List<PointerTarget> targets = new List<PointerTarget>();

        private void Awake()
        {
            _instance = this;
            PointerControls.Actions.Mouse.Enable();
            PointerControls.Actions.Touch.Enable();

            
            #region Callbacks subscribe
            
            PointerControls.Actions.Mouse.LeftMouse.started += LeftMouseStarted;
            PointerControls.Actions.Mouse.LeftMouse.canceled += LefteMouseCanceled;
            PointerControls.Actions.Mouse.LeftClick.performed += LeftMousePerformed;
            PointerControls.Actions.Mouse.LeftDouble.performed += LeftMouseDouble;
            PointerControls.Actions.Mouse.RightMouse.started += RightMouseStarted;
            PointerControls.Actions.Mouse.RightMouse.canceled += RightMouseCanceled;
            PointerControls.Actions.Mouse.RightClick.performed += RightMousePerformed;
            PointerControls.Actions.Mouse.RightDouble.performed += RightMouseDouble;
            PointerControls.Actions.Mouse.MiddleMouse.started += MiddleMouseStarted;
            PointerControls.Actions.Mouse.MiddleMouse.canceled += MiddleMouseCanceled;
            PointerControls.Actions.Mouse.MiddleClick.performed += MiddleMousePerformed;
            PointerControls.Actions.Mouse.MiddleDouble.performed += MiddleMouseDouble;

            PointerControls.Actions.Mouse.LeftMouse.canceled += LeftMouseDragEnd;
            PointerControls.Actions.Mouse.RightMouse.canceled += RightMouseDragEnd;
            PointerControls.Actions.Mouse.MiddleMouse.canceled += MiddleMouseDragEnd;

            PointerControls.Actions.Touch.Tap.started += FingerStarted;
            PointerControls.Actions.Touch.Tap.canceled += FingerCanceled;
            PointerControls.Actions.Touch.Tap.performed += FingerPerformed;
            PointerControls.Actions.Touch.DoubleTap.performed += FingerDouble;

            PointerControls.Actions.Touch.Tap.canceled += FingerDragEnd;

            #endregion
        }

        #region CallbacksMethods
        private void LeftMouseStarted(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Left, PointerAction.Down);
        private void LefteMouseCanceled(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Left, PointerAction.Up);
        private void LeftMousePerformed(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Left, PointerAction.Click);
        private void LeftMouseDouble(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Left, PointerAction.DoubleClick);
        private void RightMouseStarted(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Right, PointerAction.Down);
        private void RightMouseCanceled(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Right, PointerAction.Up);
        private void RightMousePerformed(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Right, PointerAction.Click);
        private void RightMouseDouble(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Right, PointerAction.DoubleClick);
        private void MiddleMouseStarted(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Middle, PointerAction.Down);
        private void MiddleMouseCanceled(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Middle, PointerAction.Up);
        private void MiddleMousePerformed(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Middle, PointerAction.Click);
        private void MiddleMouseDouble(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Middle, PointerAction.DoubleClick);

        private void LeftMouseDragEnd(InputAction.CallbackContext context) => DragEnd(PointerType.Left);
        private void RightMouseDragEnd(InputAction.CallbackContext context) => DragEnd(PointerType.Right);
        private void MiddleMouseDragEnd(InputAction.CallbackContext context) => DragEnd(PointerType.Middle);

        private void FingerStarted(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Finger, PointerAction.Down);
        private void FingerCanceled(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Finger, PointerAction.Up);
        private void FingerPerformed(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Finger, PointerAction.Click);
        private void FingerDouble(InputAction.CallbackContext context) => PerformPointerAction(PointerType.Finger, PointerAction.DoubleClick);
        private void FingerDragEnd(InputAction.CallbackContext context) => DragEnd(PointerType.Finger);

        #endregion
        
        private void PerformPointerAction(PointerType pointer, PointerAction action)
        {
            Vector3 contactPoint = Vector3.zero;
            PointerTarget target = FindTarget(pointer != PointerType.Finger, out contactPoint);
            if (target == null)
                return;
            switch (action)
            {
                case PointerAction.Down:
                {
                    target.OnDown(pointer, contactPoint);
                    break;
                }
                case PointerAction.Up:
                {
                    target.OnUp(pointer, contactPoint);
                    break;
                }
                case PointerAction.Click:
                {
                    target.OnClick(pointer, contactPoint);
                    break;
                }
                case PointerAction.DoubleClick:
                {
                    target.OnDoubleClick(pointer, contactPoint);
                    break;
                }
            }
        }

        private void DragEnd(PointerType pointer)
        {
            foreach (var target in targets)
            {
                if (target != null)
                    target.TryEndDrag(pointer);
            }
        }

        private PointerTarget FindTarget(bool mouse, out Vector3 contactPoint)
        {
            Vector2 positionAt = mouse
                ? (Vector2) Input.mousePosition
                : PointerControls.Actions.Touch.TapPosition.ReadValue<Vector2>();
            
            if (_detectionType == DetectionType.Overlap2D)
            {
                Vector3 overlapPoint = CameraProperties.Main.ScreenToWorldPoint(positionAt);
                Collider2D[] colliders = Physics2D.OverlapPointAll(overlapPoint);
                if (colliders.Length > 0)
                {
                    colliders = colliders.Where(collider => collider.GetComponent<PointerTarget>() != null)
                        .OrderBy(collider => collider.transform.position.z).ToArray();
                    contactPoint = NEVectors.WithZ(overlapPoint, colliders[0].transform.position.z);
                    return colliders[0].GetComponent<PointerTarget>();
                }
            }
            else
            {
                Ray ray = CameraProperties.Main.ScreenPointToRay(positionAt);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    contactPoint = hit.point;
                    return hit.collider.GetComponent<PointerTarget>();
                }
            }
            contactPoint = Vector3.zero;
            return null;
        }
    
        private void OnDestroy()
        {
            PointerControls.Actions.Mouse.LeftMouse.started -= LeftMouseStarted;
            PointerControls.Actions.Mouse.LeftMouse.canceled -= LefteMouseCanceled;
            PointerControls.Actions.Mouse.LeftClick.performed -= LeftMousePerformed;
            PointerControls.Actions.Mouse.LeftDouble.performed -= LeftMouseDouble;
            PointerControls.Actions.Mouse.RightMouse.started -= RightMouseStarted;
            PointerControls.Actions.Mouse.RightMouse.canceled -= RightMouseCanceled;
            PointerControls.Actions.Mouse.RightClick.performed -= RightMousePerformed;
            PointerControls.Actions.Mouse.RightDouble.performed -= RightMouseDouble;
            PointerControls.Actions.Mouse.MiddleMouse.started -= MiddleMouseStarted;
            PointerControls.Actions.Mouse.MiddleMouse.canceled -= MiddleMouseCanceled;
            PointerControls.Actions.Mouse.MiddleClick.performed -= MiddleMousePerformed;
            PointerControls.Actions.Mouse.MiddleDouble.performed -= MiddleMouseDouble;

            PointerControls.Actions.Mouse.LeftMouse.canceled -= LeftMouseDragEnd;
            PointerControls.Actions.Mouse.RightMouse.canceled -= RightMouseDragEnd;
            PointerControls.Actions.Mouse.MiddleMouse.canceled -= MiddleMouseDragEnd;

            PointerControls.Actions.Touch.Tap.started -= FingerStarted;
            PointerControls.Actions.Touch.Tap.canceled -= FingerCanceled;
            PointerControls.Actions.Touch.Tap.performed -= FingerPerformed;
            PointerControls.Actions.Touch.DoubleTap.performed -= FingerDouble;

            PointerControls.Actions.Touch.Tap.canceled -= FingerDragEnd;

            _instance = null;
        }
    }

    public enum PointerType
    {
        Left,
        Right,
        Middle,
        Finger
    }
}