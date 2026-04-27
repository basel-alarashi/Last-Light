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
    }
}