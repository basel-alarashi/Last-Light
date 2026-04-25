using UnityEngine;

namespace LastLight.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;

        private Rigidbody _rb;
        private Vector2 _inputDirection;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
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
            _inputDirection = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ).normalized;
        }

        private void Move()
        {
            _rb.linearVelocity = _inputDirection * moveSpeed;
        }
    }
}