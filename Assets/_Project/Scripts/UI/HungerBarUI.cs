using UnityEngine;
using UnityEngine.UI;
using LastLight.Systems;
using LastLight.Core;

namespace LastLight.UI
{
    public class HungerBarUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image fillImage;

        [Header("Color Settings")]
        [SerializeField] private Color fullColor = new Color(0.86f, 0.55f, 0.12f);
        [SerializeField] private Color lowColor = new Color(0.8f, 0.1f, 0.1f);
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
            GameEvents.OnHungerChanged += UpdateBar;
        }

        private void OnDisable()
        {
            GameEvents.OnHungerChanged -= UpdateBar;
        }

        private void UpdateBar(float percent)
        {
            if (fillImage == null) return;

            fillImage.fillAmount = percent;
            fillImage.color = percent <= lowThreshold ? lowColor : fullColor;
        }
    }
}