using Model.Data;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _defs;
        private static DefsFacade _instance;
        public static DefsFacade I => _instance == null ? LoadDefinitions() : _instance;

        public InventoryItemsDef Items => _defs;
        
        private static DefsFacade LoadDefinitions()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }
    }
}