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
            ReferenceTable.GameManager.sounds.OnPlayerHit(transform);
        }

        public void UpdateScore(int dragonNumber) {
            Debug.LogWarning("increasing score");
            score.IncreaseScore(CalculateIncrease(dragonNumber));
        }

        private float CalculateIncrease(int dragonNumber) {
            return dragonNumber switch {
                1 => 90 + 100 * Random.Range(0f, .3f),
                2 => 120 + 100 * Random.Range(0f, .45f),
                3 => 240 + 100 * Random.Range(0f, .75f),
                4 => 160 + 100 * Random.Range(0f, .45f),
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