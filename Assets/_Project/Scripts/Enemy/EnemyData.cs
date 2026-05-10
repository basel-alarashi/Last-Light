using UnityEngine;

namespace LastLight.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "LastLight/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [Header("Stats")]
        public float maxHealth = 50f;
        public float moveSpeed = 3f;
        public float damage = 10f;

        [Header("Detection")]
        public float detectionRange = 8f;
        public float attackRange = 1.5f;
        public float attackCooldown = 1.5f;

        [Header("Light Reaction")]
        public float nightDetectionBonus = 4f;
        public float torchDetectionBonus = 6f;
        public float lightAttractionSpeed = 1.2f;

        [Header("Patrol Settings")]
        public float patrolRadius = 5f;
        public float patrolWaitTime = 2f;
    }
}