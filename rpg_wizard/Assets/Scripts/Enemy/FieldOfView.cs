using System;
using System.Collections;
using UnityEngine;

namespace Enemy {
    /// <summary>
    /// FieldOfView implementation for enemy
    /// </summary>
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


        [field: SerializeField] public float SeenTimer { get; private set; } = 0;

        private EnemyStates enemyState;
        private Collider[] collArray;
        private int count;

        /// <summary>
        /// updating the state
        /// while gameObject is Alive perform FOVCheck
        /// </summary>
        /// <returns></returns>
        public IEnumerator FOVRoutine() {
            enemyState = _enemyRef.GetComponent<Enemy>().State;
            WaitForSeconds wait = new WaitForSeconds(0.2f);
            
            while (!enemyState.isDead) {
                FOVCheck();
                yield return wait;
            }
        }

        /// <summary>
        /// calculating detection Time
        /// instantiating Collider[]
        /// performing OverlappingColliderCount -> exiting function if 0
        /// calculating the distance between the player and self
        /// checking if the angle the player is inside the fov
        /// checking if the player is inside the bumpRadius
        /// </summary>
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
        
        /// <param name="selfPositionForward">self position world space</param>
        /// <param name="directionToTarget">target position world space</param>
        /// <returns>if the player is inside the fov-cone</returns>
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