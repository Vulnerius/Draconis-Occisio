using UnityEngine;

namespace Player {
    /// <summary>
    /// tornado behaviour after instantiation
    /// </summary>
    public class FireBallPlayer : MonoBehaviour {
        [SerializeField] private int damage;
        [SerializeField] private float moveSpeed;
        
        /// <summary>
        /// moveDirection of tornado based on camera forward vector
        /// </summary>
        private void Update() {
            var moveDir = moveSpeed * (Camera.main!.transform.forward * Time.deltaTime);
            transform.parent.Translate(moveDir);
        }

        /// <summary>
        /// reducing hit gameObject health after collision
        /// </summary>
        /// <param name="other">hit gameObject</param>
        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.GetComponent<Health.Health>() == null) return;

            other.gameObject.GetComponent<Health.Health>().GetDamagedOverTime(damage,1);

            var enemy = other.gameObject.GetComponent<Enemy.Enemy>();
            if (!enemy) return;
            enemy.GettingHit(other.GetContact(0).point);
        }
    }
}