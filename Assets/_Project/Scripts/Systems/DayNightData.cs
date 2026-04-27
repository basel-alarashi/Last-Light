using UnityEngine;

namespace LastLight.Systems
{
    public enum TimeOfDay { Day, Night }

    [CreateAssetMenu(fileName = "DayNightData", menuName = "LastLight/DayNight Data")]
    public class DayNightData : ScriptableObject
    {
        [Header("Time Settings")]
        public float dayDuration = 60f;       // seconds per full day
        public float nightDuration = 40f;     // seconds per full night

        [Header("Runtime State")]
        [Range(0f, 1f)]
        public float currentTimeNormalized = 0f;  // 0 = dawn, 0.5 = dusk, 1 = next dawn
        public TimeOfDay currentTimeOfDay = TimeOfDay.Day;

        public bool IsNight => currentTimeOfDay == TimeOfDay.Night;

        private void OnDisable()
        {
            currentTimeNormalized = 0f;
            currentTimeOfDay = TimeOfDay.Day;
        }
    }
}