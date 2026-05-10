using System;
using UnityEngine;

namespace LastLight.Systems
{
    [Serializable]
    public struct CraftingIngredient
    {
        public ResourceType resourceType;
        public int amount;
    }
}