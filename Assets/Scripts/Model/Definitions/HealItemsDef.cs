using System;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/HealItemsDef", fileName = "HealItemsDef")]
    public class HealItemsDef: ScriptableObject
    {
        [SerializeField] private HealItemDef[] _items;

        public HealItemDef Get(string id)
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

    }

    [Serializable]
    public struct HealItemDef
    {
        [InventoryId] [SerializeField] private string id;
        [SerializeField] private int _healAmount;

        public string Id => id;
        public int HealAmount => _healAmount;
    }
}