using UnityEngine;
using UnityEngine.UI;
using LastLight.Systems;
using LastLight.Core;

namespace LastLight.UI
{
    /// <summary>
    /// Manages the crafting menu UI.
    /// Opens/closes with C key.
    /// Updates slot craftability on inventory change.
    /// </summary>
    public class CraftingUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CraftingSystem craftingSystem;
        [SerializeField] private InventoryData inventoryData;

        [Header("UI References")]
        [SerializeField] private GameObject craftingPanel;
        [SerializeField] private Transform slotsContainer;
        [SerializeField] private GameObject slotPrefab;

        private CraftingSlotUI[] _slots;
        private bool _isOpen = false;

        private void Awake()
        {
            craftingPanel.SetActive(false);
            InitializeSlots();
        }

        private void OnEnable()
        {
            GameEvents.OnInventoryChanged += OnInventoryChanged;
            GameEvents.OnItemCrafted += OnItemCrafted;
        }

        private void OnDisable()
        {
            GameEvents.OnInventoryChanged -= OnInventoryChanged;
            GameEvents.OnItemCrafted -= OnItemCrafted;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                ToggleCraftingMenu();
        }

        private void ToggleCraftingMenu()
        {
            _isOpen = !_isOpen;
            craftingPanel.SetActive(_isOpen);
            GameEvents.TriggerCraftingMenuToggled(_isOpen);
            UpdateAllSlotCraftability();
        }

        private void InitializeSlots()
        {
            CraftingRecipe[] recipes = craftingSystem.AvailableRecipes;
            _slots = new CraftingSlotUI[recipes.Length];

            for (int i = 0; i < recipes.Length; i++)
            {
                GameObject slotGO = Instantiate(slotPrefab, slotsContainer);
                CraftingSlotUI slot = slotGO.GetComponent<CraftingSlotUI>();
                slot.Setup(recipes[i], OnCraftRequested);
                _slots[i] = slot;
            }
        }

        private void OnCraftRequested(CraftingRecipe recipe)
        {
            craftingSystem.TryCraft(recipe);
        }

        private void OnInventoryChanged(ResourceType type, int amount)
        {
            if (_isOpen)
                UpdateAllSlotCraftability();
        }

        private void OnItemCrafted(CraftableItemType item)
        {
            UpdateAllSlotCraftability();
        }

        private void UpdateAllSlotCraftability()
        {
            CraftingRecipe[] recipes = craftingSystem.AvailableRecipes;

            for (int i = 0; i < _slots.Length; i++)
                _slots[i].UpdateCraftability(recipes[i].CanCraft(inventoryData));
        }
    }
}