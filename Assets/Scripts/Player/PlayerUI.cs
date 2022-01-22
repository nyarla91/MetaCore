using System;
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

        private float _shieldsTargetT;
        private float _shieldsT;
        
        [Inject]
        private void Construct(PlayerUICluster cluster)
        {
            _cluster = cluster;
            _healthBarLine = _cluster.HealthBarLine;
            _shieldBarLine = _cluster.ShieldBarLine;
            _shieldBar = _cluster.ShieldBar;
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
        }
    }
}