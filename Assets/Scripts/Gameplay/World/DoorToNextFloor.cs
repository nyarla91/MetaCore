using System;
using System.Collections;
using Gameplay.Player;
using Player;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

namespace World
{
    [RequireComponent(typeof(Collider))]
    public class DoorToNextFloor : MonoBehaviour
    {
        private PlayerControls _playerControls;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerControls playerInput) || playerInput.Status.IsInCombat)
                return;

            _playerControls = playerInput;
            MessageWindow.Instance.Show("<sprite name=\"kF\">/<sprite name=\"gY\"> to go to the next floor");
            playerInput.OnInteract += GoToNextFloor;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out PlayerControls playerInput))
                return;

            _playerControls = null;
            MessageWindow.Instance.Hide();
            playerInput.OnInteract -= GoToNextFloor;
        }

        private void GoToNextFloor()
        {
            MessageWindow.Instance.Hide();
            _playerControls.DisabeControls();
            _playerControls.Inventory.StoreInventoryToProgression();
            _playerControls.Status.StoreHealthToProgression();
            StartCoroutine(SceneTransition());
        }

        private IEnumerator SceneTransition()
        {
            SceneOverlay.Instance.FadeIn();
            yield return new WaitForSeconds(1);
            SceneManager.LoadSceneAsync(0);
        }
    }
}