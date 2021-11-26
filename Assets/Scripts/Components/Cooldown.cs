using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Components
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] private float _value;

        private float _timesUp;
        
        public void Reset()
        {
            _timesUp = Time.time + _value;
        }
        public void Update(float newCooldownValue)
        {
            _value = newCooldownValue;
            _timesUp = Time.time;
        }

        public float GetTimeLeftInPercent()
        {
            return (_timesUp - Time.time) / _value;
        }

        public bool IsReady => _timesUp <= Time.time;

    }
}