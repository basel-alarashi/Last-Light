using UnityEngine;
using LastLight.Systems;
using LastLight.Core;
using LastLight.Player;

namespace LastLight.Enemy
{
    /// <summary>
    /// Enemy state machine: Idle, Patrol, Chase, Attack.
    /// Reacts to day/night and player torch light.
    /// </summary>
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyPatrol))]
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [Header("Data")]
        [SerializeField] private EnemyData enemyData;

        private Transform _player;
        private PlayerLight _playerLight;
        private DayNightData _dayNightData;

        private EnemyState _currentState = EnemyState.Idle;
        private EnemyMovement _movement;
        private EnemyPatrol _patrol;

        private float _attackTimer = 0f;
        private float _currentHealth;

        public EnemyState CurrentState => _currentState;

        private void Awake()
        {
            _movement = GetComponent<EnemyMovement>();
            _patrol = GetComponent<EnemyPatrol>();
            _currentHealth = enemyData.maxHealth;

            FindReferences();
        }

        private void FindReferences()
        {
            GameObject playerObj = GameObject.FindWithTag("Player");

            if (playerObj != null)
            {
                _player = playerObj.transform;
                _playerLight = playerObj.GetComponent<PlayerLight>();
            }
            else
            {
                Debug.LogError("[EnemyController] Player not found.");
            }

            if (GameManager.Instance != null)
                _dayNightData = GameManager.Instance.DayNightData;
            else
                Debug.LogWarning("[EnemyController] GameManager not found.");
        }

        private void Update()
        {
            if (enemyData == null || _player == null) return;

            UpdateState();
            HandleState();

            if (_attackTimer > 0f)
                _attackTimer -= Time.deltaTime;
        }

        private void UpdateState()
        {
            float distance = Vector3.Distance(transform.position, _player.position);
            float effectiveRange = GetEffectiveDetectionRange();

            if (distance <= enemyData.attackRange)
                _currentState = EnemyState.Attack;
            else if (distance <= effectiveRange)
                _currentState = EnemyState.Chase;
            else
                _currentState = EnemyState.Patrol;
        }

        private float GetEffectiveDetectionRange()
        {
            float range = enemyData.detectionRange;
            bool isNight = _dayNightData != null && _dayNightData.IsNight;
            bool torchOn = _playerLight != null && _playerLight.IsLightOn;

            // Night increases base detection range
            if (isNight)
                range += enemyData.nightDetectionBonus;

            // Torch light attracts enemies even more
            if (torchOn)
                range += enemyData.torchDetectionBonus;

            return range;
        }

        private void HandleState()
        {
            switch (_currentState)
            {
                case EnemyState.Idle:
                    _movement.Stop();
                    break;

                case EnemyState.Patrol:
                    _patrol.PatrolTick();
                    break;

                case EnemyState.Chase:
                    float speedMultiplier = ShouldApplyLightAttraction()
                        ? enemyData.lightAttractionSpeed
                        : 1f;
                    _movement.MoveToward(_player.position, speedMultiplier);
                    break;

                case EnemyState.Attack:
                    _movement.Stop();
                    TryAttack();
                    break;
            }
        }

        private bool ShouldApplyLightAttraction()
        {
            bool isNight = _dayNightData != null && _dayNightData.IsNight;
            bool torchOn = _playerLight != null && _playerLight.IsLightOn;
            return isNight && torchOn;
        }

        private void TryAttack()
        {
            if (_attackTimer > 0f) return;

            _attackTimer = enemyData.attackCooldown;
            Debug.Log($"[Enemy] {gameObject.name} attacks for {enemyData.damage} damage!");
            // TODO: call player health system
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
            GameEvents.TriggerEnemyDied();
            gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (enemyData == null) return;

            // Base detection range
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, enemyData.detectionRange);

            // Night detection range
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position,
                enemyData.detectionRange + enemyData.nightDetectionBonus);

            // Torch detection range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,
                enemyData.detectionRange + enemyData.nightDetectionBonus
                + enemyData.torchDetectionBonus);

            // Attack range
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
        }
#endif
    }
}