using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LastLight.Systems;

namespace LastLight.UI
{
    /// <summary>
    /// Displays a single recipe in the crafting UI.
    /// </summary>
    public class CraftingSlotUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI ingredientsText;
        [SerializeField] private Button craftButton;
        [SerializeField] private Image buttonImage;

        [Header("Colors")]
        [SerializeField] private Color canCraftColor = new Color(0.2f, 0.7f, 0.2f);
        [SerializeField] private Color cannotCraftColor = new Color(0.5f, 0.5f, 0.5f);

        private CraftingRecipe _recipe;
        private System.Action<CraftingRecipe> _onCraft;

        public void Setup(CraftingRecipe recipe, System.Action<CraftingRecipe> onCraft)
        {
            _recipe = recipe;
            _onCraft = onCraft;

            if (nameText != null)
                nameText.text = recipe.recipeName;

            if (iconImage != null && recipe.recipeIcon != null)
                iconImage.sprite = recipe.recipeIcon;

            if (ingredientsText != null)
                ingredientsText.text = BuildIngredientsText(recipe);

            if (craftButton != null)
                craftButton.onClick.AddListener(() => _onCraft?.Invoke(_recipe));
        }

        public void UpdateCraftability(bool canCraft)
        {
            if (craftButton != null)
                craftButton.interactable = canCraft;

            if (buttonImage != null)
                buttonImage.color = canCraft ? canCraftColor : cannotCraftColor;
        }

        private string BuildIngredientsText(CraftingRecipe recipe)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (CraftingIngredient ingredient in recipe.ingredients)
                sb.AppendLine($"{ingredient.resourceType} x{ingredient.amount}");

            return sb.ToString().TrimEnd();
        }
    }
}