using UnityEngine;
using LastLight.Systems;
using System.Collections;

namespace LastLight.Player
{
    /// <summary>
    /// Handles player melee attack input, detection and damage.
    /// </summary>
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private WeaponData weaponData;

        [Header("Attack Settings")]
        [SerializeField] private Transform attackOrigin;
        [SerializeField] private LayerMask enemyLayerMask;

        private float _attackTimer = 0f;
        private bool _canAttack => _attackTimer <= 0f;
        private PlayerAnimator _playerAnimator;

        private void Awake()
        {
            _playerAnimator = GetComponent<PlayerAnimator>();
        }

        private void Update()
        {
            if (_attackTimer > 0f)
                _attackTimer -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && _canAttack)
                TryAttack();
        }

        private void TryAttack()
        {
            _attackTimer = weaponData.attackCooldown;
            _playerAnimator?.SetAttacking(true);

            Collider[] hits = Physics.OverlapSphere(
                attackOrigin.position,
                weaponData.attackRange,
                enemyLayerMask
            );

            if (hits.Length == 0)
            {
                Debug.Log("[Attack] Swung but hit nothing.");
                _playerAnimator?.SetAttacking(false);
                return;
            }

            foreach (Collider hit in hits)
            {
                IDamageable damageable = hit.GetComponent<IDamageable>();
                if (damageable == null) continue;

                damageable.TakeDamage(weaponData.damage);
                ApplyKnockback(hit);
            }

            // Reset after cooldown
            StartCoroutine(ResetAttackAnimation());
        }

        private IEnumerator ResetAttackAnimation()
        {
            yield return new WaitForSeconds(weaponData.attackCooldown * 0.5f);
            _playerAnimator?.SetAttacking(false);
        }

        private void ApplyKnockback(Collider hit)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb == null) return;

            Vector3 direction = (hit.transform.position - transform.position).normalized;
            rb.AddForce(direction * weaponData.knockbackForce, ForceMode.Impulse);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (weaponData == null || attackOrigin == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackOrigin.position, weaponData.attackRange);
        }
#endif
    }
}