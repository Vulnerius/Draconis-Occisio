using CustomUtils;
using UnityEngine;

namespace UserInterface {
    public class PauseMenu : MonoBehaviour {
        [SerializeField] private GameManager manager;
        
        public void OnResume() {
            manager.SetState(GameManager.GameState.Fight);
        }

        public void OnExitPause() {
            manager.SetState(GameManager.GameState.Defeat);
        }
    }
}