using System;
using NyarlaEssentials;
using Player;
using UnityEngine;

namespace Consumable
{
    public class ConsumablePickUp : Transformer
    {
        [SerializeField] private ConsumableType _type;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerInventory playerConsumables))
            {
                playerConsumables.AddConsumableOfType(_type);

                Destroy(gameObject);
            }
        }
    }
}