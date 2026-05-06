using UnityEngine;
using LastLight.Systems;

namespace LastLight.Player
{
    /// <summary>
    /// Manages stamina drain on sprint and recovery when idle.
    /// </summary>
    public class PlayerStamina : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private StaminaData staminaData;

        private bool _isSprinting = false;
        private float _recoveryDelayTimer = 0f;

        public bool IsSprinting => _isSprinting;
        public StaminaData Data => staminaData;

        private void Update()
        {
            if (staminaData == null) return;

            HandleSprint();
            HandleRecovery();
        }

        private void HandleSprint()
        {
            bool sprintInput = Input.GetKey(KeyCode.LeftShift);

            // Can only sprint if stamina available
            _isSprinting = sprintInput && !staminaData.IsDepleted;

            if (_isSprinting)
            {
                staminaData.Drain(staminaData.drainRate * Time.deltaTime);
                _recoveryDelayTimer = staminaData.recoveryDelay;
            }
        }

        private void HandleRecovery()
        {
            if (_isSprinting) return;

            if (_recoveryDelayTimer > 0f)
            {
                _recoveryDelayTimer -= Time.deltaTime;
                return;
            }

            if (staminaData.currentStamina < staminaData.maxStamina)
                staminaData.Recover(staminaData.recoveryRate * Time.deltaTime);
        }
    }
}