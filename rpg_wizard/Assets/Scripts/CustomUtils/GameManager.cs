using System.Collections;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UserInterface;
using Random = UnityEngine.Random;

namespace CustomUtils {
    /// <summary>
    /// base class for GameLoop behaviours 
    /// </summary>
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

        [SerializeField] private ScoresTexts scoreTexts;
        [SerializeField] public GameObject optionsMenu;
        [SerializeField] public GameObject pauseMenu;
        [SerializeField] public GameObject gameModeChoose;
        [SerializeField] public GameObject tutorialWinScreen;
        [SerializeField] public FightTimer fightTimer;
        [SerializeField] public GameObject playerDefeated;
        [SerializeField] public TextMeshProUGUI scoreText;

        public CineMachineSwitcher switcher;

        private SaveAndRestoreScoreList scores;

        private static GameState _currentState;
        private static GameMode _gameMode;
        public int currentEnemyIdx;

        /// <summary>
        /// initializing the SaveAndRestoreScoreList
        /// setting GameState to Start
        /// adding this GameObject to DontDestroyOnLoad
        /// </summary>
        private void Awake() {
            scores = new();
            UpdateMainMenuTexts();
            _currentState = GameState.Start;
            if (ReferenceTable.GameManager == null)
                ReferenceTable.GameManager = this;
            else if (ReferenceTable.GameManager != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private void Update() {
            if (Keyboard.current.fKey.wasPressedThisFrame)
                switcher.SwitchLock();

            if (Keyboard.current.escapeKey.wasPressedThisFrame && _currentState != GameState.Start)
                SetState(_currentState == GameState.Fight ? GameState.Pause : GameState.Fight);
        }


        /// <summary>
        /// setting the _currentState to newState
        /// switching States accordingly
        /// </summary>
        /// <param name="newState">new State to be set</param>
        public void SetState(GameState newState) {
            _currentState = newState;
            SwitchGameState();
        }

        /// <summary>
        /// taking _currentState resolving in different actions:
        ///  Pause: stopping fightTimer, enabling PauseMenu, setting Cursor visible, Timescale 0
        ///  Fight: setting Cursor invisible, Timescale 1, enabling FightTimer, disabling options- and pauseMenu
        ///  Start: setting Scores active, setting isDead PlayerAnimationState to false,
        ///         disabling options- and pauseMenu, setting Cursor visible, loading MainMenu scene, resetting fightTimer
        ///  CutScene: disabling Scores, adding remaining Time of fight time to maxTime, resetting fightTime to reference value,
        ///             setting GameState to fight, SpawningDragonAccording to GameMode
        ///  Defeat: disabling options- and pauseMenu, setting the score, updating scoreTexts, resetting Player Properties
        /// </summary>
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
                    if (_gameMode != GameMode.Tutorial)
                        fightTimer.OnFight();
                    pauseMenu.SetActive(false);
                    optionsMenu.SetActive(false);
                    break;

                case GameState.Start:
                    scoreTexts.gameObject.SetActive(true);
                    PlayerAnimationState.isDead = false;
                    pauseMenu.SetActive(false);
                    optionsMenu.SetActive(false);
                    CursorManager.SetCursor(CursorManager.CursorEvent.Visible);
                    SceneManager.LoadScene("MainMenu");
                    fightTimer.Reset();
                    currentEnemyIdx = 0;
                    break;

                case GameState.CutScene:
                    scoreTexts.gameObject.SetActive(false);
                    fightTimer.maxTimer += fightTimer.fightTimer;
                    fightTimer.fightTimer = fightTimer.fightTimerRef;
                    SetState(GameState.Fight);
                    SpawnDragonAccordingToGameMode();
                    break;

                case GameState.Defeat:
                    if (_gameMode == GameMode.Tutorial) {
                        SetState(GameState.Start);
                        break;
                    }

                    pauseMenu.SetActive(false);
                    optionsMenu.SetActive(false);
                    playerDefeated.SetActive(true);
                    scoreText.text = ReferenceTable.Player.GetComponent<Controller>().GetScore();
                    if (_gameMode != GameMode.Tutorial)
                        scores.SaveScore(ReferenceTable.Player.GetComponent<Controller>().GetScoreSystem());
                    UpdateMainMenuTexts();
                    StartCoroutine(WaitForInput());
                    ResetPlayerProperties();
                    break;
            }
        }

