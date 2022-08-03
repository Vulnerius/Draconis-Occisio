using System.Collections;
using DefaultNamespace;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace CustomUtils {
    public class GameManager : MonoBehaviour {
        public enum GameMode {
            Tutorial,
            Solo,
            Random
        }

        public enum GameState {
            Start,
            Pause,
            Fight,
            Defeat,
            CutScene
        }

        [SerializeField] public GameObject optionsMenu;
        [SerializeField] public GameObject pauseMenu;
        [SerializeField] public GameObject gameModeChoose;
        [SerializeField] public GameObject tutorialWinScreen;
        [SerializeField] public FightTimer fightTimer;
        [SerializeField] public GameObject playerDefeated;
        [SerializeField] public TextMeshProUGUI scoreText;

        public CineMachineSwitcher switcher;

        private static GameState _currentState;
        private static GameMode _gameMode;
        public int currentEnemyIdx;

        private void Awake() {
            _currentState = GameState.Start;

            if (ReferenceTable.GameManager == null)
                ReferenceTable.GameManager = this;
            else if (ReferenceTable.GameManager != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        void Update() {
            gameObject.SetActive(true);
            if (Keyboard.current.fKey.wasPressedThisFrame)
                switcher.SwitchLock();

            if (Keyboard.current.escapeKey.wasPressedThisFrame && SceneManager.CurrentScene ==
                UnityEngine.SceneManagement.SceneManager.GetSceneByName("PlayScene"))
                SetState(_currentState == GameState.Fight ? GameState.Pause : GameState.Fight);
        }

        public void SetState(GameState newState) {
            _currentState = newState;
            SwitchGameState();
        }

        private void SwitchGameState() {
            switch (_currentState) {
                case GameState.Pause:
                    fightTimer.OnPause();
                    pauseMenu.SetActive(true);
                    CursorManager.SetCursor(CursorManager.CursorEvent.Visible);
                    Time.timeScale = 0;
                    break;
                case GameState.Fight:
                    CursorManager.SetCursor(CursorManager.CursorEvent.Invisible);
                    Time.timeScale = 1;
                    fightTimer.OnFight();
                    if (pauseMenu)
                        pauseMenu.SetActive(false);
                    break;
                case GameState.Start:
                    PlayerAnimationState.isDead = false;
                    pauseMenu.SetActive(false);
                    CursorManager.SetCursor(CursorManager.CursorEvent.Visible);
                    SceneManager.LoadScene("MainMenu");
                    fightTimer.Reset();
                    currentEnemyIdx = 0;
                    break;
                case GameState.CutScene:
                    fightTimer.maxTimer += fightTimer.fightTimer;
                    fightTimer.fightTimer = fightTimer.fightTimerRef;
                    SetState(GameState.Fight);
                    SpawnDragonAccordingToGameMode();
                    break;
                case GameState.Defeat:
                    pauseMenu.SetActive(false);
                    playerDefeated.SetActive(true);
                    scoreText.text = ReferenceTable.Player.GetComponent<Controller>().GetScore();
                    StartCoroutine(WaitForInput());
                    ResetPlayerProperties();
                    break;
            }
        }

        private void SpawnDragonAccordingToGameMode() {
            switch (_gameMode) {
                case GameMode.Solo:
                    StartCoroutine(SpawnSoloDragon(currentEnemyIdx));
                    break;
                case GameMode.Random:
                    StartCoroutine(SpawnSoloDragon(Random.Range(1, 5)));
                    break;
                case GameMode.Tutorial:
                    StartCoroutine(SpawnNewDragon());
                    break;
            }
        }

        private IEnumerator SpawnSoloDragon(int range) {
            Destroy(ReferenceTable.CurrentEnemy);
            while (UnityEngine.SceneManagement.SceneManager.GetActiveScene() != SceneManager.CurrentScene) {
                yield return new WaitForSeconds(.4f);
            }

            ReferenceTable.DragonSpawner.SpawnDragon(range);
            yield return new WaitForFixedUpdate();
        }

        private void ResetPlayerProperties() {
            ReferenceTable.Player.GetComponent<Health.Health>().ResetHealth();
            ReferenceTable.Player.GetComponent<Controller>().ResetScore();
        }

        private IEnumerator WaitForInput() {
            yield return new WaitUntil(() =>
                Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame);

            playerDefeated.SetActive(false);
            tutorialWinScreen.SetActive(false);
            SetState(GameState.Start);
        }

        private void FixedUpdate() {
            if (!ReferenceTable.CurrentEnemy) return;
            if (!ReferenceTable.CurrentEnemy.gameObject.activeSelf)
                SetState(GameState.CutScene);
        }

        private IEnumerator SpawnNewDragon() {
            Destroy(ReferenceTable.CurrentEnemy);
            while (UnityEngine.SceneManagement.SceneManager.GetActiveScene() != SceneManager.CurrentScene) {
                yield return new WaitForSeconds(.4f);
            }

            ReferenceTable.DragonSpawner.SpawnDragon(++currentEnemyIdx);
            yield return new WaitForFixedUpdate();
        }

        public void EnableGameModeUI() {
            gameModeChoose.SetActive(true);
        }

        public void SetGameMode(GameMode setGameMode) {
            _gameMode = setGameMode;
            ReferenceTable.GameManager.SetState(GameState.CutScene);
        }

        public void UpdateScore() {
            if (ReferenceTable.Player)
                ReferenceTable.Player.GetComponent<Controller>().UpdateScore(currentEnemyIdx);
        }

        public void SetTutorialDone() {
            tutorialWinScreen.SetActive(true);
            fightTimer.OnPause();
            StartCoroutine(WaitForInput());
        }

        public void EnableOptionsMenu() {
            CursorManager.SetCursor(CursorManager.CursorEvent.Visible);
            optionsMenu.SetActive(true);
        }
        public void DisableOptionsMenu() {
            optionsMenu.SetActive(false);
        }
    }
}