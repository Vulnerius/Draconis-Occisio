using CustomUtils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player {
    public class Controller : MonoBehaviour {
        private Animator animator;
        private Health.Health helf;
        private ScoreSystem score;

        private void Awake() {
            animator = GetComponentInChildren<Animator>();
            helf = GetComponent<Health.Health>();
            score = new ScoreSystem();
        }

        private void Update() {
            if (helf.CurrentHealth > 0) return;
            PlayerAnimationState.isDead = true;
            ReferenceTable.GameManager.SetState(GameManager.GameState.Defeat);
        }

        public void GotHit() {
            if (!PlayerAnimationState.isDead)
                animator.Play("GetHit");
        }

        public void UpdateScore(int dragonNumber) {
            score.IncreaseScore(CalculateIncrease(dragonNumber));
        }

        private float CalculateIncrease(int dragonNumber) {
            return dragonNumber switch {
                1 => 90 + 100 * Random.Range(0f, .3f),
                2 => 90 + 100 * Random.Range(0f, .45f),
                3 => 90 + 100 * Random.Range(0f, .75f),
                4 => 90 + 100 * Random.Range(0f, .45f),
                _ => 0f
            };
        }

        public string GetScore() {
            return $"{score.GetScore(): 0.00}";
        }

        public void ResetScore() {
            score = new ScoreSystem();
        }
    }
}