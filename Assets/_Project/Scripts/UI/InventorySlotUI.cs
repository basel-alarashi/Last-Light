using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LastLight.Systems;

namespace LastLight.UI
{
    /// <summary>
    /// Represents a single inventory slot showing
    /// a resource icon and its current amount.
    /// </summary>
    public class InventorySlotUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private GameObject container;

        public ResourceType SlotType { get; private set; }

        public void Setup(ResourceType type, Sprite icon)
        {
            SlotType = type;

            if (iconImage != null && icon != null)
                iconImage.sprite = icon;

            UpdateAmount(0);
        }

        public void UpdateAmount(int amount)
        {
            if (amount <= 0)
            {
                container.SetActive(false);
                return;
            }

            container.SetActive(true);

            if (amountText != null)
                amountText.text = $"x{amount}";
        }
    }
}