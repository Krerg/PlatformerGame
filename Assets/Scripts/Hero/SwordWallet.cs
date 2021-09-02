using System;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew
{
    public class SwordWallet: MonoBehaviour
    {

        [SerializeField] private int _swordAmount;

        private GameSession _gameSession;

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
            _swordAmount = _gameSession.Data.SwordAmount;
        }

        private void UpdateGameSession()
        {
            _gameSession.Data.SwordAmount = _swordAmount;
        }

        public int TryDisposeSword()
        {
            if (CanDisposeSword())
            {
                _swordAmount -= 1;
                UpdateGameSession();
                return 1;
            }
            return 0;
        }

        public void AddSword()
        {
            _swordAmount += 1;
            UpdateGameSession();
        }

        public bool CanDisposeSword()
        {
            return _swordAmount > 1;
        }
        
        
    }
}