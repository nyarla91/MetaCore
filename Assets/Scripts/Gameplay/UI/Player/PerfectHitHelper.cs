using NyarlaEssentials;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Player;

namespace Gameplay.UI.Player
{
    public class PerfectHitHelper : Transformer
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectMask2D _mask;

        private PlayerMarker _player;
        private float _startingTime;
        
        [Inject]
        private void Construct(PlayerMarker player)
        {
            _player = player;
            _canvasGroup.alpha = 0;
        }

        private void Awake()
        {
            _player.Attack.OnPerfectHitWaiting += Appear;
            _player.Attack.OnPerfectHitFailed += FailedDisappear;
            _player.Attack.OnPerfectHitSucceed += SuceessDisappear;
        }

        private void Update()
        {
            float widthT = 1 - (_player.Attack.PerfectHitTimeLeft / _startingTime);
            widthT *= RectTransform.rect.width / 2;
            _mask.padding = new Vector4(widthT, 0, widthT, 0);
            Vector3 position = CameraProperties.Main.WorldToScreenPoint(_player.transform.position);
            position += new Vector3(0, -40, 0);
            RectTransform.anchoredPosition = position;
        }

        private void Appear()
        {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(0.7f, 0.05f);
            _startingTime = _player.Attack.PerfectHitTimeLeft;
        }
        
        private void FailedDisappear()
        {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(0, 0.1f);
        }
        
        private void SuceessDisappear()
        {
            _canvasGroup.DOKill();
            _canvasGroup.alpha = 1;
            _canvasGroup.DOFade(0, 0.3f);
            RectTransform.localScale = Vector3.one * 2;
            RectTransform.DOScale(Vector3.one, 0.3f);
        }
    }
}