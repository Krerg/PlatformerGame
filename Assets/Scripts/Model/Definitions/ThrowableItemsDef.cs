using System;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/ThrowableItemsDef", fileName = "ThrowableItemsDef")]
    public class ThrowableItemsDef: ScriptableObject
    {
        [SerializeField] private ThrowableDef[] _items;

        public ThrowableDef Get(string id)
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
    public struct ThrowableDef
    {
        [InventoryId] [SerializeField] private string id;
        [SerializeField] private GameObject _projectile;

        public string Id => id;
        public GameObject Projectile => _projectile;
    }
}