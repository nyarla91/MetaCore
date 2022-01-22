using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUICluster : MonoBehaviour
    {
        [SerializeField] private Image _healthBarLine;
        [SerializeField] private CanvasGroup _shieldBar;
        [SerializeField] private Image _shieldBarLine;

        public Image HealthBarLine => _healthBarLine;
        public CanvasGroup ShieldBar => _shieldBar;
        public Image ShieldBarLine => _shieldBarLine;
    }
}