using System;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    public class HudController: MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;

        [SerializeField] private Text coinsAmount;
        
        private GameSession _session;
        
        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Hp.OnChanged += OnHealthChanged;
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            OnHealthChanged(_session.Data.Hp.Value, 0);
            OnInventoryChanged("Coin", _session.Data.Inventory.Count("Coin"));
        }

        private void OnHealthChanged(int newvalue, int oldvalue)
        {
            var maxHealth = _session.StatsModel.GetValue(StatId.Hp);
            var value = (float) newvalue / maxHealth;
            _healthBar.SetProgress(value);
        }

        private void OnInventoryChanged(string id, int count)
        {
            if (id == "Coin")
            {
                coinsAmount.text = count.ToString();
            }
        }

        private void OnDestroy()
        {
            _session.Data.Hp.OnChanged -= OnHealthChanged;
        }
    }
}