using System;
using Components.Lifecycle;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.Events;

namespace Level
{
    public class CheckPointComponent: MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private UnityEvent _setChecked;
        [SerializeField] private UnityEvent _setUnchecked;

        public string Id => _id;
        
        private GameSession _session;
        [SerializeField] private SpawnComponent _heroSpawner;
        
        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            if (_session.IsCheked(_id))
            {
                _setChecked?.Invoke();
            }
            else
            {
                _setUnchecked?.Invoke();
            }
        }

        public void Check()
        {
            _session.SetChecked(_id);
            _setChecked?.Invoke();
        }

        public void SpawnHero()
        {
            _heroSpawner.Spawn();
        }
        
    }
}