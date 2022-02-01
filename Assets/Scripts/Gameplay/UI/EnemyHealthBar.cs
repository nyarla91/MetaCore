using System;
using Enemies;
using Newtonsoft.Json;
using NyarlaEssentials;
using UnityEngine;

namespace UI
{
    public class EnemyHealthBar : Transformer
    {
        [SerializeField] private RectTransform _line;

        private float _verticalOffset;
        private Transform _target;
        
        public void Init(Transform target, float verticalOffset, float length, EnemyStatus targetStatus)
        {
            _target = target;
            _verticalOffset = verticalOffset;
            RectTransform.sizeDelta = new Vector2(length, 20);
            targetStatus.OnHealthPercentChanged += SetHealthbBar;
            targetStatus.OnDeath += Destroy;
        }

        private void SetHealthbBar(float percent)
        {
            _line.localScale = new Vector3(percent, 1, 1);
        }

        private void Update()
        {
            Vector3 targetPosition =CameraProperties.Main.WorldToScreenPoint(_target.position)
                                    + new Vector3(0, _verticalOffset, 0);

            RectTransform.anchoredPosition = targetPosition;
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}