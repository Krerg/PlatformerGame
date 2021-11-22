using Components.Health;
using UnityEngine;

namespace Hero.Perk
{
    public class ShieldPerk: Perk
    {

        [SerializeField] private GameObject _hero;
        
        protected override void OnPerkStart()
        {
            _hero.tag = "InvulnerablePlayer";
        }

        protected override void OnPerkEnd()
        {
            _hero.tag = "Player";
        }
    }
}