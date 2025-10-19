using Manager;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Misc {
    public class CameraController : MonoBehaviour {
        [FormerlySerializedAs("virtualCamera")]
        [SerializeField]
        private CinemachineVirtualCameraBase cinemachineCamera;

        private void Start() {
            EventManager.OnCharacterFlip += FlipView;
        }

        private void FlipView() {
            CinemachinePositionComposer positionComposer = cinemachineCamera.GetComponent<CinemachinePositionComposer>();
            Vector3 offset = positionComposer.TargetOffset;
            offset.x *= -1;
            positionComposer.TargetOffset = offset;
        }
    }
}