using UnityEngine;

namespace LastLight.Systems
{
    [CreateAssetMenu(fileName = "CraftingRecipe", menuName = "LastLight/Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        [Header("Recipe Info")]
        public string recipeName;
        public CraftableItemType outputItem;
        public int outputAmount = 1;
        public Sprite recipeIcon;

        [Header("Ingredients")]
        public CraftingIngredient[] ingredients;

        /// <summary>
        /// Checks if inventory has all required ingredients.
        /// </summary>
        public bool CanCraft(InventoryData inventory)
        {
            foreach (CraftingIngredient ingredient in ingredients)
            {
                if (inventory.GetAmount(ingredient.resourceType) < ingredient.amount)
                    return false;
            }
            return true;
        }
    }
}