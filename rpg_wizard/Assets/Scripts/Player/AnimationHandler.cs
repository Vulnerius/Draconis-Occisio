using UnityEngine;

namespace Player {
    /// <summary>
    /// Animation handling for the wizard
    /// </summary>
    public class AnimationHandler : MonoBehaviour {
        
        private static readonly int Idle = Animator.StringToHash("Idle03"); 
        private static readonly int Walk = Animator.StringToHash("WalkForward"); 
        private static readonly int Run = Animator.StringToHash("BattleRunForward"); 
        private static readonly int Attack = Animator.StringToHash("Attack01"); 
        private static readonly int Defend = Animator.StringToHash("DefendStart"); 
        private static readonly int Jump = Animator.StringToHash("JumpStart");
        private static readonly int Fly = Animator.StringToHash("JumpAir");
        private static readonly int Die = Animator.StringToHash("Die");

        [Header("References")]
        [SerializeField] private Animator playerAnimator;

        [Header("Settings")]
        [SerializeField] private float dieAnimationDuration;
        [SerializeField] private float attackAnimationDuration;
        [SerializeField] private float defendAnimationDuration;
        [SerializeField] private float jumpAnimationDuration;
        private int _currentState;
        private float _lockedTill;

        private void Update() {
            var state = GetState();
            
            if(state == _currentState) return;
            playerAnimator.CrossFade(state, .3f, 0);
            _currentState = state;
        }

        /// <summary>
        /// concept by https://www.youtube.com/watch?v=ZwLekxsSY3Y&ab_channel=Tarodev
        /// checking states playing Animations accordingly
        /// </summary>
        /// <returns>duration of current Animation</returns>
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
            
            if(PlayerAnimationState.isLevitating)
                return Fly;
            
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
        public static bool isWalking, isJumping, isRunning, isAttacking, isDefending, isDead, isLevitating;
    }
    
    public class PlayerStates {
        public Vector2 move;
        public float TurnVelocity;
        public bool moveEnabled;
        public bool ability;
    }
}
