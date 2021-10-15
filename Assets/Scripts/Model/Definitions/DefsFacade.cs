using Model.Data;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _defs;
        [SerializeField] private PlayerDef _player;
        
        private static DefsFacade _instance;
        public static DefsFacade I => _instance == null ? LoadDefinitions() : _instance;

        public InventoryItemsDef Items => _defs;
        public PlayerDef Player => _player;
        
        private static DefsFacade LoadDefinitions()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }
    }
}