using System;
using Core;
using UnityEngine;

namespace Gameplay.World.Enviroment.Totems
{
    public abstract class Totem : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CoreProjectile core))
            {
                Activate();
                Destroy(gameObject);
            }
        }

        protected abstract void Activate();
    }
}