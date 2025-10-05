using Manager;
using Unity.Cinemachine;
using UnityEngine;

namespace Misc {
    public class CameraController : MonoBehaviour {
        [SerializeField]
        private CinemachineVirtualCameraBase virtualCamera;

        private void Start() {
            EventManager.OnCharacterFlip += FlipView;
        }

        private void FlipView() {
            CinemachinePositionComposer positionComposer = virtualCamera.GetComponent<CinemachinePositionComposer>();
            Vector3 offset = positionComposer.TargetOffset;
            offset.x *= -1;
            positionComposer.TargetOffset = offset;
        }
    }
}