using UnityEngine;

namespace Projectiles {
    public class FireBall : MonoBehaviour {
        [SerializeField] private int damage;
        [SerializeField] private float lifeTime;
        [SerializeField] private float moveSpeed;

        private GameObject attacker;

        private void Awake() {
            attacker = GameObject.FindWithTag("Player");
            
            Destroy(gameObject,lifeTime);
            gameObject.GetComponent<Rigidbody>().AddForce(moveSpeed * attacker.transform.forward, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider collider) {
            collider.gameObject.GetComponent<Health>().GetDamagedInstantly(damage);
            Debug.Log(collider.gameObject.GetComponent<Health>().CurrentHealth);
        }
            
    }
}