using System.Collections;
using UnityEngine;

namespace LastLight.Enemy
{
    /// <summary>
    /// Handles enemy patrol behavior when idle.
    /// Picks random points within patrol radius and moves between them.
    /// </summary>
    public class EnemyPatrol : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;

        private Vector3 _originPoint;
        private Vector3 _currentTarget;
        private bool _isWaiting = false;
        private EnemyMovement _movement;

        public bool IsWaiting => _isWaiting;

        private void Awake()
        {
            _movement = GetComponent<EnemyMovement>();
            _originPoint = transform.position;
            PickNewTarget();
        }

        public void PatrolTick()
        {
            if (_isWaiting) return;

            float distanceToTarget = Vector3.Distance(
                transform.position,
                _currentTarget
            );

            if (distanceToTarget <= 0.5f)
                StartCoroutine(WaitAndPickTarget());
            else
                _movement.MoveToward(_currentTarget);
        }

        private IEnumerator WaitAndPickTarget()
        {
            _isWaiting = true;
            _movement.Stop();

            yield return new WaitForSeconds(enemyData.patrolWaitTime);

            PickNewTarget();
            _isWaiting = false;
        }

        private void PickNewTarget()
        {
            Vector2 randomCircle = Random.insideUnitCircle * enemyData.patrolRadius;
            _currentTarget = _originPoint + new Vector3(randomCircle.x, 0f, randomCircle.y);
        }
    }
}