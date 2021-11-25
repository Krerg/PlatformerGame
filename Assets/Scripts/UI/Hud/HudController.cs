using System;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using UI.Widgets;
using UnityEngine;

namespace UI.Hud
{
    public class HudController: MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;

        private GameSession _session;
        
        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Hp.OnChanged += OnHealthChanged;
            OnHealthChanged(_session.Data.Hp.Value, 0);
        }

        private void OnHealthChanged(int newvalue, int oldvalue)
        {
            var maxHealth = _session.StatsModel.GetValue(StatId.Hp);
            var value = (float) newvalue / maxHealth;
            _healthBar.SetProgress(value);
        }

        private void OnDestroy()
        {
            _session.Data.Hp.OnChanged -= OnHealthChanged;
        }
    }
}