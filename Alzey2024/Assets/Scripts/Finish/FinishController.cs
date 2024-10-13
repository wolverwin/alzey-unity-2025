using Manager;
using UnityEngine;

namespace Finish {
    public class FinishController : MonoBehaviour {

        [SerializeField]
        Collider2D finishCollider;

        [SerializeField]
        Animator animator;
        
        void Start() {
            finishCollider.enabled = false;

            EventManager.OnRequirementsFulfilled += SetFinishActive;
        }

        void SetFinishActive() {
            finishCollider.enabled = true;
            animator.SetTrigger("Activate");
        }
    }
}
