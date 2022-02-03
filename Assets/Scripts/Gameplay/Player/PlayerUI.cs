using Player;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerUI : PlayerComponent
    {
        private PlayerUICluster _cluster;
        
        [Inject]
        private void Construct(PlayerUICluster cluster)
        {
            _cluster = cluster;
            Inventory.OnInventoryUpdated += UpdateInventory;
        }

        private void FixedUpdate()
        {
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