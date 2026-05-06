using UnityEngine;

namespace LastLight.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerStamina))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private PlayerData playerData;

        private Rigidbody _rb;
        private PlayerStamina _stamina;
        private Vector3 _inputDirection;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _stamina = GetComponent<PlayerStamina>();
        }

        private void Update()
        {
            GatherInput();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void GatherInput()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            _inputDirection = new Vector3(x, 0f, z).normalized;
        }

        private void Move()
        {
            float speed = playerData.moveSpeed;

            if (_stamina != null && _stamina.IsSprinting)
                speed *= _stamina.Data.sprintMultiplier;

            Vector3 targetVelocity = _inputDirection * speed;
            targetVelocity.y = _rb.linearVelocity.y;
            _rb.linearVelocity = targetVelocity;
        }
    }
}