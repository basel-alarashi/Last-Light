using UnityEngine;
using LastLight.Systems;
using LastLight.Player;

namespace LastLight.Core
{
    /// <summary>
    /// Central access point for all game systems.
    /// Uses a simple singleton pattern — one instance per scene.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Player Data")]
        [SerializeField] private HungerData hungerData;
        [SerializeField] private InventoryData inventoryData;

        [Header("World Data")]
        [SerializeField] private DayNightData dayNightData;

        // Public accessors
        public HungerData HungerData => hungerData;
        public InventoryData InventoryData => inventoryData;
        public DayNightData DayNightData => dayNightData;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            ValidateReferences();
        }

        private void ValidateReferences()
        {
            if (hungerData == null)
                Debug.LogError("[GameManager] HungerData is not assigned.");
            if (inventoryData == null)
                Debug.LogError("[GameManager] InventoryData is not assigned.");
            if (dayNightData == null)
                Debug.LogError("[GameManager] DayNightData is not assigned.");
        }
    }
}