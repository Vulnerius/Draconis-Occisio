using CustomUtils;
using UnityEngine;

namespace UserInterface {
    public class PauseMenu : MonoBehaviour {
        [SerializeField] private GameManager manager;
        
        /// <summary>
        /// Setting GameState to Fight
        /// </summary>
        public void OnResume() {
            manager.SetState(GameManager.GameState.Fight);
        }

        /// <summary>
        /// Setting GameState to Defeat
        /// </summary>
        public void OnExitPause() {
            manager.SetState(GameManager.GameState.Defeat);
        }
    }
}