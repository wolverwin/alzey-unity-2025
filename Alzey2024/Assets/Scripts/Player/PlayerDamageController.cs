using Manager;
using UnityEngine;

namespace Player {
    public class PlayerDamageController : MonoBehaviour {

        /// <summary>
        /// The damage the player gets when hurt once
        /// </summary>
        [SerializeField]
        int damagePerHurt = 1;

        /// <summary>
        /// The time the player is invincible for in seconds after getting hurt
        /// </summary>
        [SerializeField]
        float hurtTime = 1.5f;

        /// <summary>
        /// The time the player is invincible for in seconds after getting hurt
        /// </summary>
        [SerializeField]
        float invincibleTime = 3;

        float hurtTimer;
        float invincibleTimer;

        void Start() {
            EventManager.OnPlayerHurt += HurtPlayer;
        }

        void Update() {
            if (hurtTimer > 0) {
                hurtTimer -= Time.deltaTime;

                if (hurtTimer < 0) {
                    EventManager.InvokeOnPlayerRecovered();
                }
            }

            if (invincibleTimer > 0) { 
                invincibleTimer -= Time.deltaTime;

                if (invincibleTimer < 0) {
                    EventManager.InvokeOnPlayerNotInvincible();
                }
            }
        }

        /// <summary>
        /// Adds the given damage to the player damage
        /// </summary>
        void HurtPlayer() {
            GameManager.Instance.Damage += damagePerHurt;
            hurtTimer = hurtTime;
            invincibleTimer = invincibleTime;
            EventManager.InvokeOnPlayerInvincible();
        }

        private void OnTriggerStay2D(Collider2D collision) {
            if (invincibleTimer <= 0 && collision.gameObject.CompareTag("Trap")) {
                EventManager.InvokeOnPlayerHurt();
            }
        }
    }
}
