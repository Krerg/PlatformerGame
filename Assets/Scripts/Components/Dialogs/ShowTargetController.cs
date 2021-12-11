using Cinemachine;
using UnityEngine;

namespace Components.Dialogs
{
    public class ShowTargetController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CinemachineVirtualCamera _camera;

        private static readonly int ShowKey = Animator.StringToHash("show");

        public void SetPosition(Vector3 targetPosition)
        {
            targetPosition.z = _camera.transform.position.z;
            _camera.transform.position = targetPosition;
        }

        public void SetState(bool isShown)
        {
            _animator.SetBool(ShowKey, isShown);
        }
    }
}