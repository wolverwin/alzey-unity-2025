using Cinemachine;
using Manager;
using UnityEngine;

namespace Misc {
    public class CameraController : MonoBehaviour {
        [SerializeField]
        CinemachineVirtualCamera virtualCamera;

        void Start() {
            EventManager.OnCharacterFlip += FlipView;
        }

        void FlipView() {
            CinemachineFramingTransposer framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            Vector3 offset = framingTransposer.m_TrackedObjectOffset;
            offset.x *= -1;
            framingTransposer.m_TrackedObjectOffset = offset;
        }
    }
}