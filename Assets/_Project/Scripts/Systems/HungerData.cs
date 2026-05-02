using UnityEngine;
using LastLight.Core;

namespace LastLight.Systems
{
    [CreateAssetMenu(fileName = "HungerData", menuName = "LastLight/Hunger Data")]
    public class HungerData : ScriptableObject
    {
        [Header("Hunger Settings")]
        public float maxHunger = 100f;
        public float decayRate = 2f;        // units lost per second
        public float starvationThreshold = 0f;

        [Header("Runtime State")]
        [Range(0f, 100f)]
        public float currentHunger = 100f;

        public bool IsStarving => currentHunger <= starvationThreshold;
        public float HungerPercent => currentHunger / maxHunger;

        public void DecreaseHunger(float amount)
        {
            currentHunger = Mathf.Clamp(currentHunger - amount, 0f, maxHunger);
            GameEvents.TriggerHungerChanged(HungerPercent);
        }

        public void IncreaseHunger(float amount)
        {
            currentHunger = Mathf.Clamp(currentHunger + amount, 0f, maxHunger);
            GameEvents.TriggerHungerChanged(HungerPercent);
        }

        private void OnDisable()
        {
            currentHunger = maxHunger;
        }
    }
}