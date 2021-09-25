using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "InventoryItems")]
    public class InventoryItemsDef : ScriptableObject
    {
        [SerializeField] private ItemDef[] _items;

        public ItemDef Get(string id)
        {
            foreach (var item in _items)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }

            return default;
        }

#if UNITY_EDITOR
        public ItemDef[] ItemDefsEditor => _items;
#endif
        
    }

    [Serializable]
    public struct ItemDef
    {
        [SerializeField] private String _id;
        [SerializeField] private bool _isStackable;
        public string Id => _id;
        public bool IsStackable => _isStackable;

        public bool IsVoid => string.IsNullOrEmpty(_id);
    }
}