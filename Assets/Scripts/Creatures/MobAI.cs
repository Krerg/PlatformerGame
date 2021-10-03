using System;
using System.Collections;
using Components;
using Components.ColliderCollision;
using Components.Lifecycle;
using PixelCrew.Creatures.Behavior;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;

        [SerializeField] protected float _alarmDelay = 1f;
        [SerializeField] private float _attackCooldown = 1f;

        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");

        private Coroutine _current;

        private bool _isDead;

        private GameObject _target;

        [SerializeField] protected SpawnListComponent _particles;
        [SerializeField] protected Creature _creature;
        [SerializeField] private Animator _animator;

        [Header("Behavior")]
        [SerializeField] private Patrol _patrol;
        
        private void Start()
        {
            StartState(Patrolling());
        }

        public void OnHeroInVision(GameObject go)
        {
            if (_isDead)
                return;
            _target = go;
            StartState(AgroToHero());
        }

        protected virtual IEnumerator AgroToHero()
        {
            LookAtHero();
            StopCreature();
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToHero());
        }

        public void LookAtHero()
        {
            var direction = GetDirectionToTarget();
            _creature.UpdateSpriteDirection(direction);
        }

        protected void StopCreature()
        {
            _creature.SetDirection(Vector2.zero);
        }
        
        protected virtual IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StopCreature();
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }

                yield return null;
            }
            StopCreature();
            _particles.Spawn("Miss");
            yield return new WaitForSeconds(1);
            StartState(Patrolling());
        }

        protected virtual IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(GoToHero());
        }

        private void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(direction);
        }
        
        private Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        protected virtual IEnumerator Patrolling()
        {
            return _patrol.DoPatrol();
        }

        protected void StartState(IEnumerator coroutine)
        {
            if (_isDead)
            {
                return;
            }
            if (_current != null)
                StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }

        public void OnDie()
        {
            StopCreature();
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);
            
            if (_current != null)
                StopCoroutine(_current);
            
        }
    }
}