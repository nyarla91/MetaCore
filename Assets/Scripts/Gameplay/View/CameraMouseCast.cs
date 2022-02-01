using NyarlaEssentials;
using UnityEngine;
using Zenject;

namespace View
{
    public class CameraMouseCast : MonoBehaviour
    {
        private Vector3 _mousePosition;
        public Vector3 MousePosition => _mousePosition;

        private Controls _controls;
        
        [Inject]
        private void Construct(Controls controls)
        {
            _controls = controls;
        }
        
        private void Update()
        {
            Ray ray = CameraProperties.Main.ScreenPointToRay(_controls.Gameplay.AimPosition.ReadValue<Vector2>());

            if (Physics.Raycast(ray, out RaycastHit hit, 5000, LayerMask.GetMask("TriggerSurface")))
            {
                _mousePosition = hit.point.WithY(0);
            }
        }
    }
}