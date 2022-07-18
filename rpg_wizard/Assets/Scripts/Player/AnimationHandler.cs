using System;
using UnityEngine;

namespace Player {
    public class AnimationHandler : MonoBehaviour {
        
        private static readonly int Idle = Animator.StringToHash("Idle03"); 
        private static readonly int Walk = Animator.StringToHash("WalkForward"); 
        private static readonly int Run = Animator.StringToHash("BattleRunForward"); 
        private static readonly int Attack = Animator.StringToHash("Attack01"); 
        private static readonly int Defend = Animator.StringToHash("DefendStart"); 
        private static readonly int Jump = Animator.StringToHash("JumpStart");
        private static readonly int Die = Animator.StringToHash("Die");
        
        [Header("References")]
        [SerializeField] private Animator playerAnimator;

        [Header("Settings")]
        [SerializeField] private int dieAnimationDuration;
        [SerializeField] private int attackAnimationDuration;
        [SerializeField] private int defendAnimationDuration;
        [SerializeField] private int jumpAnimationDuration;
        private int _currentState;
        private float _lockedTill;

        private void Update() {
            var state = GetState();
            
            if(state == _currentState) return;
            playerAnimator.CrossFade(state, .3f, 0);
            _currentState = state;
        }

        private int GetState() {
            if (Time.time < _lockedTill) return _currentState;
            
            if(PlayerAnimationState.isDead)
                return LockState(Die, dieAnimationDuration);
            
            if(PlayerAnimationState.isAttacking)
                return LockState(Attack, attackAnimationDuration);
            
            if(PlayerAnimationState.isDefending)
                return LockState(Defend, defendAnimationDuration);
            
            if(PlayerAnimationState.isJumping)
                return LockState(Jump, jumpAnimationDuration);
            
            if(PlayerAnimationState.isRunning)
                return Run;

            return PlayerAnimationState.isWalking ? Walk : Idle;
            
            int LockState(int s, float t) {
                _lockedTill = Time.time + t;
                return s;
            }
            
        }
    }
    
    public static class PlayerAnimationState {
        public static bool isWalking, isJumping, isRunning, isAttacking, isDefending, isDead;
    }
    
    public class PlayerStates {
        public Vector2 move;
        public float TurnVelocity;
        public bool moveEnabled;
        public bool jumped;
        public bool ability;
    }
}
