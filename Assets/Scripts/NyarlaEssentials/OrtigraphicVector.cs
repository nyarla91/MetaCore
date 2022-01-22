using NyarlaEssentials;
using UnityEngine;

namespace NyarlaEssentials
{
    public class OrtigraphicVector
    {
        private Vector2 _vector;

        public Vector2 Vector
        {
            get => _vector;
            set => _vector = NEVectors.Align(value, 90);
        }

        public bool IsHorizontal => Mathf.Abs(Vector.x) > Mathf.Abs(Vector.y);
        public bool IsVertical => !IsHorizontal;

        public OrtigraphicVector Opposite => new OrtigraphicVector(_vector * -1);

        public OrtigraphicVector(Vector2 vector)
        {
            Vector = vector;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return obj is OrtigraphicVector && Vector == ((OrtigraphicVector) obj).Vector;
        }

        public override string ToString()
        {
            return Vector.ToString();
        }

        public OrtigraphicVector RotateClockwise()
        {
            Vector = Vector.Rotated(-90);
            return this;
        }
        
        public OrtigraphicVector RotateCounterClockwise()
        {
            Vector = Vector.Rotated(90);
            return this;
        }
        
        public static implicit operator Vector2(OrtigraphicVector me) => me.Vector;
        public static implicit operator OrtigraphicVector(Vector2 me) => new OrtigraphicVector(me);
    }
}