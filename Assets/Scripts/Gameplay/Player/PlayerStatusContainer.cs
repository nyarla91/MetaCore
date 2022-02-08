using System;
using System.Collections.Generic;
using Gameplay.Effects.Statuses;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerStatusContainer : PlayerComponent
    {
        public const string Stunned = "Stunned";

        private List<PlayerStatus> _statuses = new List<PlayerStatus>();

        public event Action OnFixedUpdate;

        public void AddStatus(PlayerStatus addedStatus)
        {
            _statuses.Add(addedStatus);
        }

        public void RemoveSpecificStatus(PlayerStatus removedStatus)
        {
            if (_statuses.Contains(removedStatus))
            {
                _statuses.Remove(removedStatus);
            }
        }

        public void RemoveStatus<T>() where T : PlayerStatus
        {
            for (int i = 0; i < _statuses.Count; i++)
            {
                if (_statuses[i] is not T)
                    continue;
                
                _statuses.RemoveAt(i);
                break;
            }
        }
        
        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
    }
}