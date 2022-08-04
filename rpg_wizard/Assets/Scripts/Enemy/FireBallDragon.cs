using System.Collections;
using CustomUtils;
using DefaultNamespace;
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

        private void Update() {
            var moveDir = ReferenceTable.Player.transform.position - transform.position; 
            var moveDirection = moveSpeed * moveDir.normalized * Time.deltaTime;
            transform.Translate(moveDirection, Space.Self);
        }

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