        private void UpdateMainMenuTexts() {
            scoreTexts.bestRun.text = scores.ScoreList[0].GetScore().ToString();
            scoreTexts.secondbestRun.text = scores.ScoreList[1].GetScore().ToString();
            scoreTexts.thirdbestRun.text = scores.ScoreList[2].GetScore().ToString();
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

        /// <summary>
        /// Destroying dead Dragon GameObject
        /// waiting for active Scene to be rendered
        /// spawning new Dragon
        /// </summary>
        /// <param name="range">index number of dragon to be spawned</param>
        /// <returns>Waiting</returns>
        private IEnumerator SpawnSoloDragon(int range) {
            Destroy(ReferenceTable.CurrentEnemy);
            while (UnityEngine.SceneManagement.SceneManager.GetActiveScene() != SceneManager.CurrentScene) {
                yield return new WaitForSeconds(.4f);
            }

            ReferenceTable.DragonSpawner.SpawnDragon(range);
            yield return new WaitForFixedUpdate();
        }

        /// <summary>
        /// setting CurrentHealth to MaxHealth
        /// setting score to 0
        /// </summary>
        private void ResetPlayerProperties() {
            ReferenceTable.Player.GetComponent<Health.Health>().ResetHealth();
            ReferenceTable.Player.GetComponent<Controller>().ResetScore();
        }

        /// <summary>
        /// waiting for the Player to press either enter or space
        /// disabling PlayerDefeatScreen and tutorialWinScreen after input
        /// setting GameState to Start
        /// </summary>
        /// <returns>Waiting for Keyboard-Input</returns>
        private IEnumerator WaitForInput() {
            yield return new WaitUntil(() =>
                Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame);

            playerDefeated.SetActive(false);
            tutorialWinScreen.SetActive(false);
            SetState(GameState.Start);
        }

        /// <summary>
        /// setting GameState to Cutscene after dragon died
        /// </summary>
        private void FixedUpdate() {
            if (!ReferenceTable.CurrentEnemy) return;
            if (!ReferenceTable.CurrentEnemy.gameObject.activeSelf)
                SetState(GameState.CutScene);
        }

        /// <summary>
        /// is called if GameMode is Tutorial
        /// Destroying dead dragon gameObject
        /// waiting for SceneChange
        /// Spawning next dragon in array of DragonSpawnManager
        /// </summary>
        /// <returns>Waiting</returns>
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

        /// <summary>
        /// setting the _gameMode reference
        /// switching the State to CutScene
        /// </summary>
        /// <param name="setGameMode">the chosen GameMode</param>
        public void SetGameMode(GameMode setGameMode) {
            _gameMode = setGameMode;
            ReferenceTable.GameManager.SetState(GameState.CutScene);
        }
        
        public void UpdateScore() {
            if (ReferenceTable.Player)
                ReferenceTable.Player.GetComponent<Controller>().UpdateScore(currentEnemyIdx);
        }

        /// <summary>
        /// enabling the tutorialDone Canvas
        /// pausing the fightTimer
        /// </summary>
        public void SetTutorialDone() {
            tutorialWinScreen.SetActive(true);
            fightTimer.OnPause();
            StartCoroutine(WaitForInput());
        }

        /// <summary>
        /// setting the cursor visible
        /// enabling the OptionsMenu
        /// </summary>
        public void EnableOptionsMenu() {
            CursorManager.SetCursor(CursorManager.CursorEvent.Visible);
            optionsMenu.SetActive(true);
        }

        public void DisableOptionsMenu() {
            optionsMenu.SetActive(false);
        }

        public void DisableGameModeChoose() {
            gameModeChoose.SetActive(false);
        }
    }
}