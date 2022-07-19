using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy {
    public class EnemyAttacks : MonoBehaviour
    {
        [SerializeField] private GameObject fireBall;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private int shootFrequency;

        private int currentIdx;
        private GameObject player;
        private Enemy self;
        private EnemyStates state;

        private float timer;
    
        void Start() {
            player = ReferenceTable.Player;
        }

        private void Awake() {
            self = GetComponent<global::Enemy.Enemy>();
            state = self.State;
            state.isWalking = true;
            SetNextDestination();
        }

        private void Update() {
            timer += Time.deltaTime;
        
            CheckDestination();
            CheckForAttack();
        }

        private void CheckForAttack() {
            CheckForShoot();
            CheckForMelee();
        }

        private void CheckForMelee() {
            if (Vector3.Distance(transform.position, player.transform.position) < self.fov.BumpRadius) {
                state.isAttackingMelee = true;
                state.isWalking = false;
                StartCoroutine(AttackMelee());
            }
            
        }

        private void CheckForShoot() {
            if (state.foundPlayer && timer > shootFrequency && self.fov.SeenTimer >= 45f)
                StartCoroutine(ShootFireBall());
        }

        private void CheckDestination() {
            if(Vector3.Distance(transform.position, self.path[currentIdx]) <= .5)
                SetNextDestination();
        }
    
        private void SetNextDestination() {
            if(agent.isStopped) return;
            if (currentIdx == self.path.Count - 1)
                currentIdx = 0;
            else
                currentIdx++;
        
            agent.SetDestination(self.path[currentIdx]);
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
        }
    }
}
