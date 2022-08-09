using System;

namespace CustomUtils {
    public class ScoreSystem : IComparable<ScoreSystem> {
        public int CurrentScore;

        public ScoreSystem() {
            CurrentScore = 0;
        }
        
        public void IncreaseScore(int amount) {
            CurrentScore += amount;
        }

        public int GetScore() => CurrentScore;
        
        /// <summary>
        /// implementation of IComparable CompareTo()
        /// comparing this to other
        /// if other is greater than this, the spot in array will be replaced by other
        /// </summary>
        /// <param name="other">the ScoreSystem to be compared to</param>
        /// <returns>
        ///  0: the CurrentScores are the same
        ///  1: the CurrentScore of other is less than this or null - resulting in higher index
        /// -1: the CurrentScore of other is higher than this - resulting in lower index
        /// </returns>
        public int CompareTo(ScoreSystem other) {
            if (ReferenceEquals(this, other)) return 0;
            return ReferenceEquals(null, other) ? 1 : other.CurrentScore.CompareTo(CurrentScore);
        }
    }
}