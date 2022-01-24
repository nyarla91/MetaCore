using System.Collections.Generic;
using NyarlaEssentials;
using Tutorial;
using UnityEngine;

namespace Enemies
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

            GameObject consumable = Tutorials.MechanicTutorialProgress < Tutorials.MechanicTutorialsTotal
                ? _consumables[0]
                : _consumables.ChooseRandomElement();
            Instantiate(consumable, transform.position, Quaternion.identity);
        }
    }
}