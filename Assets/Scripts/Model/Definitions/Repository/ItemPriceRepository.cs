using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/ItemPriceDef", fileName = "ItemPriceDef")]
    public class ItemPriceRepository : ScriptableObject
    {
        [SerializeField] private ItemPriceDef[] _items;

        public ItemPriceDef Get(string id)
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
        
        public ItemPriceDef[] All => new List<ItemPriceDef>(_items).ToArray();
    }

    [Serializable]
    public struct ItemPriceDef
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private ItemWithCount _sellPrice;
        [SerializeField] private ItemWithCount _buyPrice;
        public string Id => _id;
        public ItemWithCount SellPrice => _sellPrice;
        public ItemWithCount BuyPrice => _buyPrice;
    }
}