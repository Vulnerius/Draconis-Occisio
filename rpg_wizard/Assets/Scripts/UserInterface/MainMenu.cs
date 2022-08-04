using System;
using CustomUtils;
using TMPro;
using UnityEngine;
using SceneManager = CustomUtils.SceneManager;

namespace UserInterface {
    public class MainMenu : MonoBehaviour {
        public void OnTutorial() {
            SceneManager.LoadScene("PlayScene");
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Tutorial);
        }
        public void OnPlay() {
            ReferenceTable.GameManager.EnableGameModeUI();
        }
        public void OnOptions() {
            ReferenceTable.GameManager.EnableOptionsMenu();
        }

        public void OnExit() {
            Application.Quit();
        }
    }
}