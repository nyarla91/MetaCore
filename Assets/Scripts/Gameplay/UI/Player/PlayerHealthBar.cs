using NyarlaEssentials;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI.Player
{
    public class PlayerHealthBar : Transformer
    {
        [SerializeField] private RectMask2D _currentHealthBar;
        [SerializeField] private RectTransform _maxHealthBar;
        [SerializeField] private TextMeshProUGUI _currentMaxHealthValue;
        [SerializeField] private TextMeshProUGUI _totalHealthValue;

        private PlayerMarker _player;
        
        [Inject]
        private void Construct(PlayerMarker player)
        {
            _player = player;
            _player.Vitals.OnHealthChanged += UpdateBars;
        }

        private void UpdateBars(float currentHealth, float maxHealth, float totalHealth)
        {
            float totalHealthWidth = RectTransform.rect.width;
            float maxHealthWidth = totalHealthWidth * maxHealth / totalHealth;
            float currentHealthWidth = maxHealthWidth * currentHealth / maxHealth;
            _maxHealthBar.sizeDelta = new Vector2(-totalHealthWidth + maxHealthWidth, 0);
            _currentHealthBar.padding = new Vector4(0, 0, maxHealthWidth - currentHealthWidth);
            _currentMaxHealthValue.text = $"{Mathf.CeilToInt(currentHealth)}/{Mathf.CeilToInt(maxHealth)}";
            _totalHealthValue.text = $"{Mathf.CeilToInt(totalHealth)}";
        }
    }
}