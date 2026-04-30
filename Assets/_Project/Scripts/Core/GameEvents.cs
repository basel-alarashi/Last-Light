using System;
using LastLight.Systems;

namespace LastLight.Core
{
    /// <summary>
    /// Central static event bus for decoupled system communication.
    /// </summary>
    public static class GameEvents
    {
        // Hunger
        public static event Action<float> OnHungerChanged;
        public static event Action OnPlayerStarving;

        // Inventory
        public static event Action<ResourceType, int> OnInventoryChanged;

        // Combat
        public static event Action<float> OnPlayerDamaged;
        public static event Action OnPlayerDied;
        public static event Action OnEnemyDied;

        // World
        public static event Action OnDayStarted;
        public static event Action OnNightStarted;

        // --- Invokers ---

        public static void TriggerHungerChanged(float percent)
            => OnHungerChanged?.Invoke(percent);

        public static void TriggerPlayerStarving()
            => OnPlayerStarving?.Invoke();

        public static void TriggerInventoryChanged(ResourceType type, int amount)
            => OnInventoryChanged?.Invoke(type, amount);

        public static void TriggerPlayerDamaged(float amount)
            => OnPlayerDamaged?.Invoke(amount);

        public static void TriggerPlayerDied()
            => OnPlayerDied?.Invoke();

        public static void TriggerEnemyDied()
            => OnEnemyDied?.Invoke();

        public static void TriggerDayStarted()
            => OnDayStarted?.Invoke();

        public static void TriggerNightStarted()
            => OnNightStarted?.Invoke();
    }
}