using UI;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerUI : PlayerComponent
    {
        private PlayerUICluster _cluster;
        
        [Inject]
        private void Construct(PlayerUICluster cluster)
        {
            _cluster = cluster;
        }
    }
}