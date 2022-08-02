using CustomUtils;
using DefaultNamespace;
using UnityEngine;
using SceneManager = CustomUtils.SceneManager;

namespace UserInterface {
    public class MainMenu : MonoBehaviour{
        public void OnPlay() {
            SceneManager.LoadScene("PlayScene");
            ReferenceTable.GameManager.SetState(GameManager.GameState.CutScene);
        }
        
        public void OnExit() {
            Application.Quit();
        }
    }
}