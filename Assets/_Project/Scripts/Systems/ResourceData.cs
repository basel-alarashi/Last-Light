using UnityEngine;

namespace LastLight.Systems
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "LastLight/Resource Data")]
    public class ResourceData : ScriptableObject
    {
        [Header("Resource Info")]
        public ResourceType resourceType;
        public string resourceName;

        [Header("Gathering Settings")]
        public int minAmount = 1;
        public int maxAmount = 3;
        public float gatherTime = 1.5f;

        [Header("Node Settings")]
        public int maxCharges = 3;
    }
}