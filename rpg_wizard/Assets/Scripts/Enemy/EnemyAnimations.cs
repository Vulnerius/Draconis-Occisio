using System;
using UnityEngine;

namespace Enemy {
    public class EnemyAnimations : MonoBehaviour {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int RangedAttack = Animator.StringToHash("Scream");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int MeleeAttack = Animator.StringToHash("Basic Attack");
        private static readonly int GetHit = Animator.StringToHash("Get Hit");

        [Header("References")] [SerializeField]
        private GameObject self;

        [SerializeField] private Animator animator;

        [Header("Settings")] [SerializeField] private float dieAnimationDuration;
        [SerializeField] private float meleeAttackAnimationDuration;
        [SerializeField] private float rangedAttackAnimationDuration;
        [SerializeField] private float getHitAnimationDuration;

        private EnemyStates selfState;
        private int _currentState;
        private float _lockedTill;

        private void Awake() {
            selfState = self.GetComponent<Enemy>().State;
        }

        private void Update() {
            var state = GetState();

            if (state == _currentState) return;
            animator.CrossFade(state, .3f, 0);
            _currentState = state;
        }

        private int GetState() {
            if (Time.time < _lockedTill) return _currentState;

            if (selfState.isDead)
                return LockState(Die, dieAnimationDuration);

            if (selfState.isAttackingMelee)
                return LockState(MeleeAttack, meleeAttackAnimationDuration);

            if (selfState.isAttackingRanged)
                return LockState(RangedAttack, rangedAttackAnimationDuration);
            
            if (selfState.isHit)
                return LockState(GetHit, getHitAnimationDuration);

            return selfState.isWalking ? Walk : Idle;

            int LockState(int s, float t) {
                _lockedTill = Time.time + t;
                return s;
            }
        }
    }
}