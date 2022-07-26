using System;
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
            if (health.CurrentHealth <= 0) {
                StartCoroutine(SetThisDead());
            }
            if(!state.isDead)
                StartCoroutine(fov.FOVRoutine());
        }

        private IEnumerator SetThisDead() {
            state.isDead = true;
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
        }

        public void GettingHit() {
            StartCoroutine(GetHit());
        }

        private IEnumerator GetHit() {
            state.isHit = true;
            yield return new WaitForSeconds(.8f);
            state.isHit = false;
        }

        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.GetComponent<Health.Health>() == null) return;
            
            other.gameObject.GetComponent<Health.Health>().GetDamagedInstantly(75);
        }
        
        private void OnTriggerStay(Collider other) {
            if(other.gameObject.GetComponent<Health.Health>() == null) return;
            
            other.gameObject.GetComponent<Health.Health>().GetDamagedOverTime(25,2.3f);
        }

        private void OnDisable() {
            OnDestroy();
        }

        private void OnDestroy() {
            StopAllCoroutines();
        }
    }
    
    public class EnemyStates {
        public bool isAttackingMelee, isAttackingRanged, isWalking, isDead, isHit, foundPlayer;
    }
}