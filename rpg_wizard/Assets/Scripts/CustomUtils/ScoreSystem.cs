using System;

namespace CustomUtils {
    public class ScoreSystem : IComparable<ScoreSystem> {
        public int currentScore;

        public ScoreSystem() {
            currentScore = 0;
        }
        
        public void IncreaseScore(int amount) {
            currentScore += amount;
        }

        public int GetScore() => currentScore;
        
        public int CompareTo(ScoreSystem other) {
            if (ReferenceEquals(this, other)) return 0;
            return ReferenceEquals(null, other) ? 1 : other.currentScore.CompareTo(currentScore);
        }
    }
}