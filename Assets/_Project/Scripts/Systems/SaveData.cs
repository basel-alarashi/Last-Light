using System;
using System.Collections.Generic;

namespace LastLight.Systems
{
    /// <summary>
    /// Plain serializable container for all game state.
    /// No Unity types — pure C# for JSON compatibility.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        // Player
        public float playerHealth;
        public float playerHunger;
        public float playerStamina;
        public float playerLightFuel;

        // Player position
        public float playerX;
        public float playerY;
        public float playerZ;

        // Inventory
        public List<InventorySaveEntry> inventoryItems = new();

        // World
        public float currentTimeNormalized;
        public bool isNight;

        // Meta
        public string saveDate;
        public int playTimeSeconds;
    }

    [Serializable]
    public class InventorySaveEntry
    {
        public string resourceType;
        public int amount;
    }
}