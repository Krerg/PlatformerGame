using Model.Data;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _items;
        [SerializeField] private ThrowableItemsDef _throwabeItemsdefs;
        [SerializeField] private HealItemsDef _healItemsdefs;
        [SerializeField] private PerkRepository _perks;
        [SerializeField] private PlayerDef _player;
        [SerializeField] private ItemPriceRepository _itemPrice;
        
        private static DefsFacade _instance;
        public static DefsFacade I => _instance == null ? LoadDefinitions() : _instance;

        public InventoryItemsDef Items => _items;
        public ThrowableItemsDef ThrowableItems => _throwabeItemsdefs;
        public HealItemsDef HealItems => _healItemsdefs;
        public PerkRepository Perks => _perks;
        public PlayerDef Player => _player;
        public ItemPriceRepository ItemPrice => _itemPrice;

        private static DefsFacade LoadDefinitions()
        {
                return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }
    }
}