using System;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud.Perk
{
    public class ActivePerkWidget: MonoBehaviour
    {
        [SerializeField] private Image _perkIcon;
        [SerializeField] private ProgressBarWidget _cooldownHud;
        private GameSession _gameSession;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
            _trash.Retain(_gameSession.PerksModel.SubscribeOnChange(UpdatePerkHud));
            UpdatePerkHud();
        }

        private void Update()
        {
            if (String.IsNullOrEmpty(_gameSession.PerksModel.Used))
            {
                return;
            }

            var cooldownValue = _gameSession.PerksModel.GetCooldownValue();
            if (cooldownValue >= 0)
            {
                _cooldownHud.SetProgress(cooldownValue);
            }
            
        }

        private void UpdatePerkHud()
        {
            if (String.IsNullOrEmpty(_gameSession.PerksModel.Used))
            {
                HideActivePerk();
                return;
            }
            ShowActivePerk();
            var perkDef = DefsFacade.I.Perks.Get(_gameSession.PerksModel.Used);
            _perkIcon.sprite = perkDef.Icon;
            _cooldownHud.SetProgress(1);
        }

        private void HideActivePerk()
        {
            _perkIcon.gameObject.SetActive(false);
            _cooldownHud.gameObject.SetActive(false);
        }

        private void ShowActivePerk()
        {
            _perkIcon.gameObject.SetActive(true);
            _cooldownHud.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}