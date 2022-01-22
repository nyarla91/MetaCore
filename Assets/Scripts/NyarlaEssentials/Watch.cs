using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NyarlaEssentials
{
    public struct Watch<T>
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (!value.Equals(_value))
                {
                    PreviousValue = _value;
                    _value = value;
                    OnChanged?.Invoke(_value, PreviousValue);
                }
            }
        }

        public T PreviousValue { get; private set; }

        public Action<T, T> OnChanged;

        public T this[int index]
        {
            set
            {
                if (index != 0)
                    throw new Exception("Only 0 index is allowed");
                Value = value;
            }
        }

        public override string ToString() => Value.ToString();
        public override bool Equals(object obj) => Value.Equals(obj);
        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator T(Watch<T> me) => me.Value;
    }

}