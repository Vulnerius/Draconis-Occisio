using System.Collections;
using CustomUtils;
using UnityEngine;

namespace Player {
    public class Controller : MonoBehaviour {
        private Animator animator;
        private Health.Health helf;

        private void Awake() {
            animator = GetComponentInChildren<Animator>();
            helf = GetComponent<Health.Health>();
        }

        private void Update() {
            if (helf.CurrentHealth > 0) return;
            PlayerAnimationState.isDead = true;
            ReferenceTable.GameManager.SetState(GameManager.GameState.Defeat);
        }
        
        public void GotHit() {
            if(!PlayerAnimationState.isDead)
                animator.Play("GetHit");
        }
    }
}