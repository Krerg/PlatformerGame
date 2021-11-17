using System;
using Cinemachine;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class SetFollowComponent: MonoBehaviour
    {
        private void Start()
        {
            var camera = GetComponent<CinemachineVirtualCamera>();
            camera.Follow = FindObjectOfType<Hero.Hero>().transform;
        }
    }
}