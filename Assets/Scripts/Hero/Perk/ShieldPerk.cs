using System;
using Components.Health;
using PixelCrew.Model;
using UnityEngine;

namespace Hero.Perk
{
    public class ShieldPerk: Perk
    {
        [SerializeField] private float _useDuration = 5;

        private float _endTime;

        [SerializeField] private GameObject _hero;

        private SpriteRenderer _spriteRenderer;

        protected override void Start()
        {
            base.Start();
            _spriteRenderer = _hero.GetComponent<SpriteRenderer>();
        }

        protected void OnPerkEnd()
        {
            _hero.tag = "Player";
            _spriteRenderer.color = new Color(1, 1, 1, 1);
        }

        private void Update()
        {
            if (_endTime != 0 && Time.time > _endTime)
            {
                OnPerkEnd();
                _endTime = 0;
            }
        }

        public override void UsePerk()
        {
            _hero.tag = "InvulnerablePlayer";
            _spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            _endTime = Time.time + _useDuration;
            _gameSession.PerksModel.UpdatePerkCooldown();
            
        }
    }
}