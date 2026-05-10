using UnityEngine;
using LastLight.Systems;
using LastLight.Core;

namespace LastLight.Player
{
    /// <summary>
    /// Drives the Player Animator based on
    /// movement, gathering, attacking and damage state.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int SpeedHash =
            Animator.StringToHash("Speed");
        private static readonly int IsSprintingHash =
            Animator.StringToHash("IsSprinting");
        private static readonly int IsGatheringHash =
            Animator.StringToHash("IsGathering");
        private static readonly int IsAttackingHash =
            Animator.StringToHash("IsAttacking");
        private static readonly int TakeDamageHash =
            Animator.StringToHash("TakeDamage");

        private Animator _animator;
        private PlayerMovement _movement;
        private PlayerStamina _stamina;
        private Rigidbody _rb;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _movement = GetComponent<PlayerMovement>();
            _stamina = GetComponent<PlayerStamina>();
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            GameEvents.OnPlayerDamaged += OnPlayerDamaged;
        }

        private void OnDisable()
        {
            GameEvents.OnPlayerDamaged -= OnPlayerDamaged;
        }

        private void Update()
        {
            UpdateMovementParams();
        }

        private void UpdateMovementParams()
        {
            float speed = new Vector3(
                _rb.linearVelocity.x,
                0f,
                _rb.linearVelocity.z
            ).magnitude;

            _animator.SetFloat(SpeedHash, speed);

            if (_stamina != null)
                _animator.SetBool(IsSprintingHash, _stamina.IsSprinting);
        }

        public void SetGathering(bool isGathering)
        {
            _animator.SetBool(IsGatheringHash, isGathering);
        }

        public void SetAttacking(bool isAttacking)
        {
            _animator.SetBool(IsAttackingHash, isAttacking);
        }

        private void OnPlayerDamaged(float amount)
        {
            _animator.SetTrigger(TakeDamageHash);
        }
    }
}