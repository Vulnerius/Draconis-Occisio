using System.Collections;
using TMPro;
using UnityEngine;

namespace CustomUtils {
    /// <summary>
    /// timer functionality for GameLoop
    /// </summary>
    public class FightTimer : MonoBehaviour{
        [SerializeField] public double maxTimerRef;
        public double maxTimer;
        [SerializeField] public double fightTimerRef;
        public double fightTimer;
        [SerializeField] public TextMeshProUGUI fightTimerText;
        [SerializeField] public TextMeshProUGUI maxTimerText;

        private void Awake() {
            Reset();
        }

        /// <summary>
        /// resets maxTimer and fightTimer back to their referenced values
        /// </summary>
        public void Reset() {
            maxTimer = maxTimerRef;
            fightTimer = fightTimerRef;
        }
        /// <summary>
        /// setting the maxTimerText in the UI and Starting the Fighting Countdown
        /// </summary>
        public void OnFight() {
            maxTimerText.text = $"{maxTimer:0.00}s";
            StartCoroutine(CountDownFightTimer());
        }

        /// <summary>
        /// Stopping the Fighting Countdown
        /// </summary>
        public void OnPause() {
            StopCoroutine(CountDownFightTimer());
        }
        
        /// <summary>
        /// looping the fightTimer:
        ///     setting the fightTimer Text in the UI 
        ///     Waiting for 1.1 seconds
        ///     reducing it by 1 each loop
        /// if the Player was not able to kill the dragon, the maxTimer is Reduced and a new Dragon will be spawn
        /// </summary>
        /// <returns>seconds</returns>
        private IEnumerator CountDownFightTimer() {
            while (fightTimer > 0) {
                fightTimerText.text = $"{fightTimer:0.00}s";
                yield return new WaitForSeconds(1.1f);
                fightTimer -= 1;
            }

            ReduceMaxTimer();
            ReferenceTable.GameManager.SetState(GameManager.GameState.CutScene);
        }

        private void ReduceMaxTimer() {
            maxTimer -= fightTimerRef;
            if(maxTimer <= 0)
                ReferenceTable.GameManager.SetState(GameManager.GameState.Defeat);
        }
    }
}