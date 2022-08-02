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
            Win,
            Defeat,
            CutScene
        }

        [SerializeField] public GameObject pauseMenu;
        [SerializeField] public GameObject gameModeChoose;
        [SerializeField] public GameObject dragonDefeated;
        [SerializeField] public GameObject TutorialWinScreen;
        [SerializeField] public FightTimer FightTimer;
        [SerializeField] public GameObject playerDefeated;
        [SerializeField] public TextMeshProUGUI scoreText;

        public CineMachineSwitcher switcher;

        private static GameState currentState;
        private static GameMode gameMode;
        public int currentEnemyIdx;

        private void Awake() {
            currentState = GameState.Start;

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
                SetState(currentState == GameState.Fight ? GameState.Pause : GameState.Fight);
        }

        public void SetState(GameState newState) {
            currentState = newState;
            SwitchGameState();
        }

        private void SwitchGameState() {
            switch (currentState) {
                case GameState.Pause:
                    FightTimer.OnPause();
                    pauseMenu.SetActive(true);
                    CursorManager.SetCursor(CursorManager.CursorEvent.Visible);
                    Time.timeScale = 0;
                    break;
                case GameState.Fight:
                    CursorManager.SetCursor(CursorManager.CursorEvent.Invisible);
                    Time.timeScale = 1;
                    FightTimer.OnFight();
                    if (pauseMenu)
                        pauseMenu.SetActive(false);
                    break;
                case GameState.Start:
                    PlayerAnimationState.isDead = false;
                    pauseMenu.SetActive(false);
                    CursorManager.SetCursor(CursorManager.CursorEvent.Visible);
                    SceneManager.LoadScene("MainMenu");
                    FightTimer.Reset();
                    currentEnemyIdx = 0;
                    break;
                case GameState.CutScene:
                    FightTimer.maxTimer += FightTimer.fightTimer;
                    FightTimer.fightTimer = FightTimer.fightTimerRef;
                    SetState(GameState.Fight);
                    SpawnDragonAccordingToGameMode();
                    break;
                case GameState.Defeat:
                    playerDefeated.SetActive(true);
                    scoreText.text = ReferenceTable.Player.GetComponent<Controller>().GetScore();
                    StartCoroutine(WaitForInput());
                    ResetPlayerProperties();
                    break;
            }
        }

        private void SpawnDragonAccordingToGameMode() {
            switch (gameMode) {
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
            TutorialWinScreen.SetActive(false);
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
            gameMode = setGameMode;
            ReferenceTable.GameManager.SetState(GameState.CutScene);
        }

        public void UpdateScore() {
            if (ReferenceTable.Player)
                ReferenceTable.Player.GetComponent<Controller>().UpdateScore(currentEnemyIdx);
        }

        public void SetTutorialDone() {
            TutorialWinScreen.SetActive(true);
            StartCoroutine(WaitForInput());
        }
    }
}