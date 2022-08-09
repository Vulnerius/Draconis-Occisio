using System.Collections;
using CustomUtils;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy {
    /// <summary>
    /// class for Enemy Attacks
    /// </summary>
    public class EnemyAttacks : MonoBehaviour {
        [SerializeField] private GameObject fireBall;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private int shootFrequency;

        private int currentPathIdx;
        private GameObject player;
        private Enemy self;
        private EnemyStates state;

        private float timer;

        private void Start() {
            player = ReferenceTable.Player;
        }

        private void Awake() {
            timer = shootFrequency;
            self = GetComponent<Enemy>();
            state = self.State;
            state.isWalking = true;
            SetNextDestination();
        }

        private void Update() {
            timer += Time.deltaTime;

            if (DeathCheck()) return;
            CheckDestination();
            CheckForAttack();
        }

        private void FixedUpdate() {
            agent.isStopped = state.foundPlayer;
        }

        /// <summary>
        /// setting agent.isStopped to state.isDead
        /// </summary>
        /// <returns>state.isDead</returns>
        private bool DeathCheck() {
            return agent.isStopped = state.isDead;
        }

        private void CheckForAttack() {
            CheckForShoot();
            CheckForMelee();
        }

        /// <summary>
        /// on world coordinates:
        ///     if the distance between the player and this is less than the bumpRadius
        ///         perform a melee attack
        ///     else
        ///         do NavMesh movement
        /// </summary>
        private void CheckForMelee() {
            if (Vector3.Distance(transform.position, player.transform.position) < self.fov.BumpRadius) {
                agent.isStopped = true;
                state.isAttackingMelee = true;
                state.isWalking = false;
                StartCoroutine(AttackMelee());
            }
            else {
                state.isWalking = true;
                agent.isStopped = false;
            }
        }

        /// <summary>
        /// shooting a fireBall if:
        ///     player in fovRadius, timer is greater or Equal to shootFrequency and SeenTimer passed half a second-ish mark
        /// </summary>
        private void CheckForShoot() {
            if (state.foundPlayer && timer >= shootFrequency && self.fov.SeenTimer >= 45f)
                StartCoroutine(ShootFireBall());
        }

        /// <summary>
        /// calculates the distance between the destination and the current position
        /// sets NextDestination is the distance is below .5 threshold
        /// </summary>
        private void CheckDestination() {
            var position = transform.position;
            var selfPosition = new Vector3(position.x, position.y - agent.baseOffset, position.z);

            if (Vector3.Distance(selfPosition, self.path[currentPathIdx]) <= .5)
                SetNextDestination();
        }

        /// <summary>
        /// sets the agents next destination
        /// </summary>
        private void SetNextDestination() {
            if (agent.isStopped) return;
            if (currentPathIdx == self.path.Count - 1)
                currentPathIdx = 0;
            else
                currentPathIdx++;

            agent.SetDestination(self.path[currentPathIdx]);
        }

        /// <summary>
        /// resetting the timer
        /// stopping NavMeshAgent movement
        /// performing ranged-attack-animation
        /// Instantiating the fireBall/tornado
        /// resuming NavMeshAgent movement
        /// resuming Walking/Flying Animation
        /// </summary>
        /// <returns>Waiting for completed action</returns>
        private IEnumerator ShootFireBall() {
            if (state.isAttackingMelee) yield break;
            timer = 0;
            state.isWalking = false;
            state.isAttackingRanged = true;
            agent.isStopped = true;

            yield return new WaitForSeconds(.2f);
            gameObject.transform.LookAt(player.transform.position);
            Vector3 instantiatePoint = transform.position + 7 * transform.forward + 2 * Vector3.up;
            yield return new WaitForSeconds(.4f);

            Instantiate(fireBall, instantiatePoint, Quaternion.identity);

            yield return new WaitForSeconds(1.2f);
            state.isAttackingRanged = false;
            state.isWalking = true;
            agent.isStopped = false;
        }

        /// <summary>
        /// rotating this towards the player
        /// performing melee attack
        /// returning to movement
        /// </summary>
        /// <returns>Waiting .8 seconds</returns>
        private IEnumerator AttackMelee() {
            gameObject.transform.LookAt(player.transform.position);
            yield return new WaitForSeconds(.8f);
            state.isAttackingMelee = false;
            state.isWalking = true;
        }
    }
}