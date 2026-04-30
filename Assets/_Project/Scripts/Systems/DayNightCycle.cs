using UnityEngine;

namespace LastLight.Systems
{
    /// <summary>
    /// Controls the day/night cycle by rotating
    /// the directional light and blending ambient colors.
    /// </summary>
    public class DayNightCycle : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private DayNightData dayNightData;

        [Header("Scene References")]
        [SerializeField] private Light directionalLight;

        [Header("Sun Settings")]
        [SerializeField] private Gradient sunColor;
        [SerializeField] private AnimationCurve sunIntensity;

        [Header("Ambient Settings")]
        [SerializeField] private Gradient ambientColor;

        private float _timer = 0f;
        private bool _isDay = true;

        private void Awake()
        {
            ValidateReferences();
        }

        private void Update()
        {
            if (dayNightData == null || directionalLight == null) return;

            AdvanceTime();
            ApplyLighting();
        }

        private void AdvanceTime()
        {
            float cycleDuration = _isDay
                ? dayNightData.dayDuration
                : dayNightData.nightDuration;

            _timer += Time.deltaTime;

            dayNightData.currentTimeNormalized = Mathf.Clamp01(_timer / cycleDuration);

            if (_timer >= cycleDuration)
            {
                _timer = 0f;
                _isDay = !_isDay;
                dayNightData.currentTimeOfDay = _isDay ? TimeOfDay.Day : TimeOfDay.Night;

                if (_isDay) GameEvents.TriggerDayStarted();
                else GameEvents.TriggerNightStarted();

                Debug.Log($"[DayNight] Switched to: {dayNightData.currentTimeOfDay}");
            }
        }

        private void ApplyLighting()
        {
            float t = dayNightData.currentTimeNormalized;

            // Rotate sun: 0° at dawn → 180° at dusk
            float sunAngle = _isDay
                ? Mathf.Lerp(0f, 180f, t)
                : Mathf.Lerp(180f, 360f, t);

            directionalLight.transform.rotation =
                Quaternion.Euler(sunAngle, -30f, 0f);

            // Apply sun color & intensity
            directionalLight.color = sunColor.Evaluate(t);
            directionalLight.intensity = sunIntensity.Evaluate(t);

            // Apply ambient color
            RenderSettings.ambientLight = ambientColor.Evaluate(t);
        }

        private void ValidateReferences()
        {
            if (dayNightData == null)
                Debug.LogError("[DayNightCycle] DayNightData is not assigned.");
            if (directionalLight == null)
                Debug.LogError("[DayNightCycle] Directional Light is not assigned.");
        }
    }
}