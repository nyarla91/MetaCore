using System;
using System.Collections;
using NyarlaEssentials;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MessageWindow : Transformer
    {
        private static MessageWindow _instance;
        public static MessageWindow Instance => _instance;
        
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        private float _t;
        private float _targetT;

        private void Awake()
        {
            _instance = this;
        }

        public void Show(string message, float duration)
        {
            Show(message);
            StartCoroutine(HideAfterTime(duration));
        }

        public void Show(string message)
        {
            _text.text = message;
            _targetT = 1;
            StopAllCoroutines();
        }

        public void Hide()
        {
            _targetT = 0;
            StopAllCoroutines();
        }

        private IEnumerator HideAfterTime(float duration)
        {
            yield return new WaitForSeconds(duration);
            Hide();
        }

        private void Update()
        {
            _t = Mathf.Lerp(_t, _targetT, Time.fixedDeltaTime * 5);
            _canvasGroup.alpha = _t;
            RectTransform.anchoredPosition = new Vector2(0, (1 - _t) * 40);
        }
    }
}