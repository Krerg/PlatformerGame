using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Behavior
{
    public abstract class Patrol: MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}