using UnityEngine;
using LastLight.Systems;

namespace LastLight.Enemy
{
    [RequireComponent(typeof(EnemyMovement))]
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [Header("Data")]
        [SerializeField] private EnemyData enemyData;

        // No longer serialized — found automatically at runtime
        private Transform _player;
        private EnemyState _currentState = EnemyState.Idle;
        private EnemyMovement _movement;
        private float _attackTimer = 0f;
        private float _currentHealth;

        public EnemyState CurrentState => _currentState;

        private void Awake()
        {
            _movement = GetComponent<EnemyMovement>();
            _currentHealth = enemyData.maxHealth;
            FindPlayer();
        }

        private void FindPlayer()
        {
            GameObject playerObj = GameObject.FindWithTag("Player");

            if (playerObj != null)
                _player = playerObj.transform;
            else
                Debug.LogError("[EnemyController] Player not found. Make sure Player tag is set.");
        }

        private void Update()
        {
            if (enemyData == null || _player == null) return;

            UpdateState();
            HandleState();

            _attackTimer -= Time.deltaTime;
        }

        private void UpdateState()
        {
            float distanceToPlayer = Vector3.Distance(
                transform.position,
                _player.position
            );

            if (distanceToPlayer <= enemyData.attackRange)
                _currentState = EnemyState.Attack;
            else if (distanceToPlayer <= enemyData.detectionRange)
                _currentState = EnemyState.Chase;
            else
                _currentState = EnemyState.Idle;
        }

        private void HandleState()
        {
            switch (_currentState)
            {
                case EnemyState.Idle:
                    _movement.Stop();
                    break;

                case EnemyState.Chase:
                    _movement.MoveToward(_player.position);
                    break;

                case EnemyState.Attack:
                    _movement.Stop();
                    TryAttack();
                    break;
            }
        }

        private void TryAttack()
        {
            if (_attackTimer > 0f) return;

            _attackTimer = enemyData.attackCooldown;
            Debug.Log($"[Enemy] {gameObject.name} attacks for {enemyData.damage} damage!");
        }

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            Debug.Log($"[Enemy] {gameObject.name} took {amount} damage. HP: {_currentHealth}");

            if (_currentHealth <= 0f)
                Die();
        }

        private void Die()
        {
            Debug.Log($"[Enemy] {gameObject.name} died.");
            gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (enemyData == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyData.detectionRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
        }
#endif
    }
}