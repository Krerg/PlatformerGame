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

        public void UpdateValue(float value)
        {
            _value = value;
            Reset();
        }

        public float GetTimeLeftInPercent()
        {
            return (_timesUp - Time.time) / _value;
        }

        public bool IsReady => _timesUp <= Time.time;

    }
}