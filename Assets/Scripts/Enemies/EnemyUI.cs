using NyarlaEssentials;
using UI;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemyUI : EnemyComponent
    {
        [SerializeField] private GameObject _healthBarPrefab;
        [SerializeField] private Transform _healthBarOrigin;
        [SerializeField] private float _healthBarVerticalOffset;
        [SerializeField] private float _healthBarLength;

        private EnemyHealthBar _healthBar;

        [Inject]
        private void Construct(Canvas canvas)
        {
            _healthBar =
                Instantiate(_healthBarPrefab, Vector3.zero, Quaternion.identity, canvas.transform).GetComponent<EnemyHealthBar>();
            _healthBar.Init(_healthBarOrigin, _healthBarVerticalOffset, _healthBarLength, Specie.Status);
        }
    }
}