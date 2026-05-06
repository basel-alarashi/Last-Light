using UnityEngine;
using LastLight.Core;

namespace LastLight.Systems
{
    [CreateAssetMenu(fileName = "LightData", menuName = "LastLight/Light Data")]
    public class LightData : ScriptableObject
    {
        [Header("Light Settings")]
        public float lightRadius = 8f;
        public float lightIntensity = 2f;
        public Color lightColor = new Color(1f, 0.75f, 0.4f);

        [Header("Fuel Settings")]
        public float maxFuel = 100f;
        public float fuelDrainRate = 3f;    // per second while active
        public float currentFuel = 100f;

        public bool HasFuel => currentFuel > 0f;
        public float FuelPercent => currentFuel / maxFuel;

        public void DrainFuel(float amount)
        {
            currentFuel = Mathf.Clamp(currentFuel - amount, 0f, maxFuel);
            GameEvents.TriggerLightFuelChanged(FuelPercent);

            if (!HasFuel)
                GameEvents.TriggerLightExtinguished();
        }

        public void AddFuel(float amount)
        {
            currentFuel = Mathf.Clamp(currentFuel + amount, 0f, maxFuel);
            GameEvents.TriggerLightFuelChanged(FuelPercent);
        }

        private void OnDisable()
        {
            currentFuel = maxFuel;
        }
    }
}