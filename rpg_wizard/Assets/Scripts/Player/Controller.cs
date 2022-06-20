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
                StartCoroutine(Die());
        }

        private IEnumerator Die() {
            animator.Play("Die");
            yield return new WaitForSeconds(3f);
            //TODO: go to defeat screen
        }

        public void GotHit() {
            animator.Play("GetHit");
        }
    }
}