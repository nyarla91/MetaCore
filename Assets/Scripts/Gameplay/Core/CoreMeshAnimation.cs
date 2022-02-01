using System;
using UnityEngine;

namespace Core
{
    public class CoreMeshAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _innerRing;
        [SerializeField] private Transform _middleRing;
        [SerializeField] private Transform _outerRing;
        [SerializeField] private float _innerSpeed;
        [SerializeField] private float _middleSpeed;
        [SerializeField] private float _outerSpeed;

        private void FixedUpdate()
        {
            _innerRing.Rotate(_innerSpeed * Time.fixedDeltaTime, 0, 0);
            _middleRing.Rotate(0, _middleSpeed * Time.fixedDeltaTime, 0);
            _outerRing.Rotate(_outerSpeed * Time.fixedDeltaTime, 0, 0);
        }
    }
}