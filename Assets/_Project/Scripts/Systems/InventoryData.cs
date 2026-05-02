using System.Collections.Generic;
using UnityEngine;
using LastLight.Core;

namespace LastLight.Systems
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "LastLight/Inventory Data")]
    public class InventoryData : ScriptableObject
    {
        private Dictionary<ResourceType, int> _items = new();

        public IReadOnlyDictionary<ResourceType, int> Items => _items;

        public void Add(ResourceType type, int amount)
        {
            if (_items.ContainsKey(type))
                _items[type] += amount;
            else
                _items[type] = amount;

            GameEvents.TriggerInventoryChanged(type, _items[type]);
        }

        public bool Remove(ResourceType type, int amount)
        {
            if (!_items.ContainsKey(type) || _items[type] < amount)
            {
                Debug.Log($"[Inventory] Not enough {type}.");
                return false;
            }

            _items[type] -= amount;

            if (_items[type] <= 0)
                _items.Remove(type);

            GameEvents.TriggerInventoryChanged(type, GetAmount(type));
            return true;
        }

        public int GetAmount(ResourceType type)
        {
            return _items.TryGetValue(type, out int amount) ? amount : 0;
        }

        public void Clear()
        {
            _items.Clear();
        }

        private void OnDisable()
        {
            Clear();
        }
    }
}