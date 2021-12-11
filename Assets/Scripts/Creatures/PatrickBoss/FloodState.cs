using UnityEngine;

namespace PixelCrew.Creatures.PatrickBoss
{
    public class FloodState: StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var floodController = animator.GetComponent<FloodController>();
            floodController.StartFlooding();
        }
    }
}