using System;
using System.Collections;
using DefaultNamespace;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using SceneManager = DefaultNamespace.SceneManager;

namespace CustomUtils {
    public class GameManager : MonoBehaviour {
        public enum GameState {
            Start,
            Pause,
            Fight,
            Win,
            Defeat,
            CutScene
        }

        [SerializeField] public GameObject pauseMenu;
        [SerializeField] public GameObject dragonDefeated;
        [SerializeField] public GameObject playerDefeated;
        public CineMachineSwitcher switcher;

        public static GameState currentState;
        public int currentEnemyIdx;

        private void Awake() {
            currentState = GameState.Start;

            if (ReferenceTable.GameManager == null)
                ReferenceTable.GameManager = this;
            else if(ReferenceTable.GameManager != this)
                Destroy(gameObject);
            
            DontDestroyOnLoad(gameObject);
        }

        void Update() {
            gameObject.SetActive(true);
            if (Keyboard.current.fKey.wasPressedThisFrame)
                switcher.SwitchLock();
            
            if (Keyboard.current.escapeKey.wasPressedThisFrame && SceneManager.currentScene ==
                UnityEngine.SceneManagement.SceneManager.GetSceneByName("PlayScene"))
                SetState(currentState == GameState.Fight ? GameState.Pause : GameState.Fight);
        }

        public void SetState(GameState newState) {
            currentState = newState;
            SwitchGameState();
        }

        private void SwitchGameState() {
            switch (currentState) {
                case GameState.Pause:
                    pauseMenu.SetActive(true);
                    CursorManager.SetCursor(CursorManager.CursorEvent.Visible);
                    Time.timeScale = 0;
                    break;
                case GameState.Fight:
                    CursorManager.SetCursor(CursorManager.CursorEvent.Invisible);
                    Time.timeScale = 1;
                    if(pauseMenu)
                        pauseMenu.SetActive(false);
                    break;
                case GameState.Start:
                    pauseMenu.SetActive(false);
                    CursorManager.SetCursor(CursorManager.CursorEvent.Visible);
                    SceneManager.LoadScene("MainMenu");
                    Time.timeScale = 0;
                    break;
                case GameState.CutScene:
                    SetState(GameState.Fight);
                    StartCoroutine(SpawnNewDragon());
                    break;
            }
        }

        private void FixedUpdate() {
            //TODO: WIN / DEFEAT-Screen goes here
            if(!ReferenceTable.CurrentEnemy) return;
            if(!ReferenceTable.CurrentEnemy.gameObject.activeSelf)
                StartCoroutine(SpawnNewDragon());
        }

        private IEnumerator SpawnNewDragon() {
            while (UnityEngine.SceneManagement.SceneManager.GetActiveScene() != SceneManager.currentScene) {
                yield return new WaitForSeconds(.4f);
            }

            Debug.LogWarning("Dragon instantiated");
            ReferenceTable.DragonSpawner.SpawnDragon(++currentEnemyIdx);
            yield return new WaitForFixedUpdate();
        }
    }
}