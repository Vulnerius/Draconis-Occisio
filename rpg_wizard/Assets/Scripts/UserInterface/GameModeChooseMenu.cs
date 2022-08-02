using CustomUtils;
using UnityEngine;

namespace UserInterface {
    public class GameModeChooseMenu : MonoBehaviour{
        private void StandardBehaviour() {
            ReferenceTable.GameManager.gameModeChoose.SetActive(false);
            SceneManager.LoadScene("PlayScene");
        }
        
        public void OnRandom() {
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Random);
        }
        
        public void OnSoloDragon1() {
            ReferenceTable.GameManager.currentEnemyIdx = 1;
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
        }
        
        public void OnSoloDragon2() {
            ReferenceTable.GameManager.currentEnemyIdx = 2;
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
        }
        
        public void OnSoloDragon3() {
            ReferenceTable.GameManager.currentEnemyIdx = 3;
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
        }
        
        public void OnSoloDragon4() {
            ReferenceTable.GameManager.currentEnemyIdx = 4;
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
        }
    }
}