using System;
using NyarlaEssentials;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI.Player
{
    public abstract class PlayerCooldownIcon : MonoBehaviour
    {
        [SerializeField] private GameObject _cooldownGroup;
        [SerializeField] private Image _cooldownDarken;
        [SerializeField] private TextMeshProUGUI _cooldownValue;
        
        private float _startingCooldownTime;
        private Timer _targetTimer;

        protected Timer TargetTimer
        {
            get => _targetTimer;
            set
            {
                if (_targetTimer != null)
                    throw new Exception("TargetTimer can be set only once");
                _targetTimer = value;
                _targetTimer.OnStarted += StartCooldown;
                _targetTimer.OnTick += UpdateCooldown;
                _targetTimer.OnExpired += EndCooldown;
            }
        }

        private void StartCooldown()
        {
            _cooldownGroup.SetActive(true);
            _startingCooldownTime = TargetTimer.Length;
        }
        private void EndCooldown()
        {
            _cooldownGroup.SetActive(false);
        }
        
        private void UpdateCooldown(float time)
        {
            _cooldownDarken.fillAmount = time / _startingCooldownTime;
            _cooldownValue.text = $"{Mathf.CeilToInt(time)}";
        }
    }
}