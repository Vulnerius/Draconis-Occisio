using System;
using UnityEngine;

namespace Projectiles {
    public class FireBallPlayer : MonoBehaviour {
        [SerializeField] private int damage;
        [SerializeField] private float moveSpeed;
        private Vector3 _forWard;

        private void Awake() {
            _forWard = Camera.main!.transform.forward;
            _forWard.y *= 0;
        }

        private void Update() {
            var moveDir = moveSpeed * (_forWard * Time.deltaTime);
            transform.parent.Translate(moveDir);
        }

        private void OnTriggerEnter(Collider other) {
            Debug.LogWarning(other.name);
            if (other.gameObject.GetComponent<Health.Health>() == null) return;

            other.gameObject.GetComponent<Health.Health>().GetDamagedInstantly(damage);

            var enemy = other.gameObject.GetComponent<Enemy.Enemy>();
            if (!enemy) return;
            enemy.GettingHit();
        }
    }
}