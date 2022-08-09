using System.Collections;
using CustomUtils;
using Player;
using UnityEngine;

namespace Enemy {
    /// <summary>
    /// same as FireBallDragon class
    /// </summary>
    public class TornadoDragon : MonoBehaviour{
        [SerializeField] private int damage;
        [SerializeField] private float lifeTime;
        [SerializeField] private float moveSpeed;
        
        private void Awake() {
            var gameObject1 = gameObject;
            gameObject1.tag = "Enemy";
            Destroy(transform.parent.gameObject, lifeTime);
        }

        private void Update() {
            var moveDir = ReferenceTable.Player.transform.position - transform.position; 
            var moveDirection = moveSpeed * moveDir.normalized * Time.deltaTime;
            transform.parent.Translate(moveDirection, Space.Self);
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
            Destroy(transform.parent.gameObject);
            yield return new WaitForSeconds(.3f);
        }
    }
}