using System;
using Gameplay.Enemies;
using Gameplay.Player;

namespace Gameplay.Effects.Statuses
{
    public abstract class StatusDamageBonus : PlayerStatus
    {
        private PlayerAttackDamageBonus _bonus;

        public StatusDamageBonus(PlayerStatusContainer playerStatusContainer, float timeLeft, int percents)
            : base(playerStatusContainer, timeLeft)
        {
            _bonus = new PlayerAttackDamageBonus(Condition, percents);
            PlayerStatusContainer.Attack.damageModifiers.Add(Name, _bonus);
        }

        protected virtual Func<EnemyVitals, bool> Condition => enemy => true;
        protected abstract string Name { get; }

        public override int Power => _bonus.Percents;

        protected override void Remove()
        {
            PlayerStatusContainer.Attack.damageModifiers.Remove(Name);
        }
    }
}