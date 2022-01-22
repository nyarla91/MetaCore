using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Player
{
    public class PlayerUI : PlayerComponent
    {
        [SerializeField] private float _shieldScaleSpeed;
        private PlayerUICluster _cluster;
        private Image _healthBarLine;
        private Image _shieldBarLine;
        private CanvasGroup _shieldBar;
        private Image _teleportCooldown;
        private TextMeshProUGUI _teleportCooldownValue;

        private float _shieldsTargetT;
        private float _shieldsT;
        
        [Inject]
        private void Construct(PlayerUICluster cluster)
        {
            _cluster = cluster;
            _healthBarLine = _cluster.HealthBarLine;
            _shieldBarLine = _cluster.ShieldBarLine;
            _shieldBar = _cluster.ShieldBar;
            _teleportCooldown = cluster.TeleportCooldown;
            _teleportCooldownValue = cluster.TeleportCooldownValue;
            Status.OnHealthPercentChanged += SetHealthBarLength;
            Status.OnShieldsPercentChanged += SetShieldBarLength;
            Core.OnCoreShoot += () => { _shieldsTargetT = 1; };
            Core.OnCoreCollect += () => { _shieldsTargetT = 0; };

        }
        
        private void SetHealthBarLength(float percent)
        {
            _healthBarLine.rectTransform.localScale = new Vector3(percent, 1, 1);
        }
        private void SetShieldBarLength(float percent)
        {
            _shieldBarLine.rectTransform.localScale = new Vector3(percent, 1, 1);
        }

        private void FixedUpdate()
        {
            _shieldsT = Mathf.Lerp(_shieldsT, _shieldsTargetT, _shieldScaleSpeed * Time.fixedDeltaTime);
            _shieldBar.alpha = 1 - _shieldsT * 0.6f;
            _shieldBar.GetComponent<RectTransform>().localScale =
                new Vector3(1 - _shieldsT * 0.2f, 1 - _shieldsT * 0.5f, 1);

            float teleportCooldownLeft = Core.TeleportCooldownLeft;
            float targetT = teleportCooldownLeft > 0 ? 0.6f : 1;
            float t = Mathf.Lerp(_teleportCooldown.rectTransform.localScale.x, targetT, Time.fixedDeltaTime * 5);
            string teleportText = teleportCooldownLeft > 0 ? Mathf.CeilToInt(teleportCooldownLeft).ToString() : string.Empty;
            _teleportCooldown.color = new Color(t, t, t, 1);
            _teleportCooldown.rectTransform.localScale = new Vector3(t, t, t);
            _teleportCooldownValue.text = teleportText;
        }
    }
}