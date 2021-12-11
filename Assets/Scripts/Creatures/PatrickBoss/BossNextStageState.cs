using Components.Lifecycle;
using UnityEngine;

namespace PixelCrew.Creatures.PatrickBoss
{
    public class BossNextStageState : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CircularProjectileSpawner>();
            spawner.Stage++;

            var changeLight = animator.GetComponent<ChangeLightsComponent>();
            changeLight.SetColor();
        }
    }
}