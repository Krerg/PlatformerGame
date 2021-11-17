using System;
using UnityEngine;

namespace Effects
{
    public class ParallaxEffect: MonoBehaviour
    {
        [SerializeField] private float _effectValue;
        [SerializeField] private Transform _followTarget;
        
        private float _startX;
        private float _startY;
        
        
        private void Start()
        {
            _startX = transform.position.x;
            _startY = transform.position.y;
        }

        private void LateUpdate()
        {
            var currentPosition = transform.position;
            var deltaX = _followTarget.position.x * _effectValue;
            transform.position = new Vector3(_startX + deltaX, currentPosition.y, currentPosition.z);
        }
    }
}