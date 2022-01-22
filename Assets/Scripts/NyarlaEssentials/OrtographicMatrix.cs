using System;
using UnityEngine;

namespace NyarlaEssentials
{
    [Serializable]
    public class OrtographicMatrix<T>
    {
        [SerializeField] private T _top;
        [SerializeField] private T _bottom;
        [SerializeField] private T _left;
        [SerializeField] private T _right;

        public T Top => _top;
        public T Bottom => _bottom;
        public T Left => _left;
        public T Right => _right;

        public OrtographicMatrix(T top, T bottom, T left, T right)
        {
            _top = top;
            _bottom = bottom;
            _left = left;
            _right = right;
        }

        public T ElementFromVector(OrtigraphicVector vector)
        {
            if (vector.IsHorizontal)
            {
                return vector.Vector.x < 0 ? Left : Right;
            }
            return vector.Vector.y < 0 ? Bottom : Top;
        }
    }
}