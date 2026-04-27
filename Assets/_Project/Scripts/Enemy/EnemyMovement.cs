using UnityEngine;

namespace LastLight.Enemy
{
    /// <summary>
    /// Handles enemy physical movement via Rigidbody.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private EnemyData enemyData;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void MoveToward(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0f;

            _rb.linearVelocity = new Vector3(
                direction.x * enemyData.moveSpeed,
                _rb.linearVelocity.y,
                direction.z * enemyData.moveSpeed
            );

            // Face the player
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);
        }

        public void Stop()
        {
            _rb.linearVelocity = new Vector3(
                0f,
                _rb.linearVelocity.y,
                0f
            );
        }
    }
}