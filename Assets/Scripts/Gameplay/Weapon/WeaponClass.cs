using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Weapon
{
    [CreateAssetMenu(menuName = "Weapon Class")]
    public class WeaponClass : ScriptableObject
    {
        [SerializeField] private float _seriesRestorationTime;
        [SerializeField] private WeaponAttack[] _attacks;

        public int AttacksCount => _attacks.Length;
        public float SeriesRestorationTime => _seriesRestorationTime;
        
        public WeaponAttack GetAttack(int index) => _attacks[index];
    }

    [Serializable]
    public class WeaponAttack
    {
        [SerializeField] private float _swingTime;
        [SerializeField] private float _restorationTime;
        [SerializeField] private float _thrustForce;
        [SerializeField] private float _pushModifier = 1;
        [SerializeField] private float _damageModifier = 1;

        public float SwingTime => _swingTime;
        public float RestorationTime => _restorationTime;
        public float ThrustForce => _thrustForce;
        public float PushModifier => _pushModifier;
        public float DamageModifier => _damageModifier;
    }
}