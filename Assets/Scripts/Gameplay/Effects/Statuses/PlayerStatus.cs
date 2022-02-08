using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Effects.Statuses
{
    public abstract class PlayerStatus
    {
        private float _duration;
        private float _timeLeft;
        public abstract int Power { get; }

        public float Duration => _duration;
        public float TimeLeft => _timeLeft;

        private PlayerStatusContainer _playerStatusContainer;

        protected PlayerStatusContainer PlayerStatusContainer => _playerStatusContainer;
        
        protected PlayerStatus(PlayerStatusContainer playerStatusContainer, float duration)
        {
            _duration = _timeLeft = duration;
            _playerStatusContainer = playerStatusContainer;
            playerStatusContainer.OnFixedUpdate += FixedUpdate;
        }

        protected abstract void Remove();

        protected virtual void FixedUpdate()
        {
            _timeLeft -= Time.fixedDeltaTime;
            if (_timeLeft <= 0)
            {
                _playerStatusContainer.RemoveSpecificStatus(this);
            }
        }

        ~PlayerStatus()
        {
            Debug.Log("StatusRemoved");
            Remove();
        }
    }
}