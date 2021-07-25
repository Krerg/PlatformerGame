using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        private Collider2D[] _interactionResult = new Collider2D[5];

        public GameObject[] GetObjectInRange()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _interactionResult);
            var overlaps = new List<GameObject>();

            for (int i = 0; i < size; i++)
            {
                overlaps.Add(_interactionResult[i].gameObject);
            }

            return overlaps.ToArray();
        }
    }
}