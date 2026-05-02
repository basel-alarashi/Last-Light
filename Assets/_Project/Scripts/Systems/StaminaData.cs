using UnityEngine;
using LastLight.Core;

namespace LastLight.Systems
{
    [CreateAssetMenu(fileName = "StaminaData", menuName = "LastLight/Stamina Data")]
    public class StaminaData : ScriptableObject
    {
        [Header("Stamina Settings")]
        public float maxStamina = 100f;
        public float drainRate = 20f;       // per second while sprinting
        public float recoveryRate = 10f;    // per second while not sprinting
        public float recoveryDelay = 1.5f;  // seconds before recovery starts

        [Header("Sprint Settings")]
        public float sprintMultiplier = 1.8f;

        [Header("Runtime State")]
        [Range(0f, 100f)]
        public float currentStamina = 100f;

        public bool IsDepleted => currentStamina <= 0f;
        public float StaminaPercent => currentStamina / maxStamina;

        public void Drain(float amount)
        {
            currentStamina = Mathf.Clamp(currentStamina - amount, 0f, maxStamina);
            GameEvents.TriggerStaminaChanged(StaminaPercent);

            if (IsDepleted)
                GameEvents.TriggerStaminaDepleted();
        }

        public void Recover(float amount)
        {
            currentStamina = Mathf.Clamp(currentStamina + amount, 0f, maxStamina);
            GameEvents.TriggerStaminaChanged(StaminaPercent);
        }

        private void OnDisable()
        {
            currentStamina = maxStamina;
        }
    }
}