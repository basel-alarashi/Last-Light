using UnityEngine;

namespace LastLight.Core
{
    /// <summary>
    /// Manages map boundaries and world settings.
    /// Placeholder for future map generation logic.
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        [Header("Map Settings")]
        [SerializeField] private float mapWidth = 200f;
        [SerializeField] private float mapHeight = 200f;

        public float MapWidth => mapWidth;
        public float MapHeight => mapHeight;

        private void Awake()
        {
            ValidateMapSettings();
        }

        private void ValidateMapSettings()
        {
            if (mapWidth <= 0 || mapHeight <= 0)
            {
                Debug.LogError("[MapManager] Invalid map dimensions.");
            }
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                transform.position,
                new Vector3(mapWidth, 1f, mapHeight)
            );
        }
        #endif
    }
}