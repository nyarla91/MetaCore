using System;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerAbility : PlayerComponent
    {
        [SerializeField] private float[] _manaSegments;
        
        private float _mana;
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

        private float _totalMana;
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

        public Action<float> OnManaChanged;

        private void Awake()
        {
            Controls.OnInteract += () => Status.TakeDamage(30, 0.1f);
            Controls.OnTeleportToCore += () => Mana += 8;
            Controls.OnUseAbility += () => print("Ability");
            Controls.OnUseHealing += TryUseHealing;
        }

        private void Start()
        {
            Mana = 0;
        }

        private void TryUseHealing()
        {
            if (!EnoughManaForOneUse)
                return;

            Status.RestoreHealth(Status.MaxHealth * 0.3f);
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