using System;
using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class LinearProjectile: Projectile
    {
        protected override void Start()
        {
            base.Start();
            var force = new Vector2(_direction * _speed, 0);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}