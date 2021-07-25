using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew
{
    public class HeroWallet : MonoBehaviour
    {
        [SerializeField] private int _coins;

        public int Coins
        {
            get => _coins;
        }

        public void OnPickSilverCoin()
        {
            AddCoins(1);
        }

        public void OnPickGoldCoin()
        {
            AddCoins(10);
        }

        private void AddCoins(int coins)
        {
            _coins += coins;
            Debug.Log("Coins: " + _coins);
        }

        public void DisposeCoins(int coinsToDispose)
        {
            _coins -= coinsToDispose;
        }
        
        
        
    }
}