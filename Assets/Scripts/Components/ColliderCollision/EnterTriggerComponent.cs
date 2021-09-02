using System;
using PixelCrew.Components.Extensions;
using UnityEngine;

namespace Components.ColliderCollision
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private String _tag;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterCollisionComponent.EnterEvent _action;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.IsInLayer(_layer)) return;
            if (!string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag)) return;

            _action?.Invoke(other.gameObject);
        }
    }
}