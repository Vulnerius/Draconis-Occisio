using Player;
using UnityEngine;

namespace Enemy {
    public class FireBallDragon : MonoBehaviour {
        [SerializeField] private int damage;

        [SerializeField] private float lifeTime;

        [SerializeField] private float moveSpeed;
        private Transform m_Transform;
    
        private void Awake() {
            m_Transform = GameObject.FindWithTag("Enemy").transform;
            GetComponent<Rigidbody>().AddForce(moveSpeed * m_Transform.forward, ForceMode.Impulse);
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter(Collider other) {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            if(other.gameObject.layer == LayerMask.NameToLayer("Shield")) Destroy(gameObject);
            
            if (other.gameObject.GetComponentInParent<Health.Health>() == null) return;

            other.gameObject.GetComponentInParent<Health.Health>().GetDamagedInstantly(damage);

            var playerController = other.gameObject.GetComponentInParent<Controller>();
            if(playerController == null) return;
            playerController.GotHit();
        }
    }
}