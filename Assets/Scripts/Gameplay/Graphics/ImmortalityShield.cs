using System;
using System.Collections;
using NyarlaEssentials;
using UnityEngine;

namespace Graphics
{
    public class ImmortalityShield : Transformer
    {
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _yOffset;
        
        private Transform _parent;
        private float _targetScale = 1.5f;
        
        public void Init(float signalTime,float duration, Transform parent)
        {
            _parent = parent;
            StartCoroutine(Signal(signalTime));
            StartCoroutine(SelfDestruct(duration));
        }

        private IEnumerator Signal(float time)
        {
            yield return new WaitForSeconds(time);
            _targetScale = 1;
        }

        private IEnumerator SelfDestruct(float duration)
        {
            yield return new WaitForSeconds(duration);
            gameObject.SelfDestruct();
        }

        private void FixedUpdate()
        {
            transform.position = _parent.position + new Vector3(0, _yOffset, 0);
            transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
            transform.localScale = Mathf.Lerp(transform.localScale.x, _targetScale, Time.fixedDeltaTime * 4) * Vector3.one;
        }
    }
}