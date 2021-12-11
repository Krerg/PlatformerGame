using System;
using System.Collections;
using PixelCrew.Creatures.Weapons;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace Components.Lifecycle
{
    public class CircularProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private CircularProjectileSettings[] _settings;
        public int Stage { get; set; }

        [ContextMenu("Launch!")]
        public void LaunchProjectiles()
        {
            StartCoroutine(SpawnProjectiles());
        }

        private IEnumerator SpawnProjectiles()
        {
            var setting = _settings[Stage];
            var sectorStep = 2 * Mathf.PI / setting.BurstCount;
            for (int i = 0; i < setting.BurstCount; i++)
            {
                var angle = sectorStep * i;
                var direction1 = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                var direction2 = new Vector2(-Mathf.Cos(angle), -Mathf.Sin(angle));
                
                var instance1 = SpawnUtils.Spawn(setting.Prefab.gameObject, transform.position);
                var projectile1 = instance1.GetComponent<DirectionalProjectile>();
                projectile1.Launch(direction1);
                
                var instance2 = SpawnUtils.Spawn(setting.Prefab.gameObject, transform.position);
                var projectile2 = instance2.GetComponent<DirectionalProjectile>();
                projectile2.Launch(direction2);

                yield return new WaitForSeconds(setting.Delay);
            }
        }
    }

    [Serializable]
    public struct CircularProjectileSettings
    {
        [SerializeField] private DirectionalProjectile _prefab;
        [SerializeField] private int _burstCount;
        [SerializeField] private float _delay;

        public DirectionalProjectile Prefab => _prefab;

        public int BurstCount => _burstCount;

        public float Delay => _delay;
    }
}