using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NyarlaEssentials.Pointers
{
    public class PointerTarget : MonoBehaviour
    {
        
        [Tooltip("Period of invoking OnDrag event\n0 or less - every frame")]
        [SerializeField] private float _dragPeriod;
        public float DeltaDragTime => _dragPeriod > 0 ? _dragPeriod : Time.deltaTime;
        
        public delegate void MousePointActionHandler(PointerType button, Vector3 contactPoint);
        public MousePointActionHandler OnDown, OnUp, OnClick, OnDoubleClick;
        
        public delegate void MouseActionHandler(PointerType button);
        public MouseActionHandler OnDrag, OnDragEnd;

        private Dictionary<PointerType, Coroutine> _dragCoroutines = new Dictionary<PointerType, Coroutine>();

        private void Awake()
        {
            PointerCaster.Instance.targets.Add(this);
            OnDown += PoinerPositionPlug;
            OnUp += PoinerPositionPlug;
            OnClick += PoinerPositionPlug;
            OnDoubleClick += PoinerPositionPlug;
            OnDrag += PointerPlug;
            OnDragEnd += PointerPlug;
            OnDown += (button, contact) => _dragCoroutines.Add(button, StartCoroutine(Drag(button)));
        }

        private void PoinerPositionPlug(PointerType button, Vector3 contactPoint)
        {
            
        }
        private void PointerPlug(PointerType button)
        {
            
        }

        private IEnumerator Drag(PointerType button)
        {
            while (true)
            {
                OnDrag(button);
                if (_dragPeriod > 0)
                    yield return new WaitForSeconds(_dragPeriod);
                else
                    yield return null;
            }
                
        }

        public void TryEndDrag(PointerType button)
        {
            if (_dragCoroutines.ContainsKey(button) && _dragCoroutines[button] != null)
            {
                StopCoroutine(_dragCoroutines[button]);
                _dragCoroutines.Remove(button);
                OnDragEnd(button);
            }
        }

        private void OnDestroy()
        {
            PointerCaster.Instance?.targets.Remove(this);
        }
    }

}