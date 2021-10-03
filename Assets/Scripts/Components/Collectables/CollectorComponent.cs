using System.Collections.Generic;
using Model.Data;
using PixelCrew.Model;
using UnityEngine;

namespace Components.Collectables
{
    public class CollectorComponent: MonoBehaviour, ICanAddInInventory
    {
        [SerializeField] private List<InventoryItemData> _items = new List<InventoryItemData>();
        public void AddCollectable(string id, int value)
        {
            _items.Add(new InventoryItemData(id){Value = value});
        }

        public void DropInInventory()
        {
            var session = FindObjectOfType<GameSession>();
            foreach (var item in _items)
            {
                session.Data.Inventory.Add(item.Id, item.Value);
            }
        }
    }
}