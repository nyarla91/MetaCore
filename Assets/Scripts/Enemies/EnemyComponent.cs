using Enemies.Species;
using NyarlaEssentials;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemySpecie))]
    public class EnemyComponent : Transformer
    {
        private EnemySpecie _specie;

        public EnemySpecie Specie => _specie ??= GetComponent<EnemySpecie>();
    }
}