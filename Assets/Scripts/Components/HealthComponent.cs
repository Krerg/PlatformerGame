using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onHealUp;
        [SerializeField] private UnityEvent _onDamageTaken;
        [SerializeField] private UnityEvent _onHealthEmpty;

        public void ApplyHealthChange(int damageValue)
        {
            _health += damageValue;
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
    }
}