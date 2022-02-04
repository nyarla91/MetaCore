using System;
using System.Collections;
using Core;
using NyarlaEssentials;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerCore : PlayerComponent
    {
        [SerializeField] private GameObject _coreProjectilePrefab;
        [SerializeField] private GameObject _aimLinePrefab;
        [SerializeField] private float _teleportCooldown;
        [SerializeField] private GameObject _coreInPlayer;

        private Transform _aimLine;
        private CoreProjectile _projectile;
        private Timer _teleportCooldownTimer;
        private bool _isCoreOut;
        private bool _isAiming;
        private Coroutine _aimingCoroutine;

        public bool IsCoreOut => _isCoreOut;
        public bool IsAiming => _isAiming;
        public CoreProjectile Projectile => _projectile;
        public Timer TeleportCooldownTimer => _teleportCooldownTimer;
        
        public event Action OnCoreShoot;
        public event Action OnCoreCollect;
        public event Action OnCoreTeleport;

        public void ForceRestoreTeleport()
        {
            _teleportCooldownTimer.Stop();
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
            _teleportCooldownTimer = new Timer(this, _teleportCooldown, false, true);
        }

        private void StartAiming() => _aimingCoroutine = StartCoroutine(Aiming());

        private IEnumerator Aiming()
        {
            if (IsCoreOut)
                yield break;

            _isAiming = true;
            Marker.Animator.SetBool("Aiming", true);
            Movement.AddSpeedModifier("CoreAiming", 0.3f);
            _aimLine = Instantiate(_aimLinePrefab, transform.position, Quaternion.identity).transform;
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
            
            _aimingCoroutine?.StopThisCoroutine(this);
            _aimingCoroutine = null;
        }

        public void ReturnCore()
        {
            if (!_isCoreOut)
                return;

            _projectile.Collect();
        }

        private void TeleportToCore()
        {
            if (!_isCoreOut || !_teleportCooldownTimer.IsExpired)
                return;
            
            Rigidbody.position = _projectile.transform.position;
            _teleportCooldownTimer.Restart();
            _projectile.Collect();
            OnCoreTeleport?.Invoke();
        }
    }
}