using CustomUtils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player {
    /// <summary>
    /// basic controls for Player like
    /// Health
    /// GetHit-Animation
    /// Score
    /// </summary>
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
            if (PlayerAnimationState.isDead) return;
            animator.Play("GetHit");
        }

        public void UpdateScore(int dragonNumber) {
            score.IncreaseScore(CalculateIncrease(dragonNumber));
        }

        private int CalculateIncrease(int dragonNumber) {
            return dragonNumber switch {
                1 => (int) (90 + 100 * Random.Range(0f, .3f)),
                2 => (int) (120 + 100 * Random.Range(0f, .45f)),
                3 => (int) (240 + 100 * Random.Range(0f, .75f)),
                4 => (int) (160 + 100 * Random.Range(0f, .45f)),
                _ => 0
            };
        }

        public string GetScore() {
            return $"{score.GetScore()}";
        }
        public ScoreSystem GetScoreSystem() {
            return score;
        }

        public void ResetScore() {
            score = new ScoreSystem();
        }
    }
}