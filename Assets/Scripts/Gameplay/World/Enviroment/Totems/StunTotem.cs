using System.Collections.Generic;
using Enemies;
using Gameplay.Enemies;
using UnityEngine;

namespace Gameplay.World.Enviroment.Totems
{
    public class StunTotem : Totem
    {
        [SerializeField] private float _radius;
        [SerializeField] private float _duration;

        protected override void Activate()
        {
            Collider[] colliers = Physics.OverlapSphere(transform.position, _radius, LayerMask.GetMask("Enemy"));
            List<EnemyStatus> statuses = new List<EnemyStatus>();
            foreach (var collider in colliers)
            {
                if (collider.TryGetComponent(out EnemyStatus status))
                {
                    status.Stun(_duration);
                }
            }
        }
    }
}