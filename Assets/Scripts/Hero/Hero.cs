using System;
using Components;
using Components.ColliderCollision;
using Components.Health;
using Components.Lifecycle;
using Model.Data;
using PixelCrew;
using PixelCrew.Components.Extensions;
using PixelCrew.Creatures;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using UnityEngine;

namespace Hero
{
    public class Hero : Creature, ICanAddInInventory
    {
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [SerializeField] private float _slamDownVelocity;

        [SerializeField] private ParticleSystem _hitParticles;

        [SerializeField] private RuntimeAnimatorController  _armed;
        [SerializeField] private RuntimeAnimatorController  _disarmed;

        [SerializeField] private Cooldown _throwCooldown;

        [SerializeField] private SpawnComponent _throwSpawner;

        [SerializeField] private HealthComponent _heroHealth;
        
        private bool _allowDoubleJump;

        private static readonly int ThrowTrigger = Animator.StringToHash("throw");

        private GameSession _session;

        private const string SwordId = "Sword";
        
        private int SwordCount => _session.Data.Inventory.Count(SwordId);
        private int CoinsCount => _session.Data.Inventory.Count("Coin");
        private string SelectedId => _session.QuickInventory.SelectedItem.Id;
        
        private bool CanThrow
        {
            get
            {
                if (SelectedId == SwordId)
                {
                    return SwordCount > 1;
                }
                var def = DefsFacade.I.Items.Get(SelectedId);
                return def.HasTag(ItemTag.Throwable);
            }
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            GetComponent<HealthComponent>().SetHealth(_session.Data.Hp.Value);
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            
            UpdateHeroArmState();
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == SwordId)
            {
                UpdateHeroArmState();
            }
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
                DoJumpVfx();
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

        public void AddCollectable(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }
        
        public override void TakeDamage()
        {
            base.TakeDamage();
            if (CoinsCount > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(CoinsCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);
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
            if (SwordCount <= 0) return;
            base.Attack();
        }

        private void UpdateHeroArmState()
        {
            _animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed;
        }


        /**
     * TODO Move to separete class
     */
        public void OnHealthChanged(int health)
        {
            _session.Data.Hp.Value = health;
        }

        public void Throw()
        {
            if (!_throwCooldown.IsReady) return;
            if (CanThrow)
            {
                _animator.SetTrigger(ThrowTrigger);
                _throwCooldown.Reset();
            }
        }

        public void UseHealthPotion()
        {
            var def = DefsFacade.I.Items.Get(SelectedId);
            var isHealItem =  def.HasTag(ItemTag.Usable) && def.Id.Contains("Potion");
            if (!isHealItem) return;
            var healAmount = DefsFacade.I.HealItems.Get(SelectedId).HealAmount;
            _heroHealth.ApplyHealthChange(healAmount);
            _session.Data.Inventory.Remove(SelectedId, 1);
        }

        public void OnDoThrow()
        {
            Sounds.Play("Range");
            var id = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.ThrowableItems.Get(id);
            _throwSpawner.SetPrefab(throwableDef.Projectile);
            _throwSpawner.Spawn();
            _session.Data.Inventory.Remove(id, 1);
        }

        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }
    }
}