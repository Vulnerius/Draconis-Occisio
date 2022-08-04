using System;
using UnityEngine;

namespace Projectiles {
    public class FireBallPlayer : MonoBehaviour {
        [SerializeField] private int damage;
        [SerializeField] private float moveSpeed;
        
        private void Update() {
            var moveDir = moveSpeed * (Camera.main!.transform.forward * Time.deltaTime);
            transform.parent.Translate(moveDir);
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.GetComponent<Health.Health>() == null) return;

            other.gameObject.GetComponent<Health.Health>().GetDamagedOverTime(damage,1);

            var enemy = other.gameObject.GetComponent<Enemy.Enemy>();
            if (!enemy) return;
            enemy.GettingHit();
        }
    }
}