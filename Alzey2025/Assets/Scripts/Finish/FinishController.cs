using Manager;
using UnityEngine;

namespace Finish {
    public class FinishController : MonoBehaviour {

        [SerializeField]
        private Collider2D finishCollider;

        [SerializeField]
        private Animator animator;

        private void Start() {
            finishCollider.enabled = false;

            EventManager.OnRequirementsFulfilled += SetFinishActive;
        }

        private void SetFinishActive() {
            finishCollider.enabled = true;
            animator.SetTrigger("Activate");
        }
    }
}
