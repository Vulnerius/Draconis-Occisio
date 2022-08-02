using CustomUtils;
using DefaultNamespace;
using UnityEngine;
using SceneManager = CustomUtils.SceneManager;

namespace UserInterface {
    public class MainMenu : MonoBehaviour{
        public void OnTutorial() {
            SceneManager.LoadScene("PlayScene");
            ReferenceTable.GameManager.SetState(GameManager.GameState.Tutorial);
        }
        public void OnPlay() {
            ReferenceTable.GameManager.EnableGameModeUI();
        }
        
        public void OnExit() {
            Application.Quit();
        }
    }
}