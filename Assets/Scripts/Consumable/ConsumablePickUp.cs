using System;
using NyarlaEssentials;
using Player;
using Tutorial;
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

                if (!Tutorials.ConsumableTutorialsSeen.Contains(_type))
                {
                    print(_type.ToString());
                    Tutorials.ConsumableTutorialsSeen.Add(_type);
                    TutorialWindow.Instance.Show(_type.ToString());
                }
                
                Destroy(gameObject);
            }
        }
    }
}