using System.Collections;
using UnityEngine;
using TMPro;
using LastLight.Core;

namespace LastLight.UI
{
    /// <summary>
    /// Shows a brief "Game Saved" or "Game Loaded"
    /// notification on screen.
    /// </summary>
    public class SaveNotificationUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject notificationPanel;
        [SerializeField] private TextMeshProUGUI notificationText;

        [Header("Settings")]
        [SerializeField] private float displayDuration = 2f;

        private Coroutine _hideCoroutine;

        private void Awake()
        {
            if (notificationPanel != null)
                notificationPanel.SetActive(false);
        }

        private void OnEnable()
        {
            GameEvents.OnGameSaved += OnGameSaved;
            GameEvents.OnGameLoaded += OnGameLoaded;
        }

        private void OnDisable()
        {
            GameEvents.OnGameSaved -= OnGameSaved;
            GameEvents.OnGameLoaded -= OnGameLoaded;
        }

        private void OnGameSaved()
            => ShowNotification("💾 Game Saved");

        private void OnGameLoaded()
            => ShowNotification("✅ Game Loaded");

        private void ShowNotification(string message)
        {
            if (notificationText != null)
                notificationText.text = message;

            if (notificationPanel != null)
                notificationPanel.SetActive(true);

            if (_hideCoroutine != null)
                StopCoroutine(_hideCoroutine);

            _hideCoroutine = StartCoroutine(HideAfterDelay());
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(displayDuration);

            if (notificationPanel != null)
                notificationPanel.SetActive(false);
        }
    }
}