using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    public class Enemy : MonoBehaviour {
        [SerializeField] private Health.Health health;
        [SerializeField] public FieldOfView fov;
        [SerializeField] public List<Vector3> path;

        private EnemyStates state;
        public EnemyStates State => state;

        private void Awake() {
            state = new();
        }

        private void Update() {
            if (health.CurrentHealth <= 0)
                state.isDead = true;
            StartCoroutine(fov.FOVRoutine());
        }

        public void GettingHit() {
            StartCoroutine(GetHit());
        }

        private IEnumerator GetHit() {
            state.isHit = true;
            yield return new WaitForSeconds(1.3f);
            state.isHit = false;
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
        public bool isAttackingMelee, isAttackingRanged, isWalking, isDead, isHit, foundPlayer;
    }
}