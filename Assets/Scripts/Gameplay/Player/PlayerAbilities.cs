using System;
using NyarlaEssentials;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerAbilities : PlayerComponent
    {
        [SerializeField] private float _maskAbilityCooldown;
        [SerializeField] private float[] _manaSegments;
        
        private float _mana;
        private float _totalMana;
        private Timer _maskAbilityCooldownTimer;
        
        public float Mana
        {
            get => _mana;
            set
            {
                _mana = Mathf.Clamp(value, 0, TotalMana);
                OnManaChanged?.Invoke(_mana);
            }
        }
        public float[] ManaSegments => _manaSegments;
        public float TotalMana
        {
            get
            {
                if (_totalMana > 0)
                    return _totalMana;
                foreach (var manaPool in _manaSegments)
                {
                    _totalMana += manaPool;
                }
                return _totalMana;
            }
        }
        private bool EnoughManaForOneUse => Mana >= ManaSegments[0];
        public float MaskAbilityCooldownLeft => _maskAbilityCooldownTimer.TimeLeft;
        public Timer MaskAbilityCooldownTimer => _maskAbilityCooldownTimer;
        public event Action<float> OnManaChanged;
        public event Action OnMaskAbilityUsed;

        private void Awake()
        {
            Controls.OnInteract += () => Vitals.TakeDamage(30, 0.1f);
            Controls.OnTeleportToCore += () => Mana += 8;
            Controls.OnUseMaskAbility += TryUseMaskAbility;
            Controls.OnUseHealing += TryUseHealing;
            _maskAbilityCooldownTimer = new Timer(this, _maskAbilityCooldown);
        }

        private void Start()
        {
            Mana = 0;
        }

        private void TryUseHealing()
        {
            if (!EnoughManaForOneUse)
                return;

            Vitals.RestoreHealth(Vitals.MaxHealth * 0.3f);
            SpendManaSegment();
        }

        private void TryUseMaskAbility()
        {
            if (!EnoughManaForOneUse || !_maskAbilityCooldownTimer.IsExpired)
                return;

            Vitals.RestoreHealth(5);
            _maskAbilityCooldownTimer.Restart();
            OnMaskAbilityUsed?.Invoke();
            SpendManaSegment();
        }

        private void SpendManaSegment()
        {
            if (!EnoughManaForOneUse)
                return;

            int fullSegments = 0;
            float fullSegmentsVolume = 0;
            float manaRemaining = Mana;
            for (int i = 0; i < _manaSegments.Length - 1; i++)
            {
                if (manaRemaining >= ManaSegments[i])
                {
                    manaRemaining -= ManaSegments[i];
                    fullSegmentsVolume += ManaSegments[i];
                    fullSegments++;
                    continue;
                }
                break;
            }
            float currentSegmentMissingPercent = 1 - (Mana - fullSegmentsVolume) / ManaSegments[fullSegments];
            Mana = fullSegmentsVolume - ManaSegments[fullSegments - 1] * currentSegmentMissingPercent;
        }
    }
}