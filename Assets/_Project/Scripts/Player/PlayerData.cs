using UnityEngine;
using LastLight.Core;

namespace LastLight.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "LastLight/Player Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement")]
        public float moveSpeed = 5f;

        [Header("Health")]
        public float maxHealth = 100f;
        public float currentHealth = 100f;

        [Header("Combat")]
        public float invincibilityDuration = 0.5f;

        public bool IsDead => currentHealth <= 0f;
        public float HealthPercent => currentHealth / maxHealth;

        public void TakeDamage(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
            GameEvents.TriggerPlayerDamaged(amount);

            if (IsDead)
                GameEvents.TriggerPlayerDied();
        }

        public void Heal(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        }

        private void OnDisable()
        {
            currentHealth = maxHealth;
        }
    }
}