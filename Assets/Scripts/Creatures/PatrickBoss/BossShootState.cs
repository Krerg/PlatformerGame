using System.Collections;
using System.Collections.Generic;
using Components.Lifecycle;
using UnityEngine;

public class BossShootState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var spawner = animator.GetComponent<CircularProjectileSpawner>();
        spawner.LaunchProjectiles();
    }
}
