using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using LastLight.Core;

namespace LastLight.Systems
{
    /// <summary>
    /// Controls global darkness and vignette effects
    /// based on day/night cycle and torch state.
    /// </summary>
    public class DarknessController : MonoBehaviour
    {
        [Header("Volume Reference")]
        [SerializeField] private Volume globalVolume;

        [Header("Darkness Settings")]
        [SerializeField] private float dayAmbientIntensity = 1f;
        [SerializeField] private float nightAmbientIntensity = 0.05f;
        [SerializeField] private float transitionDuration = 3f;

        [Header("Vignette Settings")]
        [SerializeField] private float dayVignetteIntensity = 0.2f;
        [SerializeField] private float nightVignetteIntensity = 0.7f;
        [SerializeField] private float torchVignetteIntensity = 0.45f;
        [SerializeField] private Color vignetteColor = Color.black;

        [Header("Color Adjustment Settings")]
        [SerializeField] private float dayExposure = 0f;
        [SerializeField] private float nightExposure = -2f;

        private Vignette _vignette;
        private ColorAdjustments _colorAdjustments;
        private bool _isNight = false;
        private bool _isTorchOn = false;
        private Coroutine _transitionCoroutine;

        private void Awake()
        {
            InitializePostProcessing();
        }

        private void OnEnable()
        {
            GameEvents.OnNightStarted += OnNightStarted;
            GameEvents.OnDayStarted += OnDayStarted;
            GameEvents.OnLightIgnited += OnTorchIgnited;
            GameEvents.OnLightExtinguished += OnTorchExtinguished;
        }

        private void OnDisable()
        {
            GameEvents.OnNightStarted -= OnNightStarted;
            GameEvents.OnDayStarted -= OnDayStarted;
            GameEvents.OnLightIgnited -= OnTorchIgnited;
            GameEvents.OnLightExtinguished -= OnTorchExtinguished;
        }

        private void InitializePostProcessing()
        {
            if (globalVolume == null)
            {
                Debug.LogError("[DarknessController] Global Volume not assigned.");
                return;
            }

            globalVolume.profile.TryGet(out _vignette);
            globalVolume.profile.TryGet(out _colorAdjustments);

            if (_vignette == null)
                Debug.LogError("[DarknessController] Vignette override not found in Volume Profile.");
            if (_colorAdjustments == null)
                Debug.LogError("[DarknessController] ColorAdjustments override not found in Volume Profile.");

            // Set initial day state
            ApplyDaySettings(instant: true);
        }

        private void OnNightStarted()
        {
            _isNight = true;
            StartTransition();
        }

        private void OnDayStarted()
        {
            _isNight = false;
            _isTorchOn = false;
            StartTransition();
        }

        private void OnTorchIgnited()
        {
            _isTorchOn = true;
            StartTransition();
        }

        private void OnTorchExtinguished()
        {
            _isTorchOn = false;
            StartTransition();
        }

        private void StartTransition()
        {
            if (_transitionCoroutine != null)
                StopCoroutine(_transitionCoroutine);

            _transitionCoroutine = StartCoroutine(TransitionRoutine());
        }

        private IEnumerator TransitionRoutine()
        {
            float targetVignette = GetTargetVignetteIntensity();
            float targetExposure = _isNight ? nightExposure : dayExposure;
            float targetAmbient = _isNight ? nightAmbientIntensity : dayAmbientIntensity;

            float startVignette = _vignette != null
                ? _vignette.intensity.value : 0f;
            float startExposure = _colorAdjustments != null
                ? _colorAdjustments.postExposure.value : 0f;
            float startAmbient = RenderSettings.ambientIntensity;

            float elapsed = 0f;

            while (elapsed < transitionDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / transitionDuration);
                float smooth = Mathf.SmoothStep(0f, 1f, t);

                if (_vignette != null)
                {
                    _vignette.intensity.value = Mathf.Lerp(startVignette, targetVignette, smooth);
                    _vignette.color.value = vignetteColor;
                }

                if (_colorAdjustments != null)
                    _colorAdjustments.postExposure.value = Mathf.Lerp(startExposure, targetExposure, smooth);

                RenderSettings.ambientIntensity = Mathf.Lerp(startAmbient, targetAmbient, smooth);

                yield return null;
            }
        }

        private float GetTargetVignetteIntensity()
        {
            if (!_isNight) return dayVignetteIntensity;
            if (_isTorchOn) return torchVignetteIntensity;
            return nightVignetteIntensity;
        }

        private void ApplyDaySettings(bool instant = false)
        {
            if (instant)
            {
                if (_vignette != null)
                    _vignette.intensity.value = dayVignetteIntensity;
                if (_colorAdjustments != null)
                    _colorAdjustments.postExposure.value = dayExposure;

                RenderSettings.ambientIntensity = dayAmbientIntensity;
                return;
            }

            StartTransition();
        }
    }
}