using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Enemy {
    [Serializable]
    public class FieldOfView {
        [field: Header("Visibility"), SerializeField]
        public float SeeRadius { get; private set; }

        [field: SerializeField, Range(0, 360)] public float Angle { get; private set; }

        [field: Header("Bump"), SerializeField]
        public float BumpRadius { get; private set; }

        [field: Header("References")] [SerializeField]
        private GameObject _enemyRef;

        [SerializeField] private LayerMask _targetMask;

        [field: Header("Outputs"),
                SerializeField]
        public bool CanSeePlayer { get; set; }

        private EnemyStates enemyState;
        
        [field: SerializeField] public float SeenTimer { get; private set; } = 0;
        
        
        public IEnumerator FOVRoutine() {
            enemyState = _enemyRef.GetComponent<Enemy>().State;
            WaitForSeconds wait = new WaitForSeconds(0.2f);
            
            while (!enemyState.isDead) {
                FOVCheck();
                yield return wait;
            }
        }

        private Collider[] collArray;
        private int count;

        private void FOVCheck() {
            CalcDetectTime();
            collArray = new Collider[5];

            count = Physics.OverlapSphereNonAlloc(_enemyRef.transform.position, SeeRadius, collArray,
                _targetMask,
                QueryTriggerInteraction.Collide);

            if (count == 0) return;

            Collider playerCollider = collArray[0];
            Transform selfTransform = _enemyRef.transform;
            Vector3 playerPosition = playerCollider.transform.position;
            Vector3 selfPosition = selfTransform.position;

            Vector3 directionToTarget = (playerPosition - selfPosition).normalized;
            float distance = Vector3.Distance(playerPosition, selfPosition);

            CanSeePlayer = CheckAngle(selfTransform.forward, directionToTarget) && distance <= SeeRadius;
            if (!CanSeePlayer) CanSeePlayer = CheckForBumpDistance(distance);
            
            enemyState.foundPlayer = CanSeePlayer;
        }

        private bool CheckForBumpDistance(float distance) {
            return distance <= BumpRadius;
        }

        private bool CheckAngle(Vector3 selfPositionForward, Vector3 directionToTarget) {
            return Vector3.Angle(selfPositionForward, directionToTarget) <= Angle / 2;
        }

        public void CalcDetectTime() {
            if (CanSeePlayer) {
                SeenTimer += Time.deltaTime;
            }
            else {
                SeenTimer = 0;
            }
        }
        
    }
}