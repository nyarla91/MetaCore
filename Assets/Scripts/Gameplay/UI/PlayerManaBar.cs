using System;
using NyarlaEssentials;
using Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class PlayerManaBar : Transformer
    {
        [SerializeField] private GameObject _emptySegmentPrefab;
        [SerializeField] private GameObject _fullSegmentPrefab;
        [SerializeField] private RectMask2D _barMask;

        private PlayerMarker _player;
        private float _maxMana;
        
        [Inject]
        private void Construct(PlayerMarker player)
        {
            _player = player;
            _maxMana = _player.Ability.TotalMana;
            _player.Ability.OnManaChanged += UpdateMana;
            InstantiateManaBar();
        }

        private void InstantiateManaBar()
        {
            float[] manaSegments = _player.Ability.ManaSegments;
            float lastX = 0;
            for (int i = 0; i < manaSegments.Length; i++)
            {
                float width = RectTransform.rect.width * (manaSegments[i] / _maxMana);
                InstantiateSegment(_emptySegmentPrefab, lastX, width, RectTransform);
                InstantiateSegment(_fullSegmentPrefab, lastX, width, _barMask.GetComponent<RectTransform>());
                lastX += width;
            }
            _barMask.transform.SetAsLastSibling();

            void InstantiateSegment(GameObject prefab, float x, float width, RectTransform parent)
            {
                RectTransform segment = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent)
                    .GetComponent<RectTransform>();
                segment.anchoredPosition = new Vector2(x, 0);
                segment.sizeDelta = new Vector2(width, 0);
            }
        }

        private void UpdateMana(float mana)
        {
            float missingManaPercent = 1 - (mana / _maxMana);
            _barMask.padding = new Vector4(0, 0, RectTransform.rect.width * missingManaPercent);
        }
    }
}