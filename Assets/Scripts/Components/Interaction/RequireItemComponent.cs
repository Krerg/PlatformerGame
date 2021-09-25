using Model.Data;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using UnityEngine;
using UnityEngine.Events;

namespace Components.Interaction
{
    public class RequireItemComponent: MonoBehaviour
    {

        [SerializeField] private InventoryItemData[] _required;
        
        [SerializeField] private bool _removeAfterUse;

        [SerializeField] private UnityEvent _onSucess;
        [SerializeField] private UnityEvent _onFail;

        public void Check()
        {
            var session = FindObjectOfType<GameSession>();
            var isAllRequirementsMet = true;
            foreach (var itemData in _required)
            {
                var numItems = session.Data.Inventory.Count(itemData.Id);
                if (numItems < itemData.Value)
                {
                    isAllRequirementsMet = false;
                }
            }
            if (isAllRequirementsMet)
            {
                if (_removeAfterUse) 
                {
                    foreach (var itemData in _required)
                    {
                        session.Data.Inventory.Remove(itemData.Id, itemData.Value);
                    }
                }
                _onSucess?.Invoke();
            }
            else
            {
                _onFail?.Invoke();
            }
        }
        
    }
}