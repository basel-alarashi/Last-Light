using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using LastLight.Core;

namespace LastLight.Player
{
    /// <summary>
    /// Handles non-animation player feedback:
    /// damage flash, gather highlight, starvation pulse.
    /// </summary>
    public class PlayerFeedback : MonoBehaviour
    {
        [Header("Damage Flash")]
        [SerializeField] private Image damageFlashImage;
        [SerializeField] private Color damageFlashColor = new Color(1f, 0f, 0f, 0.3f);
        [SerializeField] private float damageFlashDuration = 0.2f;

        [Header("Starvation Pulse")]
        [SerializeField] private Image starvationPulseImage;
        [SerializeField] private Color starvationColor = new Color(1f, 0.5f, 0f, 0.2f);
        [SerializeField] private float pulseDuration = 1f;

        private Coroutine _flashCoroutine;
        private Coroutine _pulseCoroutine;

        private void Awake()
        {
            if (damageFlashImage != null)
                damageFlashImage.color = Color.clear;

            if (starvationPulseImage != null)
                starvationPulseImage.color = Color.clear;
        }

        private void OnEnable()
        {
            GameEvents.OnPlayerDamaged += OnPlayerDamaged;
            GameEvents.OnPlayerStarving += OnPlayerStarving;
        }

        private void OnDisable()
        {
            GameEvents.OnPlayerDamaged -= OnPlayerDamaged;
            GameEvents.OnPlayerStarving -= OnPlayerStarving;
        }

        private void OnPlayerDamaged(float amount)
        {
            if (_flashCoroutine != null)
                StopCoroutine(_flashCoroutine);

            _flashCoroutine = StartCoroutine(DamageFlashRoutine());
        }

        private void OnPlayerStarving()
        {
            if (_pulseCoroutine != null) return;
            _pulseCoroutine = StartCoroutine(StarvationPulseRoutine());
        }

        private IEnumerator DamageFlashRoutine()
        {
            if (damageFlashImage == null) yield break;

            damageFlashImage.color = damageFlashColor;
            yield return new WaitForSeconds(damageFlashDuration);

            float elapsed = 0f;
            while (elapsed < damageFlashDuration)
            {
                elapsed += Time.deltaTime;
                damageFlashImage.color = Color.Lerp(
                    damageFlashColor,
                    Color.clear,
                    elapsed / damageFlashDuration
                );
                yield return null;
            }

            damageFlashImage.color = Color.clear;
        }

        private IEnumerator StarvationPulseRoutine()
        {
            if (starvationPulseImage == null)
            {
                _pulseCoroutine = null;
                yield break;
            }

            float elapsed = 0f;
            while (elapsed < pulseDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.PingPong(elapsed, pulseDuration * 0.5f)
                    / (pulseDuration * 0.5f);
                starvationPulseImage.color = Color.Lerp(
                    Color.clear,
                    starvationColor,
                    t
                );
                yield return null;
            }

            starvationPulseImage.color = Color.clear;
            _pulseCoroutine = null;
        }
    }
}