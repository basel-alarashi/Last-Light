using System;
using System.IO;
using UnityEngine;
using LastLight.Core;
using LastLight.Player;

namespace LastLight.Systems
{
    /// <summary>
    /// Handles serialization and deserialization of game state.
    /// Uses JSON for human-readable saves.
    /// </summary>
    public class SaveLoadSystem : MonoBehaviour
    {
        private const string SAVE_FILE_NAME = "lastlight_save.json";

        private string SaveFilePath =>
            Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

        [Header("References")]
        [SerializeField] private PlayerData playerData;
        [SerializeField] private HungerData hungerData;
        [SerializeField] private StaminaData staminaData;
        [SerializeField] private LightData lightData;
        [SerializeField] private InventoryData inventoryData;
        [SerializeField] private DayNightData dayNightData;
        [SerializeField] private Transform playerTransform;

        private float _playTimeSeconds = 0f;

        private void Update()
        {
            _playTimeSeconds += Time.deltaTime;

            // Quick save with F5
            if (Input.GetKeyDown(KeyCode.F5))
                Save();

            // Quick load with F9
            if (Input.GetKeyDown(KeyCode.F9))
                Load();
        }

        public void Save()
        {
            try
            {
                SaveData data = BuildSaveData();
                string json = JsonUtility.ToJson(data, prettyPrint: true);
                File.WriteAllText(SaveFilePath, json);

                Debug.Log($"[SaveLoad] Game saved to: {SaveFilePath}");
                GameEvents.TriggerGameSaved();
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveLoad] Save failed: {e.Message}");
            }
        }

        public void Load()
        {
            try
            {
                if (!File.Exists(SaveFilePath))
                {
                    Debug.LogWarning("[SaveLoad] No save file found.");
                    return;
                }

                string json = File.ReadAllText(SaveFilePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                ApplySaveData(data);
                Debug.Log("[SaveLoad] Game loaded successfully.");
                GameEvents.TriggerGameLoaded();
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveLoad] Load failed: {e.Message}");
            }
        }

        public bool HasSaveFile()
        {
            return File.Exists(SaveFilePath);
        }

        public void DeleteSave()
        {
            if (File.Exists(SaveFilePath))
            {
                File.Delete(SaveFilePath);
                Debug.Log("[SaveLoad] Save file deleted.");
            }
        }

        private SaveData BuildSaveData()
        {
            SaveData data = new SaveData
            {
                // Player stats
                playerHealth = playerData != null ? playerData.currentHealth : 0f,
                playerHunger = hungerData != null ? hungerData.currentHunger : 0f,
                playerStamina = staminaData != null ? staminaData.currentStamina : 0f,
                playerLightFuel = lightData != null ? lightData.currentFuel : 0f,

                // Player position
                playerX = playerTransform != null ? playerTransform.position.x : 0f,
                playerY = playerTransform != null ? playerTransform.position.y : 0f,
                playerZ = playerTransform != null ? playerTransform.position.z : 0f,

                // World
                currentTimeNormalized = dayNightData != null
                    ? dayNightData.currentTimeNormalized : 0f,
                isNight = dayNightData != null && dayNightData.IsNight,

                // Meta
                saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                playTimeSeconds = (int)_playTimeSeconds
            };

            // Inventory
            if (inventoryData != null)
            {
                foreach (var item in inventoryData.Items)
                {
                    data.inventoryItems.Add(new InventorySaveEntry
                    {
                        resourceType = item.Key.ToString(),
                        amount = item.Value
                    });
                }
            }

            return data;
        }

        private void ApplySaveData(SaveData data)
        {
            // Player stats
            if (playerData != null)
            {
                playerData.currentHealth = data.playerHealth;
            }

            if (hungerData != null)
            {
                hungerData.currentHunger = data.playerHunger;
                GameEvents.TriggerHungerChanged(hungerData.HungerPercent);
            }

            if (staminaData != null)
            {
                staminaData.currentStamina = data.playerStamina;
                GameEvents.TriggerStaminaChanged(staminaData.StaminaPercent);
            }

            if (lightData != null)
            {
                lightData.currentFuel = data.playerLightFuel;
                GameEvents.TriggerLightFuelChanged(lightData.FuelPercent);
            }

            // Player position
            if (playerTransform != null)
            {
                playerTransform.position = new Vector3(
                    data.playerX,
                    data.playerY,
                    data.playerZ
                );
            }

            // Inventory
            if (inventoryData != null)
            {
                inventoryData.Clear();

                foreach (InventorySaveEntry entry in data.inventoryItems)
                {
                    if (Enum.TryParse(entry.resourceType, out ResourceType type))
                        inventoryData.Add(type, entry.amount);
                    else
                        Debug.LogWarning($"[SaveLoad] Unknown resource type: {entry.resourceType}");
                }
            }

            // World time
            if (dayNightData != null)
            {
                dayNightData.currentTimeNormalized = data.currentTimeNormalized;
                dayNightData.currentTimeOfDay = data.isNight
                    ? TimeOfDay.Night
                    : TimeOfDay.Day;
            }

            _playTimeSeconds = data.playTimeSeconds;
        }
    }
}