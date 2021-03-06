using UnityEngine;

namespace Components.Health
{
    public class HealthModificatorComponent : MonoBehaviour
    {
        [SerializeField] protected int _damage;
        
        public void OnHealthChanged(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ApplyHealthChange(_damage);
            }
        }
    }
}