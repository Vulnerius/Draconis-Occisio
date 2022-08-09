using System.Collections;
using System.Collections.Generic;
using CustomUtils;
using UnityEngine;
using UnityEngine.VFX;

namespace Enemy {
    /// <summary>
    /// basic enemy behaviour
    /// </summary>
    public class Enemy : MonoBehaviour {
        [SerializeField] private Health.Health health;
        [SerializeField] private VisualEffect impactEffect;
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
            ReferenceTable.GameManager.UpdateScore();
            gameObject.SetActive(false);
        }

        public void GettingHit(Vector3 contactPoint) {
            PlayHitVFX(contactPoint);
            StartCoroutine(GetHit());
        }

        private void PlayHitVFX(Vector3 contactPoint) {
            var impact = Instantiate(impactEffect, contactPoint, Quaternion.identity);
            impact.Play();
            Destroy(impact.gameObject, 1);
        }

        private IEnumerator GetHit() {
            state.isHit = true;
            yield return new WaitForSeconds(.8f);
            state.isHit = false;
        }

        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.GetComponent<Health.Health>() == null) return;
            
            other.gameObject.GetComponent<Health.Health>().GetDamagedOverTime(65,1);
        }
        
        private void OnTriggerStay(Collider other) {
            if(other.gameObject.GetComponent<Health.Health>() == null) return;
            
            other.gameObject.GetComponent<Health.Health>().GetDamagedOverTime(5,1f);
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