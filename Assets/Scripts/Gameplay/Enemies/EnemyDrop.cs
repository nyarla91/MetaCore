using System.Collections.Generic;
using Enemies;
using NyarlaEssentials;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyDrop : EnemyComponent
    {
        [SerializeField] [Range(0, 1)] private float _dropChance;
        [SerializeField] private List<GameObject> _consumables;

        private void Awake()
        {
            Specie.Status.OnDeath += TryDrop;
        }

        private void TryDrop()
        {
            if (Random.value > _dropChance)
                return;

            Instantiate(_consumables.ChooseRandomElement(), transform.position, Quaternion.identity);
        }
    }
}