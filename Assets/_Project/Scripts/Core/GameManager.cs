using UnityEngine;
using LastLight.Systems;
using LastLight.Player;

namespace LastLight.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Player Data")]
        [SerializeField] private PlayerData playerData;
        [SerializeField] private HungerData hungerData;
        [SerializeField] private InventoryData inventoryData;
        [SerializeField] private StaminaData staminaData;
        [SerializeField] private LightData lightData;
        public StaminaData StaminaData => staminaData;
        public LightData LightData => lightData;

        [Header("World Data")]
        [SerializeField] private DayNightData dayNightData;
        [SerializeField] private GameSettings gameSettings;

        // Public accessors
        public PlayerData PlayerData => playerData;
        public HungerData HungerData => hungerData;
        public InventoryData InventoryData => inventoryData;
        public DayNightData DayNightData => dayNightData;
        public GameSettings GameSettings => gameSettings;

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
            if (playerData == null)
                Debug.LogError("[GameManager] PlayerData is not assigned.");
            if (hungerData == null)
                Debug.LogError("[GameManager] HungerData is not assigned.");
            if (inventoryData == null)
                Debug.LogError("[GameManager] InventoryData is not assigned.");
            if (dayNightData == null)
                Debug.LogError("[GameManager] DayNightData is not assigned.");
            if (gameSettings == null)
                Debug.LogError("[GameManager] GameSettings is not assigned.");
        }
    }
}