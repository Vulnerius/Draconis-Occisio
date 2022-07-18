using System;
using System.Collections;
using UnityEngine;

namespace Enemy {
    public class Enemy : MonoBehaviour {
        [SerializeField] private Health.Health health;

        private EnemyStates state;
        public EnemyStates State => state;

        private void Awake() {
            state = new();
        }

        private void Update() {
            if (health.CurrentHealth <= 0)
                state.isDead = true;
        }

        public void GettingHit() {
            state.isHit = true;
        }
        
        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.GetComponent<Health.Health>() == null) return;
            
            other.gameObject.GetComponent<Health.Health>().GetDamagedInstantly(75);
        }

        private void OnDestroy() {
            StopAllCoroutines();
        }
    }

    public class EnemyStates {
        public bool isAttackingMelee, isAttackingRanged, isWalking, isDead, isHit;
    }
}