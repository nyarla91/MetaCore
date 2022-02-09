using System;
using Gameplay.Enemies;
using Gameplay.Player;

namespace Gameplay.Effects.Statuses
{
    public sealed class StatusDamageBonusVsStunned : StatusDamageBonus
    {
        public StatusDamageBonusVsStunned(PlayerStatusContainer playerStatusContainer, float timeLeft, int percents)
            : base(playerStatusContainer, timeLeft, percents) { }

        protected override Func<EnemyVitals, bool> Condition => enemy => enemy.Specie.StatusContainer.IsStunned;
        protected override string Name => "vsStunned";
    }
}