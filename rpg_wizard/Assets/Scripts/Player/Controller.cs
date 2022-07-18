using System.Collections;
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
            if (helf.CurrentHealth <= 0)
                PlayerAnimationState.isDead = true;
        }
        
        public void GotHit() {
            animator.Play("GetHit");
        }
    }
}