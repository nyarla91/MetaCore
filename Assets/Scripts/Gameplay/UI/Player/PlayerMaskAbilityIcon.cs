using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI.Player
{
    public class PlayerMaskAbilityIcon : PlayerCooldownIcon
    {

        private PlayerMarker _player;
        
        [Inject]
        private void Construct(PlayerMarker player)
        {
            _player = player;
            TargetTimer = _player.Abilities.MaskAbilityCooldownTimer;
        }
    }
}