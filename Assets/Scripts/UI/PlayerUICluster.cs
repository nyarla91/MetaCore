using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUICluster : MonoBehaviour
    {
        [SerializeField] private Image _healthBarLine;
        [SerializeField] private CanvasGroup _shieldBar;
        [SerializeField] private Image _shieldBarLine;
        [SerializeField] private Image _teleportCooldown;
        [SerializeField] private TextMeshProUGUI _teleportCooldownValue;

        public Image HealthBarLine => _healthBarLine;
        public CanvasGroup ShieldBar => _shieldBar;
        public Image ShieldBarLine => _shieldBarLine;
        public Image TeleportCooldown => _teleportCooldown;
        public TextMeshProUGUI TeleportCooldownValue => _teleportCooldownValue;
    }
}