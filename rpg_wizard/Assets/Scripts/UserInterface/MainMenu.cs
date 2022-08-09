using CustomUtils;
using UnityEngine;
using SceneManager = CustomUtils.SceneManager;

namespace UserInterface {
    /// <summary>
    /// behaviour for MainMenu
    /// </summary>
    public class MainMenu : MonoBehaviour {

        /// <summary>
        /// Loading PlayScene
        /// Setting GameMode to Tutorial
        /// </summary>
        public void OnTutorial() {
            SceneManager.LoadScene("PlayScene");
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Tutorial);
        }
        
        /// <summary>
        /// Enabling GameModeChooseMenu
        /// </summary>
        public void OnPlay() {
            ReferenceTable.GameManager.EnableGameModeUI();
        }
        
        /// <summary>
        /// Enabling OptionsMenu
        /// </summary>
        public void OnOptions() {
            ReferenceTable.GameManager.EnableOptionsMenu();
        }

        /// <summary>
        /// Quitting the Game
        /// </summary>
        public void OnExit() {
            Application.Quit();
        }
    }
}