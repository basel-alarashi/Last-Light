using UnityEngine;
using LastLight.Systems;

namespace LastLight.Player
{
    /// <summary>
    /// Drives hunger decay over time and handles starvation.
    /// </summary>
    public class PlayerHunger : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private HungerData hungerData;

        [Header("Starvation Settings")]
        [SerializeField] private float starvationDamageRate = 5f; // HP/sec (Phase 2)

        private void Update()
        {
            if (hungerData == null) return;

            DecayHunger();
            CheckStarvation();
        }

        private void DecayHunger()
        {
            hungerData.DecreaseHunger(hungerData.decayRate * Time.deltaTime);
        }

        private void CheckStarvation()
        {
            if (hungerData.IsStarving)
            {
                Debug.LogWarning("[Hunger] Player is starving!");
                // TODO Phase 2: apply HP damage here
            }
        }

        /// <summary>
        /// Called when player eats food from inventory.
        /// </summary>
        public void Eat(float amount)
        {
            hungerData.IncreaseHunger(amount);
            Debug.Log($"[Hunger] Ate {amount} hunger restored. Current: {hungerData.currentHunger:F1}");
        }
    }
}