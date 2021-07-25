using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew
{
    public class HeroWallet : MonoBehaviour
    {
        [SerializeField] private int _coins;
        [SerializeField] private CoinAmountChangeEvent _onChange;

        public int Coins
        {
            get => _coins;
        }

        public void OnPickSilverCoin()
        {
            ChangeCoinAmount(1);
        }

        public void OnPickGoldCoin()
        {
            ChangeCoinAmount(10);
        }

        public void ChangeCoinAmount(int coins)
        {
            _coins += coins;
            _onChange?.Invoke(_coins);
        }

        public void DisposeCoins(int coinsToDispose)
        {
            ChangeCoinAmount(-coinsToDispose);
        }
    }

    [Serializable]
    public class CoinAmountChangeEvent : UnityEvent<int>
    {
    }
}