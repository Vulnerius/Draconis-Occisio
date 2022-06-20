using UnityEngine;

namespace Projectiles {
    public class FireBallPlayer : MonoBehaviour {
        [SerializeField] private int damage;
        [SerializeField] private float lifeTime;
        [SerializeField] private float moveSpeed;

        private GameObject attacker;

        private void Awake() {
            attacker = GameObject.FindWithTag("Player");
            
            Destroy(gameObject,lifeTime);
            gameObject.GetComponent<Rigidbody>().AddForce(moveSpeed * Camera.main!.transform.forward, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.GetComponent<Health.Health>() == null) return;

            other.gameObject.GetComponent<Health.Health>().GetDamagedInstantly(damage);

            var enemy = other.gameObject.GetComponent<Enemy.Enemy>();
            if(enemy == null) return;
            enemy.GettingHit();
        }
            
    }
}