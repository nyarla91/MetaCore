using System;
using Project;
using UnityEngine;
using Zenject;

namespace UI
{
    public class ControlsRelatedObject : MonoBehaviour
    {
        [SerializeField] private GameObject _gamepadObject;
        [SerializeField] private GameObject _keyboardObject;

        private ControlsType _controlsType;
        
        [Inject]
        private void Construct(ControlsType controlsType)
        {
            _controlsType = controlsType;
            controlsType.OnSwitchToGamepad += ActivateGamepad;
            controlsType.OnSwitchToKeyboardMouse += ActivateKeyboardMouse;
        }

        private void ActivateGamepad()
        {
            _gamepadObject.SetActive(true);
            _keyboardObject.SetActive(false);
        }

        private void ActivateKeyboardMouse()
        {
            _gamepadObject.SetActive(false);
            _keyboardObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _controlsType.OnSwitchToGamepad += ActivateGamepad;
            _controlsType.OnSwitchToKeyboardMouse += ActivateKeyboardMouse;
        }
    }
}