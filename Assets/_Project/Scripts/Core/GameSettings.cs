using UnityEngine;

namespace LastLight.Core
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "LastLight/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("World")]
        public float worldSize = 200f;

        [Header("Survival")]
        public float hungerDecayRate = 2f;
        public float staminaRecoveryRate = 10f;
        public float foodRestoreAmount = 30f;

        [Header("Combat")]
        public float globalDamageMultiplier = 1f;
        public float enemyDetectionMultiplier = 1f;

        [Header("Day Night")]
        public float dayDuration = 60f;
        public float nightDuration = 40f;
    }
}