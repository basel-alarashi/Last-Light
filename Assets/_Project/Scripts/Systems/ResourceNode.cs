using UnityEngine;

namespace LastLight.Systems
{
    /// <summary>
    /// Represents an interactable resource node in the world.
    /// </summary>
    public class ResourceNode : MonoBehaviour, IGatherable
    {
        [Header("Resource Settings")]
        [SerializeField] private ResourceData resourceData;

        private int _remainingCharges;
        public bool IsDepleted => _remainingCharges <= 0;
        public ResourceData Data => resourceData;

        private void Awake()
        {
            if (resourceData == null)
            {
                Debug.LogError($"[ResourceNode] No ResourceData assigned on {gameObject.name}");
                return;
            }

            _remainingCharges = resourceData.maxCharges;
        }

        /// <summary>
        /// Attempts to gather from this node.
        /// Returns amount gathered, or 0 if depleted.
        /// </summary>
        public int Gather()
        {
            if (IsDepleted) return 0;

            int amount = Random.Range(resourceData.minAmount, resourceData.maxAmount + 1);
            _remainingCharges--;

            if (IsDepleted)
            {
                OnDepleted();
            }

            return amount;
        }

        private void OnDepleted()
        {
            Debug.Log($"[ResourceNode] {gameObject.name} is depleted.");
            gameObject.SetActive(false);
        }
    }
}