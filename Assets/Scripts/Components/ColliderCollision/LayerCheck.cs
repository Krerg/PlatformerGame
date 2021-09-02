using UnityEngine;

namespace Components.ColliderCollision
{
    public class LayerCheck : MonoBehaviour
    {

        [SerializeField] public LayerMask _layer;

        [SerializeField] private Collider2D _collider;

        public bool IsTouchingLayer;
    
        private void OnTriggerStay2D(Collider2D other)
        {
            IsTouchingLayer = _collider.IsTouchingLayers(_layer);
        }
    
        private void OnTriggerE2D(Collider2D other)
        {
            IsTouchingLayer = _collider.IsTouchingLayers(_layer);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            IsTouchingLayer = _collider.IsTouchingLayers(_layer);
        }
    }
}
