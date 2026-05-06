using UnityEngine;
using UnityEngine.UI;
using LastLight.Core;

namespace LastLight.UI
{
    /// <summary>
    /// Displays torch fuel bar — event driven.
    /// </summary>
    public class TorchLightUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image fillImage;
        [SerializeField] private GameObject torchUIContainer;

        [Header("Color Settings")]
        [SerializeField] private Color fullColor = new Color(1f, 0.75f, 0.2f);
        [SerializeField] private Color lowColor = new Color(0.8f, 0.3f, 0.1f);
        [SerializeField] private float lowThreshold = 0.3f;

        private void Awake()
        {
            // Setup fill image
            if (fillImage != null)
            {
                Texture2D tex = new Texture2D(1, 1);
                tex.SetPixel(0, 0, Color.white);
                tex.Apply();

                fillImage.sprite = Sprite.Create(
                    tex,
                    new Rect(0, 0, 1, 1),
                    new Vector2(0.5f, 0.5f)
                );

                fillImage.type = Image.Type.Filled;
                fillImage.fillMethod = Image.FillMethod.Horizontal;
                fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
                fillImage.color = fullColor;
            }
        }

        private void OnEnable()
        {
            Debug.Log("[TorchLightUI] Subscribing to light events.");
            GameEvents.OnLightFuelChanged += UpdateBar;
            GameEvents.OnLightExtinguished += OnExtinguished;
            GameEvents.OnLightIgnited += OnIgnited;
        }

        private void OnDisable()
        {
            Debug.Log("[TorchLightUI] Unsubscribing from light events.");
            GameEvents.OnLightFuelChanged -= UpdateBar;
            GameEvents.OnLightExtinguished -= OnExtinguished;
            GameEvents.OnLightIgnited -= OnIgnited;
        }

        private void UpdateBar(float percent)
        {
            if (fillImage == null) return;

            fillImage.fillAmount = percent;
            fillImage.color = percent <= lowThreshold ? lowColor : fullColor;
        }

        private void OnIgnited()
        {
            Debug.Log("[TorchLightUI] OnIgnited received.");
            if (torchUIContainer != null)
                torchUIContainer.SetActive(true);
        }

        private void OnExtinguished()
        {
            Debug.Log("[TorchLightUI] OnExtinguished received.");
            if (torchUIContainer != null)
                torchUIContainer.SetActive(false);
        }
    }
}