using System;
using System.Collections;
using PixelCrew;
using PixelCrew.Components;
using PixelCrew.Components.Extensions;
using PixelCrew.Model;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;


public class Hero : MonoBehaviour
{
    private Vector2 _direction;

    [SerializeField] private CheckCircleOverlap _attackRange;

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _damageJumpSpeed;
    [SerializeField] private LayerCheck _layerCheck;
    [SerializeField] private float _interactRadius;
    [SerializeField] private Collider2D[] _interactionResult = new Collider2D[1];
    [SerializeField] private LayerMask _interactionLayer;

    [Space] [SerializeField] private SpawnComponent _footStepsSpawnComponent;
    [SerializeField] private SpawnComponent _jumpParticlesSpawnComponent;
    [SerializeField] private SpawnComponent _fallParticlesSpawnComponent;
    [SerializeField] private SpawnComponent _attackParticlesSpawnComponent;

    [SerializeField] private float _slamDownVelocity;

    [SerializeField] private ParticleSystem _hitParticles;
    [SerializeField] private HeroWallet _wallet;

    private Animator _animator;
    [SerializeField] private AnimatorController _armed;
    [SerializeField] private AnimatorController _disarmed;

    private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
    private static readonly int IsRunningKey = Animator.StringToHash("is-running");
    private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
    private static readonly int Hit = Animator.StringToHash("hit");
    private static readonly int AttackTrigger = Animator.StringToHash("attack");

    private static readonly int FallKey = Animator.StringToHash("Fall");

    private bool _isGrounded;
    private bool _allowDoubleJump;

    private bool _isDamageTaken = false;
    [SerializeField] private int _damage;
    
    private GameSession _session;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        FindObjectOfType<HealthComponent>().SetHealth(_session.Data.Hp);
        _wallet.ChangeCoinAmount(_session.Data.Coins);
        UpdateHeroArmState();
    }

    private void Update()
    {
        _isGrounded = IsGrounded();
    }

    private void FixedUpdate()
    {
        var xVelocity = _direction.x * _speed;
        var yVelocity = CalculateYVelocity();
        _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

        _animator.SetBool(IsGroundKey, _isGrounded);
        _animator.SetFloat(VerticalVelocityKey, _rigidbody.velocity.y);
        _animator.SetBool(IsRunningKey, _direction.x != 0);
        UpdateFlipDirection();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.IsInLayer(_layerCheck._groundLayer))
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
        _fallParticlesSpawnComponent.Spawn();
    }

    private float CalculateYVelocity()
    {
        var yVelocity = _rigidbody.velocity.y;
        var isJumpPressing = _direction.y > 0;
        if (_isDamageTaken)
        {
            return yVelocity;
        }

        if (IsGrounded())
        {
            _allowDoubleJump = true;
        }

        if (isJumpPressing)
        {
            yVelocity = CalculateJumpVelocity(yVelocity);
        }
        else if (_rigidbody.velocity.y > 0)
        {
            yVelocity *= 0.05f;
        }

        return yVelocity;
    }

    private float CalculateJumpVelocity(float yVelocity)
    {
        var isFalling = _rigidbody.velocity.y <= 0.001f;
        if (!isFalling) return yVelocity;

        if (_isGrounded)
        {
            SpawnJumpParticles();
            yVelocity += _jumpSpeed;
        }
        else if (_allowDoubleJump)
        {
            SpawnJumpParticles();
            yVelocity = _jumpSpeed;
            _allowDoubleJump = false;
        }

        return yVelocity;
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

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private bool IsGrounded()
    {
        return _layerCheck.IsTouchingLayer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
    }

    public void TakeDamage()
    {
        _animator.SetTrigger(Hit);
        _isDamageTaken = true;
        StartCoroutine(ResetDamageTaken());
        if (_wallet.Coins > 0)
        {
            SpawnCoins();
        }

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);
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

    public void SpawnFootDust()
    {
        _footStepsSpawnComponent.Spawn();
    }
    
    public void SpawnAttackParticles()
    {
        _attackParticlesSpawnComponent.Spawn();
    }

    public void SpawnJumpParticles()
    {
        _jumpParticlesSpawnComponent.Spawn();
    }

    private IEnumerator ResetDamageTaken()
    {
        yield return new WaitForSeconds(0.5f);
        _isDamageTaken = false;
    }

    public void saySomething()
    {
        Debug.Log("Shooot!");
    }

    public void Interact()
    {
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactRadius, _interactionResult,
            _interactionLayer);
        for (int i = 0; i < size; i++)
        {
            var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    /**
     * Plays attack animation
     */
    public void Attack()
    {
        if (!_session.Data.isArmed) return;
        _animator.SetTrigger(AttackTrigger);
    }

    /**
     * Calculates objects to damage of attack animation played.
     */
    public void OnAttackEvent()
    {
        var gos = _attackRange.GetObjectInRange();
        foreach (var go in gos)
        {
            var hp = go.GetComponent<HealthComponent>();
            if (hp != null && go.CompareTag("Enemy"))
            {
                hp.ApplyHealthChange(_damage);
            }
        }
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
    
}