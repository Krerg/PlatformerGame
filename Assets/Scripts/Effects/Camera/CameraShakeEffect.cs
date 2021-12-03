using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Effects.Camera
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShakeEffect: MonoBehaviour
    {
        [SerializeField] private float _animationTime = 0.3f;
        [SerializeField] private float _intensity = 1;
        private CinemachineBasicMultiChannelPerlin _noise;
        private Coroutine _coroutine;
        
        private void Awake()
        {
            var virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void Shake()
        {
            if (_coroutine != null)
            {
                StopAnimation();
            }
            _coroutine = StartCoroutine(StartAnimation());
        }

        private void StopAnimation()
        {
            _noise.m_FrequencyGain = 0f;
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private IEnumerator StartAnimation()
        {
            _noise.m_FrequencyGain = _intensity;
            yield return new WaitForSeconds(_animationTime);
            StopAnimation();
        }
    }
}