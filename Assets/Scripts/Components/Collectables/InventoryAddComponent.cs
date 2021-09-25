using PixelCrew.Model.Definitions;
using UnityEngine;

namespace Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId][SerializeField] private string _id;
        [SerializeField] private int _count;

        public void Add(GameObject go)
        {
            var hero = go.GetComponent<Hero.Hero>();
            if (hero != null)
            {
                hero.AddCollectable(_id, _count);
            }
        }
        
    }
}