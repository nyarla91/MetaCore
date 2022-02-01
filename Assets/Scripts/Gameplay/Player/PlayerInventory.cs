using System;
using System.Collections;
using System.Collections.Generic;
using Graphics;
using UI;
using UnityEngine;
using World;

namespace Player
{
    public class PlayerInventory : PlayerComponent
    {
        public const float RepairAmmount = 25; 
        public const float ChronoBoostDuration = 8; 
        public const float ChronoBoostMultiplier = 3;
        public const float ImmortalityDuration = 5;

        [SerializeField] private GameObject _shieldPrefab;
        
        private Dictionary<ConsumableType, int> _inventory = new Dictionary<ConsumableType, int>();

        public Action OnInventoryUpdated;

        private Coroutine _chronoBoosterCoroutine;
        private Coroutine _immortalityModuleCoroutine;
        
        public int ConsumablesAmmountOfType(ConsumableType type)
        {
            if (_inventory.ContainsKey(type))
                return _inventory[type];
            return 0;
        }

        public void AddConsumableOfType(ConsumableType type)
        {
            if (_inventory.ContainsKey(type))
                _inventory[type]++;
            else
                _inventory.Add(type, 1);
            OnInventoryUpdated?.Invoke();
        }

        private void RemoveOneConsumableOfType(ConsumableType type)
        {
            if (_inventory.ContainsKey(type))
                _inventory[type]--;
            OnInventoryUpdated?.Invoke();
        }

        public void StoreInventoryToProgression() => Progression.Inventory = _inventory;

        private void Awake()
        {
            Input.OnRepairPackUse += UseRepairPack;
            Input.OnImmortalityModuleUse += UseImmortalityModule;
            Input.OnCoreChargeUse += UseCoreCharger;
            Input.OnChronoBoostUse += UseChronoBooster;
        }

        private void Start()
        {
            if (Progression.Inventory != null)
                _inventory = Progression.Inventory;
            OnInventoryUpdated?.Invoke();
        }

        private void UseRepairPack()
        {
            if (ConsumablesAmmountOfType(ConsumableType.RepairPack) < 1)
                return;
            if (Status.Health >= Status.MaxHealth)
            {
                MessageWindow.Instance.Show("! You are already at full health !", 3);
                return;
            }
            Status.RestoreHealth(RepairAmmount);
            RemoveOneConsumableOfType(ConsumableType.RepairPack);
        }

        private void UseCoreCharger()
        {
            if (ConsumablesAmmountOfType(ConsumableType.CoreCharger) < 1)
                return;
            if (!Core.IsCoreOut)
            {
                MessageWindow.Instance.Show("! The core is in you !", 2);
                return;
            }
            Core.Projectile.DamageArea.FullyCharge();
            RemoveOneConsumableOfType(ConsumableType.CoreCharger);
        }

        private void UseImmortalityModule()
        {
            if (ConsumablesAmmountOfType(ConsumableType.ImmortalityModule) < 1)
                return;
            if (_immortalityModuleCoroutine != null || Status.ImmortaliityTimeLeft > 0)
            {
                MessageWindow.Instance.Show("! You are already immortal !", 2);
                return;
            }
            _immortalityModuleCoroutine = StartCoroutine(ImmortalityModuleUsage());
            RemoveOneConsumableOfType(ConsumableType.ImmortalityModule);
        }

        private void UseChronoBooster()
        {
            if (ConsumablesAmmountOfType(ConsumableType.ChronoBooster) < 1)
                return;
            if (_chronoBoosterCoroutine != null && Core.TeleportCooldownLeft <= 0)
            {
                MessageWindow.Instance.Show("! You are already chono boosted and teleport has no cooldown !", 4);
                return;
            }
            _chronoBoosterCoroutine = StartCoroutine(ChronoBoosterUsage());
            RemoveOneConsumableOfType(ConsumableType.ChronoBooster);
        }

        private IEnumerator ChronoBoosterUsage()
        {
            Core.ForceRestoreTeleport();
            Movement.MaxSpeed *= ChronoBoostMultiplier;
            yield return new WaitForSeconds(ChronoBoostDuration);
            Movement.MaxSpeed /= ChronoBoostMultiplier;
            _chronoBoosterCoroutine = null;
        }

        private IEnumerator ImmortalityModuleUsage()
        {
            Status.ImmortaliityTimeLeft = ImmortalityDuration;
            ImmortalityShield shield = Instantiate(_shieldPrefab, transform.position, Quaternion.identity)
                .GetComponent<ImmortalityShield>();
            shield.Init(ImmortalityDuration - 2, ImmortalityDuration, transform);
            _immortalityModuleCoroutine = null;
            yield break;
        }
    }

    public enum ConsumableType
    {
        RepairPack,
        ImmortalityModule,
        CoreCharger,
        ChronoBooster
    }
}