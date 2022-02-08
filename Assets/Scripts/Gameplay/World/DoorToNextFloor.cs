using System.Collections;
using Gameplay.Player;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.World
{
    [RequireComponent(typeof(Collider))]
    public class DoorToNextFloor : MonoBehaviour
    {
        private PlayerControls _playerControls;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerControls playerInput) || playerInput.Vitals.IsInCombat)
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
            _playerControls.Vitals.StoreHealthToProgression();
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