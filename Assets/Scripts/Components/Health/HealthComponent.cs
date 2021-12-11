using System;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.Events;

namespace Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;

        public int Health => _health;

        [SerializeField] private UnityEvent _onHealUp;
        [SerializeField] public UnityEvent _onDamageTaken;
        [SerializeField] public UnityEvent _onHealthEmpty;
        [SerializeField] public HealthChangeEvent _onChange;

        private Lock _immune = new Lock();

        public Lock Immune => _immune;

        public void ApplyHealthChange(int damageValue)
        {
            if (_health <= 0 || _immune.IsLocked) return;
            
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