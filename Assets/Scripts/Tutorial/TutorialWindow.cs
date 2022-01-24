using System;
using NyarlaEssentials;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Tutorial
{
    public class TutorialWindow : Transformer
    {
        private static TutorialWindow _instance;
        public static TutorialWindow Instance => _instance;
        
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private RectTransform _window;
        [SerializeField] private Image _illustration;

        private Controls _controls;
        private Tutorials _tutorials;
        
        private float _t;
        private float _targetT;
        private bool _neverShow;

        [Inject]
        private void Construct(Tutorials tutorials, Controls controls)
        {
            _instance = this;
            _controls = controls;
            _tutorials = tutorials;
            controls.Menu.Cancel.performed += Hide;
            controls.Menu.NeverShow.performed += HideNeverShow;
        }
        
        public void Show(string pageName)
        {
            Page page = _tutorials.GetPage(pageName);
            if (_neverShow || page == null)
                return;
            
            _targetT = 1;
            _label.text = page.Label;
            _text.text = page.TutorialText;
            _illustration.sprite = page.Illustration;
            _controls.Gameplay.Disable();
            _controls.Menu.Enable();
        }

        private void Hide(InputAction.CallbackContext context) => Hide();
        
        public void Hide()
        {
            if (_t < 0.97f)
                return;
            
            _targetT = 0;
            _controls.Gameplay.Enable();
            _controls.Menu.Disable();
        }
        
        private void HideNeverShow(InputAction.CallbackContext context) => HideNeverShow();

        public void HideNeverShow()
        {
            Hide();
            _neverShow = true;
        }

        private void Update()
        {
            _t = Mathf.Lerp(_t, _targetT, Time.deltaTime * 20);
            _window.anchoredPosition = new Vector2(0, (1 - _t) * 200);
            _canvasGroup.alpha = _t;
            Time.timeScale = 1 - _t + 0.01f;
        }
    }
}