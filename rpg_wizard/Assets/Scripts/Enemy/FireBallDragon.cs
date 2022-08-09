using System.Collections;
using CustomUtils;
using Player;
using UnityEngine;

namespace Enemy {
    public class FireBallDragon : MonoBehaviour {
        [SerializeField] private int damage;
        [SerializeField] private float lifeTime;
        [SerializeField] private float moveSpeed;
        
        private void Awake() {
            var gameObject1 = gameObject;
            gameObject1.tag = "Enemy";
            Destroy(gameObject1, lifeTime);
        }

        /// <summary>
        /// updating moveDirection and moving towards it
        /// </summary>
        private void Update() {
            var moveDir = ReferenceTable.Player.transform.position - transform.position; 
            var moveDirection = moveSpeed * moveDir.normalized * Time.deltaTime;
            transform.Translate(moveDirection, Space.Self);
        }

        /// <summary>
        /// destroying if the hit object is the Shield
        /// getting the health of hit object -> reducing it by damage
        /// signaling Player to be hit
        /// Destroying this gameObject
        /// </summary>
        /// <param name="other">Collider that was hit</param>
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer == LayerMask.NameToLayer("Shield")) StartCoroutine(DestroyThis());

            if (other.gameObject.GetComponentInParent<Health.Health>() == null) return;
            if (other.gameObject.CompareTag(gameObject.tag)) return;

            other.gameObject.GetComponentInParent<Health.Health>().GetDamagedOverTime(damage,1);

            var playerController = other.gameObject.GetComponentInParent<Controller>();
            if(!playerController) return;
            playerController.GotHit();

            StartCoroutine(DestroyThis());
        }

        private IEnumerator DestroyThis() {
            Destroy(gameObject);
            yield return new WaitForSeconds(.3f);
        }
    }
}