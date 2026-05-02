using UnityEngine;
using LastLight.Player;

namespace LastLight.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private PlayerData playerData;

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
            Vector3 targetVelocity = _inputDirection * playerData.moveSpeed;
            targetVelocity.y = _rb.linearVelocity.y;
            _rb.linearVelocity = targetVelocity;
        }
    }
}