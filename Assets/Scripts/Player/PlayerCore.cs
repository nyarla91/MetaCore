using System;
using System.Collections;
using Core;
using NyarlaEssentials;
using NyarlaEssentials.Sound;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCore : PlayerComponent
    {
        [SerializeField] private GameObject _coreProjectilePrefab;
        [SerializeField] private GameObject _aimLine_prefab;
        [SerializeField] private float _teleportCooldown;
        [SerializeField] private GameObject _coreInPlayer;

        private Transform _aimLine;
        private CoreProjectile _projectile;
        private bool _isCoreOut;
        private float _teleportCooldownLeft;

        public bool IsCoreOut => _isCoreOut;
        public float TeleportCooldownLeft => _teleportCooldownLeft;
        public CoreProjectile Projectile => _projectile;
        
        public Action OnCoreShoot;
        public Action OnCoreCollect;

        public void ForceRestoreTeleport()
        {
            _teleportCooldownLeft = 0;
        }
        
        private void Awake()
        {
            Input.OnCoreReturn += ReturnCore;
            Input.OnTeleportToCore += TeleportToCore;
            Input.OnStartCoreAim += StartAiming;
            OnCoreShoot += () =>
            {
                _coreInPlayer.SetActive(false);
                SoundPlayer.Play("shoot", 1);
            };
            OnCoreCollect += () => _coreInPlayer.SetActive(true);
            //Input.OnCancelCoreAim += ExecutePostAiming;
        }

        private void StartAiming() => StartCoroutine(Aiming());

        private IEnumerator Aiming()
        {
            if (IsCoreOut)
                yield break;
            
            Marker.Animator.SetBool("Aiming", true);
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
            Marker.Animator.SetBool("Aiming", false);
            _aimLine?.gameObject?.SelfDestruct();
            StopAllCoroutines();
        }

        public void ReturnCore()
        {
            if (!_isCoreOut)
                return;

            _projectile.Collect();
        }

        private void TeleportToCore()
        {
            if (!_isCoreOut || _teleportCooldownLeft > 0)
                return;

            SoundPlayer.Play("teleport", 2);
            Rigidbody.position = _projectile.transform.position;
            _teleportCooldownLeft = _teleportCooldown;
            _projectile.Collect();
        }

        private void FixedUpdate()
        {
            _teleportCooldownLeft -= Time.fixedDeltaTime;
        }
    }
}