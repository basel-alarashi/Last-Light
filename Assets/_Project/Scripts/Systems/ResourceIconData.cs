using System;
using UnityEngine;

namespace LastLight.Systems
{
    [CreateAssetMenu(fileName = "ResourceIconData", menuName = "LastLight/Resource Icon Data")]
    public class ResourceIconData : ScriptableObject
    {
        [Serializable]
        public struct ResourceIcon
        {
            public ResourceType type;
            public Sprite icon;
        }

        [SerializeField] private ResourceIcon[] icons;

        public Sprite GetIcon(ResourceType type)
        {
            foreach (ResourceIcon entry in icons)
            {
                if (entry.type == type)
                    return entry.icon;
            }

            Debug.LogWarning($"[ResourceIconData] No icon found for {type}");
            return null;
        }
    }
}