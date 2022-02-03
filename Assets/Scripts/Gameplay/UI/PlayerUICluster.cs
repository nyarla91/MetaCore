using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUICluster : MonoBehaviour
    {
        [SerializeField] private Image _teleportCooldown;
        [SerializeField] private TextMeshProUGUI _teleportCooldownValue;
        [SerializeField] private Image _repairPack;
        [SerializeField] private TextMeshProUGUI _repairPackAmmount;
        [SerializeField] private Image _immortalityModule;
        [SerializeField] private TextMeshProUGUI _immortalityModuleAmmount;
        [SerializeField] private Image _coreCharger;
        [SerializeField] private TextMeshProUGUI _coreChargerAmmount;
        [SerializeField] private Image _chronoBooster;
        [SerializeField] private TextMeshProUGUI _chronoBoosterAmmount;

        public Image TeleportCooldown => _teleportCooldown;
        public TextMeshProUGUI TeleportCooldownValue => _teleportCooldownValue;

        public Image RepairPack => _repairPack;
        public TextMeshProUGUI RepairPackAmmount => _repairPackAmmount;
        public Image ImmortalityModule => _immortalityModule;
        public TextMeshProUGUI ImmortalityModuleAmmount => _immortalityModuleAmmount;
        public Image CoreCharger => _coreCharger;
        public TextMeshProUGUI CoreChargerAmmount => _coreChargerAmmount;
        public Image ChronoBooster => _chronoBooster;
        public TextMeshProUGUI ChronoBoosterAmmount => _chronoBoosterAmmount;
    }
}