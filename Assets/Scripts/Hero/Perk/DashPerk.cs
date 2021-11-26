using Components;
using UnityEngine;

namespace Hero.Perk
{
    public class DashPerk: Perk
    {
        private Cooldown _useCooldown;

        [SerializeField] private Hero _hero;
        
        public override void UsePerk()
        {
            _gameSession.PerksModel.UpdatePerkCooldown();
            _hero.UseDash();
        }
    }
}