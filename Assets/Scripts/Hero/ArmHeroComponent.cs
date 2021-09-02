using PixelCrew.Creatures;
using UnityEngine;

namespace PixelCrew
{
    public class ArmHeroComponent : MonoBehaviour
    {
        public void ArmHero(GameObject go)
        {
            var hero = go.GetComponent<Hero.Hero>();
            if (hero != null)
            {
                hero.ArmHero();
            }
        }
    }
}