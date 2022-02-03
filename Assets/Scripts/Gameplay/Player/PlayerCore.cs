using System;
using System.Collections;
using Core;
using Gameplay.Player;
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
        private bool _isAiming;

        public bool IsCoreOut => _isCoreOut;
        public bool IsAiming => _isAiming;
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
            Controls.OnCoreReturn += ReturnCore;
            Controls.OnTeleportToCore += TeleportToCore;
            Controls.OnStartCoreAim += StartAiming;
            OnCoreShoot += () =>
            {
                _coreInPlayer.SetActive(false);
            };
            OnCoreCollect += () => _coreInPlayer.SetActive(true);
            Controls.OnCancelCoreAim += InterruptAiming;
        }

        private void StartAiming() => StartCoroutine(Aiming());

        private IEnumerator Aiming()
        {
            if (IsCoreOut)
                yield break;

            _isAiming = true;
            Marker.Animator.SetBool("Aiming", true);
            Movement.AddSpeedModifier("CoreAiming", 0.3f);
            _aimLine = Instantiate(_aimLine_prefab, transform.position, Quaternion.identity).transform;
            bool endAiming = false;
            
            Controls.OnEndCoreAim += EndAiming;
            while (!endAiming)
            {
                _aimLine.position = transform.position;
                _aimLine.rotation = Quaternion.Euler(0, -Controls.RelativeAimVector.XZtoXY().ToDegrees(), 0);
                yield return null;
            }
            Controls.OnEndCoreAim -= EndAiming;

            _projectile = Instantiate(_coreProjectilePrefab, transform.position, Quaternion.identity)
                .GetComponent<CoreProjectile>();
            
            _projectile.Init(Controls.RelativeAimVector);
            _projectile.OnCoreDestroy += () =>
            {
                _isCoreOut = false;
                OnCoreCollect?.Invoke();
            };
            _isCoreOut = true;
            OnCoreShoot?.Invoke();
            
            InterruptAiming();
            
            void EndAiming() => endAiming = true;
        }

        public void InterruptAiming()
        {
            _isAiming = false;
            Movement.RemoveSpeedModifier("CoreAiming");
            Marker.Animator.SetBool("Aiming", false);
            if (_aimLine != null)
                _aimLine.gameObject.SelfDestruct();
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