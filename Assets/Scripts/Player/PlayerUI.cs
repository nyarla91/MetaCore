using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Player
{
    public class PlayerUI : PlayerComponent
    {
        [SerializeField] private float _shieldScaleSpeed;

        private PlayerUICluster _cluster;

        private float _shieldsTargetT;
        private float _shieldsT;
        
        [Inject]
        private void Construct(PlayerUICluster cluster)
        {
            _cluster = cluster;
            Status.OnHealthPercentChanged += SetHealthBarLength;
            Status.OnShieldsPercentChanged += SetShieldBarLength;
            Core.OnCoreShoot += () => { _shieldsTargetT = 1; };
            Core.OnCoreCollect += () => { _shieldsTargetT = 0; };
            Inventory.OnInventoryUpdated += UpdateInventory;
        }
        
        private void SetHealthBarLength(float percent)
        {
            _cluster.HealthBarLine.rectTransform.localScale = new Vector3(percent, 1, 1);
        }
        private void SetShieldBarLength(float percent)
        {
            _cluster.ShieldBarLine.rectTransform.localScale = new Vector3(percent, 1, 1);
        }

        private void FixedUpdate()
        {
            _shieldsT = Mathf.Lerp(_shieldsT, _shieldsTargetT, _shieldScaleSpeed * Time.fixedDeltaTime);
            _cluster.ShieldBar.alpha = 1 - _shieldsT * 0.6f;
            _cluster.ShieldBar.GetComponent<RectTransform>().localScale =
                new Vector3(1 - _shieldsT * 0.2f, 1 - _shieldsT * 0.5f, 1);

            float teleportCooldownLeft = Core.TeleportCooldownLeft;
            float targetT = teleportCooldownLeft > 0 ? 0.6f : 1;
            float t = Mathf.Lerp(_cluster.TeleportCooldown.rectTransform.localScale.x, targetT, Time.fixedDeltaTime * 5);
            string teleportText = teleportCooldownLeft > 0 ? Mathf.CeilToInt(teleportCooldownLeft).ToString() : string.Empty;
            _cluster.TeleportCooldown.color = new Color(t, t, t, 1);
            _cluster.TeleportCooldown.rectTransform.localScale = new Vector3(t, t, t);
            _cluster.TeleportCooldownValue.text = teleportText;
        }

        private void UpdateInventory()
        {
            UpdateConsumable(_cluster.RepairPack, _cluster.RepairPackAmmount, ConsumableType.RepairPack);
            UpdateConsumable(_cluster.ImmortalityModule, _cluster.ImmortalityModuleAmmount, ConsumableType.ImmortalityModule);
            UpdateConsumable(_cluster.CoreCharger, _cluster.CoreChargerAmmount, ConsumableType.CoreCharger);
            UpdateConsumable(_cluster.ChronoBooster, _cluster.ChronoBoosterAmmount, ConsumableType.ChronoBooster);
        }

        private void UpdateConsumable(Image targetImage, TextMeshProUGUI targetText, ConsumableType type)
        {
            int ammount = Inventory.ConsumablesAmmountOfType(type);
            float rgba = ammount > 0 ? 1 : 0.5f;
            string ammountText = ammount > 0 ? ammount.ToString() : "";
            targetImage.color = new Color(rgba, rgba, rgba, rgba);
            targetText.text = ammountText;
        }
    }
}