using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class NinjaMobAI: MobAI
    {

        [SerializeField] private SpriteRenderer _mobRenderer;

        private Coroutine _mobVisibility;
        
        private void MakeMobInvisible()
        {
            if(_mobVisibility != null)
                StopCoroutine(_mobVisibility);
            _mobVisibility = StartCoroutine(FadeOut());
            _creature.SetInvulnerableToDamage();
        }

        protected override IEnumerator AgroToHero()
        {
            MakeMobInvisible();
            return base.AgroToHero();
        }
        
        private IEnumerator FadeOut()
        {
            var color = _mobRenderer.material.color;
            for (float i = color.a; i >= 0; i -= Time.deltaTime)
            {
                _mobRenderer.material.color = new Color(color.r, color.g, color.b,i);
                yield return null;
            }
        }
        
        private IEnumerator FadeIn()
        {
            var color = _mobRenderer.material.color;
            for (float i = color.a; i < 1; i += Time.deltaTime)
            {
                _mobRenderer.material.color = new Color(color.r, color.g, color.b,i);
                yield return null;
            }
        }

        protected override IEnumerator Patrolling()
        {
            MakeMobVisible();
            return base.Patrolling();
        }
        
        private void MakeMobVisible()
        {
            if (_mobRenderer.material.color.a == 1)
                return;            
            if (_mobVisibility != null)
            {
                StopCoroutine(_mobVisibility);
            }
            _creature.SetVulnerableToDamage();
            _mobVisibility = StartCoroutine(FadeIn());
        }
        
    }
}