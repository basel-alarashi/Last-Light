using UnityEngine;

namespace LastLight.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;

        private Rigidbody _rb;
        private Vector3 _inputDirection;

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
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            _inputDirection = new Vector3(x, 0f, z).normalized;
        }

        private void Move()
        {
            Vector3 targetVelocity = _inputDirection * moveSpeed;
            targetVelocity.y = _rb.linearVelocity.y; // preserve gravity
            _rb.linearVelocity = targetVelocity;
        }
    }
}