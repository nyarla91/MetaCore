using Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI.Player
{
    public class PlayerCoreIcon : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _shootIcon;
        [SerializeField] private Sprite _returnIcon;
        
        private PlayerMarker _player;
        
        [Inject]
        private void Construct(PlayerMarker player)
        {
            _player = player;
            _player.Core.OnCoreShoot += TurnToReturn;
            _player.Core.OnCoreCollect += TurnToShoot;
        }

        private void TurnToShoot() => _image.sprite = _shootIcon;
        private void TurnToReturn() => _image.sprite = _returnIcon;
    }
}