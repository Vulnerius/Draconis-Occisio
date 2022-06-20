using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Player {
    public class Controller : MonoBehaviour {
        private Animator Animator;

        private void Awake() {
            Animator = GetComponentInChildren<Animator>();
        }

        public void GotHit() {
            Animator.Play("GetHit");
        }
    }
}