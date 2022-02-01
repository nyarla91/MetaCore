using System;
using System.Collections;
using UnityEngine;

namespace Graphics
{
    public class Lifetime : MonoBehaviour
    {
        [SerializeField] private float _lifetime;

        private void Start()
        {
            StartCoroutine(DestroyAfterDuration());
        }

        private IEnumerator DestroyAfterDuration()
        {
            yield return new WaitForSeconds(_lifetime);
            Destroy(gameObject);
        }
    }
}