using System.Collections;
using CustomUtils;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy {
    public class EnemyAttacks : MonoBehaviour {
        [SerializeField] private GameObject fireBall;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private int shootFrequency;

        private int currentPathIdx;
        private GameObject player;
        private Enemy self;
        private EnemyStates state;

        private float timer;

        void Start() {
            var colliderOffset = GetComponent<EnemyMeleeAnimations>()
                ? new Vector3(0, 2.2f, 0)
                : new Vector3(0, agent.baseOffset + .2f, 0);

            //GetComponent<BoxCollider>().center = colliderOffset;
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

        private bool DeathCheck() {
            return agent.isStopped = state.isDead;
        }

        private void CheckForAttack() {
            CheckForShoot();
            CheckForMelee();
        }

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

        private void CheckForShoot() {
            if (state.foundPlayer && timer > shootFrequency && self.fov.SeenTimer >= 45f)
                StartCoroutine(ShootFireBall());
        }

        private void CheckDestination() {
            var position = transform.position;
            var selfPosition = new Vector3(position.x, position.y - agent.baseOffset, position.z);

            if (Vector3.Distance(selfPosition, self.path[currentPathIdx]) <= .5)
                SetNextDestination();
        }

        private void SetNextDestination() {
            if (agent.isStopped) return;
            if (currentPathIdx == self.path.Count - 1)
                currentPathIdx = 0;
            else
                currentPathIdx++;

            agent.SetDestination(self.path[currentPathIdx]);
        }

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

        private IEnumerator AttackMelee() {
            gameObject.transform.LookAt(player.transform.position);
            yield return new WaitForSeconds(.8f);
            state.isAttackingMelee = false;
            state.isWalking = true;
        }
    }
}