using System;
using System.Collections;
using Player;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;
using PlayerInput = Player.PlayerInput;

namespace World
{
    [RequireComponent(typeof(Collider))]
    public class DoorToNextFloor : MonoBehaviour
    {
        private PlayerInput _playerInput;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerInput playerInput) || playerInput.Status.IsInCombat)
                return;

            _playerInput = playerInput;
            MessageWindow.Instance.Show("Press [F]/(Y) to go to the next floor");
            playerInput.OnInteract += GoToNextFloor;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out PlayerInput playerInput))
                return;

            _playerInput = null;
            MessageWindow.Instance.Hide();
            playerInput.OnInteract -= GoToNextFloor;
        }

        private void GoToNextFloor()
        {
            MessageWindow.Instance.Hide();
            _playerInput.DisabeControls();
            _playerInput.Inventory.StoreInventoryToProgression();
            _playerInput.Status.StoreHealthToProgression();
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