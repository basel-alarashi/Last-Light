using UnityEngine;
using UnityEngine.UI;
using LastLight.Core;

namespace LastLight.UI
{
    /// <summary>
    /// Displays stamina bar — event driven, no polling.
    /// </summary>
    public class StaminaBarUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image fillImage;

        [Header("Color Settings")]
        [SerializeField] private Color fullColor = new Color(0.2f, 0.6f, 1f);
        [SerializeField] private Color lowColor = new Color(0.1f, 0.2f, 0.8f);
        [SerializeField] private float lowThreshold = 0.3f;

        private void Awake()
        {
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
            GameEvents.OnStaminaChanged += UpdateBar;
        }

        private void OnDisable()
        {
            GameEvents.OnStaminaChanged -= UpdateBar;
        }

        private void UpdateBar(float percent)
        {
            if (fillImage == null) return;

            fillImage.fillAmount = percent;
            fillImage.color = percent <= lowThreshold ? lowColor : fullColor;
        }
    }
}