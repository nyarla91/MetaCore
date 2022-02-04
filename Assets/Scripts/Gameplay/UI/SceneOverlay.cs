using System;
using System.Collections;
using Gameplay.World;
using UnityEngine;
using World;

namespace UI
{
    public class SceneOverlay : MonoBehaviour
    {
        private static SceneOverlay _instance;
        public static SceneOverlay Instance => _instance;
        
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeSpeed;

        public void FadeIn()
        {
            StopAllCoroutines();
            StartCoroutine(Fade(false));
        }

        public void FadeOut()
        {
            StopAllCoroutines();
            StartCoroutine(Fade(true));
            Progression.Floor++;
        }

        private void Awake()
        {
            _instance = this;
            FadeOut();
        }

        private IEnumerator Fade(bool isOut)
        {
            for (float i = 0; i < 1; i += Time.deltaTime * _fadeSpeed)
            {
                _canvasGroup.alpha = isOut ? (1 - i) : i;
                yield return new WaitForFixedUpdate();
            }
            MessageWindow.Instance.Show("Floor " + Progression.Floor, 3);
        }
    }
}