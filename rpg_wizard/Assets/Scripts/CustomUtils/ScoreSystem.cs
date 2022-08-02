using System;

namespace CustomUtils {
    [Serializable]
    public class ScoreSystem {
        private float currentScore;

        public ScoreSystem() {
            currentScore = .0f;
        }
        
        public void IncreaseScore(float amount) {
            currentScore += amount;
        }

        public float GetScore() => currentScore;
    }
}