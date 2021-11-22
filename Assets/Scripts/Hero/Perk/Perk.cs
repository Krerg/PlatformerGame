using System;
using PixelCrew.Model;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace Hero.Perk
{
    public abstract class Perk : MonoBehaviour
    {
        [SerializeField] private string _perkId;

        private GameSession _gameSession;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
            _trash.Retain(_gameSession.PerksModel.SubscribeOnUse(id => CallOnEqualId(id, OnPerkStart)));
            _trash.Retain(_gameSession.PerksModel.SubscribeOnCooldown(id => CallOnEqualId(id, OnPerkEnd)));
        }

        private void CallOnEqualId(string perkId, Action call)
        {
            if (perkId == _perkId)
                call?.Invoke();
        }

        protected abstract void OnPerkStart();
        protected abstract void OnPerkEnd();

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}