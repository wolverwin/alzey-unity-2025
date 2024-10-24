using Manager;
using Trap;
using UnityEngine;

namespace Player {
    public class PlayerDamageController : MonoBehaviour {
        /// <summary>
        /// The time the player can't move for in seconds after getting hurt
        /// </summary>
        [SerializeField, Tooltip("The time the player can't move for, in seconds")]
        private float hurtTime = 0.5f;

        /// <summary>
        /// The time the player is invincible for in seconds after getting hurt
        /// </summary>
        [SerializeField]
        private float invincibleTime = 3;

        /// <summary>
        /// Whether to use trigger for the collision or not
        /// </summary>
        [SerializeField, Tooltip("If true, the damage is taken when hitting a trigger, instead of a collider")]
        private bool useTrigger;

        private GameManager gameManager;
        private float hurtTimer;
        private float invincibleTimer;

        private void Start() {
            gameManager = GameManager.Instance;
            EventManager.OnPlayerHurt += HurtPlayer;
        }

        private void Update() {
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
        private void HurtPlayer(GameObject source) {
            TrapController trapController = source.GetComponent<TrapController>();
            gameManager.Damage += trapController.Damage;
            trapController.TriggerHitAnimation();
            hurtTimer = hurtTime;
            invincibleTimer = invincibleTime;
            EventManager.InvokeOnPlayerInvincible();
        }

        private void OnCollisionStay2D(Collision2D collision) {
            if (gameManager.IsPlayerDead) {
                return;
            }

            if (collision.gameObject.CompareTag("DeathPlane")) {
                EventManager.InvokeOnPlayerDied();
                return;
            }

            if (useTrigger) {
                return;
            }

            if (invincibleTimer <= 0 && collision.gameObject.CompareTag("Trap")) {
                EventManager.InvokeOnPlayerHurt(collision.gameObject);
            }
        }

        private void OnTriggerStay2D(Collider2D collision) {
            if (!useTrigger || gameManager.IsPlayerDead) {
                return;
            }

            if (invincibleTimer <= 0 && collision.gameObject.CompareTag("Trap")) {
                EventManager.InvokeOnPlayerHurt(collision.gameObject);
            }
        }
    }
}
