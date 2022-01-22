using System;
using System.Collections;
using Core;
using NyarlaEssentials;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCore : PlayerComponent
    {
        [SerializeField] private GameObject _coreProjectilePrefab;
        [SerializeField] private GameObject _aimLine_prefab;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _shortCooldown;

        private Transform _aimLine;
        private CoreProjectile _projectile;
        private bool _isCoreOut;
        private bool _coreReady = true;

        public bool IsCoreOut => _isCoreOut;
        
        public Action OnCoreShoot;
        public Action OnCoreCollect;
        
        private void Awake()
        {
            Input.OnCoreReturn += ReturnCore;
            Input.OnTeleportToCore += TeleportToCore;
            Input.OnStartCoreAim += StartAiming;
            Input.OnCancelCoreAim += ExecutePostAiming;
        }

        private void StartAiming() => StartCoroutine(Aiming());

        private IEnumerator Aiming()
        {
            if (IsCoreOut || !_coreReady)
                yield break;

            //_coreReady = false;
            Movement.Freeze();
            _aimLine = Instantiate(_aimLine_prefab, transform.position, Quaternion.identity).transform;
            bool endAiming = false;
            
            Input.OnEndCoreAim += EndAiming;
            while (!endAiming)
            {
                _aimLine.position = transform.position;
                _aimLine.rotation = Quaternion.Euler(0, -Input.RelativeAimVector.XZtoXY().ToDegrees(), 0);
                yield return null;
            }
            Input.OnEndCoreAim -= EndAiming;

            _projectile = Instantiate(_coreProjectilePrefab, transform.position, Quaternion.identity)
                .GetComponent<CoreProjectile>();
            
            _projectile.Init(Input.RelativeAimVector);
            _projectile.OnCoreDestroy += () =>
            {
                _isCoreOut = false;
                OnCoreCollect?.Invoke();
            };
            _isCoreOut = true;
            OnCoreShoot?.Invoke();
            
            ExecutePostAiming();
            
            void EndAiming() => endAiming = true;
        }

        private void ExecutePostAiming()
        {
            Movement.Unfreeze();
            Destroy(_aimLine.gameObject);
            StopAllCoroutines();
        }

        private void ReturnCore()
        {
            if (!_isCoreOut)
                return;

            _projectile.Collect();
        }

        private void TeleportToCore()
        {
            if (!_isCoreOut)
                return;

            Rigidbody.position = _projectile.transform.position;
            _projectile.Collect();
        }
    }
}