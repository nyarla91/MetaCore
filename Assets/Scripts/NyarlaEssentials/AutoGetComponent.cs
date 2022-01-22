using UnityEngine;

namespace NyarlaEssentials
{
    public class AutoGetComponent<T> where T : MonoBehaviour
    {
        private GameObject _owner;

        private T _value;
        public T Value => _value ??= _owner.GetComponent<T>();
        
        public AutoGetComponent(GameObject owner)
        {
            _owner = owner;
        }

        public override string ToString() => Value.ToString();
        public override bool Equals(object obj) => Value.Equals(obj);
        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator T(AutoGetComponent<T> me) => me.Value;
    }
}