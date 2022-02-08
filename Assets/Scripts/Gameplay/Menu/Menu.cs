using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Menu
{
    public class Menu : MonoBehaviour
    {
        [Inject] private Controls _controls;

        private void Awake()
        {
            _controls.Gameplay.OpenMenu.performed += Open;
            _controls.Menu.Close.performed += Close;
        }

        private void Open(InputAction.CallbackContext context)
        {
            _controls.Gameplay.Disable();
            _controls.Menu.Enable();
            Time.timeScale = 0;
        }

        private void Close(InputAction.CallbackContext context)
        {
            _controls.Gameplay.Enable();
            _controls.Menu.Disable();
            Time.timeScale = 1;
        }
    }
}