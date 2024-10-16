using Cinemachine;
using Manager;
using UnityEngine;

namespace Misc {
    public class CameraController : MonoBehaviour {
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        private void Start() {
            EventManager.OnCharacterFlip += FlipView;
        }

        private void FlipView() {
            CinemachineFramingTransposer framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            Vector3 offset = framingTransposer.m_TrackedObjectOffset;
            offset.x *= -1;
            framingTransposer.m_TrackedObjectOffset = offset;
        }
    }
}