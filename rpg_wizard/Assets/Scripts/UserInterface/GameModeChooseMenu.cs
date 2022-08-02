using CustomUtils;
using UnityEngine;

namespace UserInterface {
    public class GameModeChooseMenu : MonoBehaviour{
        private void StandardBehaviour() {
            ReferenceTable.GameManager.SetState(GameManager.GameState.CutScene);
            ReferenceTable.GameManager.gameModeChoose.SetActive(false);
            SceneManager.LoadScene("PlayScene");
        }
        
        public void OnRandom() {
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Random);
            StandardBehaviour();
        }
        
        public void OnSoloDragon1() {
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
            ReferenceTable.GameManager.currentEnemyIdx = 1;
            StandardBehaviour();
        }
        
        public void OnSoloDragon2() {
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
            ReferenceTable.GameManager.currentEnemyIdx = 2;
            StandardBehaviour();
        }
        
        public void OnSoloDragon3() {
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
            ReferenceTable.GameManager.currentEnemyIdx = 3;
            StandardBehaviour();
        }
        
        public void OnSoloDragon4() {
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
            ReferenceTable.GameManager.currentEnemyIdx = 4;
            StandardBehaviour();
        }
    }
}