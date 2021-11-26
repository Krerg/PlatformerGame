using System;
using PixelCrew.Model;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace Hero.Perk
{
    public abstract class Perk : MonoBehaviour
    {
        [SerializeField] private string _perkId;

        protected GameSession _gameSession;

        protected virtual void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
        }

        public void OnPerk()
        {
            if (_gameSession.PerksModel.Used == _perkId && _gameSession.PerksModel.IsPerkReady())
            {
                UsePerk();
            }
        }

        public abstract void UsePerk();
    }
}