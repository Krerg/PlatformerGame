using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Components.Collectables
{
    public class VerticalLevitationComponent: MonoBehaviour
    {
        [SerializeField] private float _frequency = 4f;
        [SerializeField] private float _amplitude = 1f;
        [SerializeField] private bool _randomize;
        
        private float _originalY;
        private float _time;
        private Rigidbody2D _rigidbody;
        private float _seed;
        
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _originalY = _rigidbody.position.y;
            if (_randomize)
            {
                _seed = Random.value * Mathf.PI * 2;
            }
        }

        private void Update()
        {
            var pos = _rigidbody.position;
            pos.y = _originalY + Mathf.Sin(_seed + Time.time * _frequency) * -_amplitude;
            _rigidbody.MovePosition(pos);
        }
    }
}