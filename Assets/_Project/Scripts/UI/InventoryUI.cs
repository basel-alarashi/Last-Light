using System.Collections.Generic;
using UnityEngine;
using LastLight.Systems;
using LastLight.Core;

namespace LastLight.UI
{
    /// <summary>
    /// Manages all inventory slot UIs.
    /// Listens to GameEvents for updates — no polling.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private InventoryData inventoryData;
        [SerializeField] private ResourceIconData iconData;

        [Header("UI References")]
        [SerializeField] private Transform slotsContainer;
        [SerializeField] private GameObject slotPrefab;

        private Dictionary<ResourceType, InventorySlotUI> _slots = new();

        private void Awake()
        {
            InitializeSlots();
        }

        private void OnEnable()
        {
            GameEvents.OnInventoryChanged += OnInventoryChanged;
        }

        private void OnDisable()
        {
            GameEvents.OnInventoryChanged -= OnInventoryChanged;
        }

        private void InitializeSlots()
        {
            // Create a slot for each resource type
            foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
            {
                if (type == ResourceType.None) continue;

                GameObject slotGO = Instantiate(slotPrefab, slotsContainer);
                slotGO.name = $"Slot_{type}";

                InventorySlotUI slot = slotGO.GetComponent<InventorySlotUI>();
                slot.Setup(type, iconData.GetIcon(type));

                _slots[type] = slot;
            }
        }

        private void OnInventoryChanged(ResourceType type, int amount)
        {
            if (_slots.TryGetValue(type, out InventorySlotUI slot))
                slot.UpdateAmount(amount);
        }
    }
}