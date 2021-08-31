using System;
using System.Collections;
using PixelCrew;
using PixelCrew.Components;
using PixelCrew.Components.Extensions;
using PixelCrew.Model;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Creatures
{
    public class Hero : Creature
    {
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [SerializeField] private float _slamDownVelocity;

        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] private HeroWallet _wallet;
        
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        private float _defaultGravityScale;

        private bool _allowDoubleJump;

        private static readonly int ThrowTrigger = Animator.StringToHash("throw");

        private GameSession _session;

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = _rigidbody.gravityScale;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            GetComponent<HealthComponent>().SetHealth(_session.Data.Hp);
            _wallet.ChangeCoinAmount(_session.Data.Coins);
            UpdateHeroArmState();
        }

        protected override void Update()
        {
            base.Update();
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.IsInLayer(_groundCheck._layer))
            {
                var contact = col.contacts[0];
                if (contact.relativeVelocity.y > _slamDownVelocity)
                {
                    SpawnFallParticles();
                }
            }
        }

        void SpawnFallParticles()
        {
            _particles.Spawn("SlamDown");
        }

        protected override float CalculateYVelocity()
        {
            if (_isGrounded)
            {
                _allowDoubleJump = true;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!_isGrounded && _allowDoubleJump)
            {
                _particles.Spawn("Jump");
                _allowDoubleJump = false;
                return _jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }


        private void UpdateFlipDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            if (_wallet.Coins > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_wallet.Coins, 5);
            _wallet.DisposeCoins(numCoinsToDispose);
            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);
            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        public void saySomething()
        {
            Debug.Log("Shooot!");
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        public override void Attack()
        {
            if (!_session.Data.isArmed) return;
            base.Attack();
        }


        public void ArmHero()
        {
            _session.Data.isArmed = true;
            _animator.runtimeAnimatorController = _armed;
        }

        private void UpdateHeroArmState()
        {
            _animator.runtimeAnimatorController = _session.Data.isArmed ? _armed : _disarmed;
        }


        /**
     * TODO Move to separete class
     */
        public void OnHealthChanged(int health)
        {
            _session.Data.Hp = health;
        }

        /**
     * TODO Move to separete class
     */
        public void OnCoinsChanged(int coins)
        {
            _session.Data.Coins = coins;
        }

        public void Throw()
        {
            if (_session.Data.isArmed)
            {
                _animator.SetTrigger(ThrowTrigger);
            }
        }

        public void OnDoThrow()
        {
            _particles.Spawn("Throw");
        }
    }
}