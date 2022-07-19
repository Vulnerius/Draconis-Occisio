using System;
using System.Collections;
using DefaultNamespace;
using Player;
using UnityEngine;

namespace Enemy {
    public class FireBallDragon : MonoBehaviour {
        [SerializeField] private int damage;

        [SerializeField] private float lifeTime;

        [SerializeField] private float moveSpeed;
        private Transform m_Transform;
    
        private void Awake() {
            gameObject.tag = "Enemy";
            m_Transform = ReferenceTable.LookAtPlayer;
            Destroy(gameObject, lifeTime);
        }

        private void Update() {
            var moveDirection = moveSpeed * (m_Transform.position * Time.deltaTime);
            transform.Translate(moveDirection);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer == LayerMask.NameToLayer("Shield")) StartCoroutine(DestroyThis());

            if (other.gameObject.GetComponentInParent<Health.Health>() == null) return;
            if (other.gameObject.CompareTag(gameObject.tag)) return;

            other.gameObject.GetComponentInParent<Health.Health>().GetDamagedInstantly(damage);

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