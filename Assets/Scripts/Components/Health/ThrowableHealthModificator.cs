using System;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;

namespace Components.Health
{
    public class ThrowableHealthModificator: HealthModificatorComponent
    {
        private void Start()
        {
            var session = FindObjectOfType<GameSession>();
            _damage = (int) session.StatsModel.GetValue(StatId.RangeDamage);
        }
    }
}