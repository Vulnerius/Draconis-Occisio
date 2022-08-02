using System.Collections;
using TMPro;
using UnityEngine;

namespace CustomUtils {
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

        public void Reset() {
            maxTimer = maxTimerRef;
            fightTimer = fightTimerRef;
        }
        public void OnFight() {
            maxTimerText.text = $"{maxTimer:0.00}s";
            StartCoroutine(CountDownFightTimer());
        }

        public void OnPause() {
            StopCoroutine(CountDownFightTimer());
        }
        
        private IEnumerator CountDownFightTimer() {
            while (fightTimer > 0) {
                fightTimerText.text = $"{fightTimer:0.00}s";
                yield return new WaitForSeconds(1f);
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