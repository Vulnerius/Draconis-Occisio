using System;
using System.Collections;
using DefaultNamespace;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CustomUtils {
    public class GameManager : MonoBehaviour {
        public enum GameState {
            Tutorial,
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

        private static GameState currentState;
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
            
            if (Keyboard.current.escapeKey.wasPressedThisFrame && SceneManager.CurrentScene ==
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
                    PlayerAnimationState.isDead = false;
                    pauseMenu.SetActive(false);
                    CursorManager.SetCursor(CursorManager.CursorEvent.Visible);
                    SceneManager.LoadScene("MainMenu");
                    currentEnemyIdx = 0;
                    break;
                case GameState.CutScene:
                    SetState(GameState.Fight);
                    StartCoroutine(SpawnNewDragon());
                    break;
                case GameState.Defeat:
                    playerDefeated.SetActive(true);
                    StartCoroutine(WaitForInput());
                    ResetPlayerProperties();
                    break;
            }
        }

        private void ResetPlayerProperties() {
            ReferenceTable.Player.GetComponent<Health.Health>().ResetHealth();
        }

        private IEnumerator WaitForInput() {
            yield return new WaitUntil(() =>
                Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame);
            
            playerDefeated.SetActive(false);
            SetState(GameState.Start);
        }

        private void FixedUpdate() {
            if(!ReferenceTable.CurrentEnemy) return;
            if(!ReferenceTable.CurrentEnemy.gameObject.activeSelf)
                StartCoroutine(SpawnNewDragon());
        }

        private IEnumerator SpawnNewDragon() {
            Destroy(ReferenceTable.CurrentEnemy);
            while (UnityEngine.SceneManagement.SceneManager.GetActiveScene() != SceneManager.CurrentScene) {
                yield return new WaitForSeconds(.4f);
            }
            
            ReferenceTable.DragonSpawner.SpawnDragon(++currentEnemyIdx);
            yield return new WaitForFixedUpdate();
        }
    }
}