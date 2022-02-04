using System;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI.Player
{
    public class PlayerCoreTeleportIcon : PlayerCooldownIcon
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        private PlayerMarker _player;
        private float _maxCooldownTime;
        
        [Inject]
        private void Construct(PlayerMarker player)
        {
            _player = player;
            TargetTimer = _player.Core.TeleportCooldownTimer;
            _player.Core.OnCoreShoot += FadeIn;
            _player.Core.OnCoreCollect += FadeOut;
        }

        private void FadeIn()
        {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(1, 0.2f);
        }
        
        private void FadeOut()
        {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(0.3f, 0.2f);
        }
    }
}