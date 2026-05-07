using UnityEngine;
using LastLight.Core;
using LastLight.Player;

namespace LastLight.Systems
{
    /// <summary>
    /// Handles crafting logic — validates recipes and
    /// consumes ingredients from inventory.
    /// </summary>
    public class CraftingSystem : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private InventoryData inventoryData;
        [SerializeField] private CraftingRecipe[] availableRecipes;

        [Header("References")]
        [SerializeField] private PlayerLight playerLight;

        public CraftingRecipe[] AvailableRecipes => availableRecipes;

        private void Awake()
        {
            if (inventoryData == null)
                Debug.LogError("[CraftingSystem] InventoryData not assigned.");
        }

        /// <summary>
        /// Attempts to craft a recipe.
        /// Returns true if successful.
        /// </summary>
        public bool TryCraft(CraftingRecipe recipe)
        {
            if (recipe == null) return false;

            if (!recipe.CanCraft(inventoryData))
            {
                Debug.Log($"[Crafting] Not enough ingredients for {recipe.recipeName}.");
                GameEvents.TriggerCraftingFailed();
                return false;
            }

            ConsumeIngredients(recipe);
            ApplyCraftedItem(recipe);

            Debug.Log($"[Crafting] Crafted {recipe.recipeName} x{recipe.outputAmount}.");
            GameEvents.TriggerItemCrafted(recipe.outputItem);
            return true;
        }

        private void ConsumeIngredients(CraftingRecipe recipe)
        {
            foreach (CraftingIngredient ingredient in recipe.ingredients)
                inventoryData.Remove(ingredient.resourceType, ingredient.amount);
        }

        private void ApplyCraftedItem(CraftingRecipe recipe)
        {
            switch (recipe.outputItem)
            {
                case CraftableItemType.Torch:
                    if (playerLight != null)
                        playerLight.AddFuel(50f);
                    break;

                case CraftableItemType.Bandage:
                    // TODO: Phase 3 — apply to player health
                    Debug.Log("[Crafting] Bandage crafted — ready for health system.");
                    break;

                case CraftableItemType.WoodSpear:
                    // TODO: Phase 3 — equip weapon
                    Debug.Log("[Crafting] Wood Spear crafted — ready for weapon system.");
                    break;

                case CraftableItemType.Campfire:
                    // TODO: Phase 3 — place campfire in world
                    Debug.Log("[Crafting] Campfire crafted — ready for placement system.");
                    break;

                default:
                    Debug.LogWarning($"[Crafting] No handler for {recipe.outputItem}.");
                    break;
            }
        }
    }
}