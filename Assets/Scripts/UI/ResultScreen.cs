using System;
using System.Collections;
using System.Collections.Generic;
using NyarlaEssentials;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using World;
using Zenject;

namespace UI
{
    public class ResultScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _floorCounter;
        [SerializeField] private TextMeshProUGUI _killsCounter;
        [SerializeField] private TextMeshProUGUI _timeCounter;

        private Controls _controls;
        private PlayerMarker _playerMarker;
        
        [Inject]
        private void Construct(Controls controls, PlayerMarker playerMarker)
        {
            _playerMarker = playerMarker;
            _playerMarker.Status.OnDeath += Show;
            
            _controls = controls;
            _controls.Menu.Confirm.performed += Restart;
            _controls.Menu.Confirm.performed += Quit;
        }
        
        private void Show()
        {
            Time.timeScale = 0.01f;
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _floorCounter.text = Progression.Floor.ToString();
            _killsCounter.text = Progression.Kills.ToString();
            _timeCounter.text = NEString.SecondsToFormatTime(Mathf.CeilToInt(Progression.RunTime), false);
            
        }

        public void Restart(InputAction.CallbackContext context) => Restart();

        public void Restart()
        {
            if (!_playerMarker.Status.IsDead)
                return;

            Progression.Reset();
            StartCoroutine(SceneTransition());
            Time.timeScale = 1;
        }
        
        public void Quit(InputAction.CallbackContext context) => Quit();

        public void Quit()
        {
            if (!_playerMarker.Status.IsDead)
                return;
        }
        
        private IEnumerator SceneTransition()
        {
            SceneOverlay.Instance.FadeIn();
            yield return new WaitForSeconds(1);
            SceneManager.LoadSceneAsync(0);
        }
        
        private void FixedUpdate()
        {
            Progression.RunTime += Time.fixedDeltaTime;
        }
    }
}