using System;
using UnityEngine;
using UnityEngine.Events;

namespace Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onHealUp;
        [SerializeField] private UnityEvent _onDamageTaken;
        [SerializeField] private UnityEvent _onHealthEmpty;

        [SerializeField] private HealthChangeEvent _onChange;

        private int maxHealth;
        
        private void Start()
        {
            maxHealth = _health;
        }

        public void ApplyHealthChange(int damageValue)
        {
            if (_health <= 0) return;
            
            _health += damageValue;
            
            _onChange?.Invoke(_health);
            if (damageValue > 0)
            {
                _onHealUp?.Invoke();
            }
            else
            {
                _onDamageTaken?.Invoke();
            }

            if (_health <= 0)
            {
                _onHealthEmpty?.Invoke();
            }
        }

        public void SetHealth(int health)
        {
            _health = health;
        }
    }

    [Serializable]
    public class HealthChangeEvent : UnityEvent<int>
    {
        
    }
}