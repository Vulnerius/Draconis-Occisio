using Enemy;
using UnityEngine;

namespace CustomUtils {
    /// <summary>
    /// static class holding references for Player, CurrentEnemy, GameManager and DragonSpawnManager
    /// </summary>
    public class ReferenceTable : MonoBehaviour {
        [field: SerializeField] private  DragonSpawnManager dragonSpawner;

        public static DragonSpawnManager DragonSpawner;
        public static GameManager GameManager;
        public static GameObject Player;
        public static GameObject CurrentEnemy;

        public static Transform LookAtEnemy;

        private void Awake() {
            DragonSpawner = dragonSpawner;
        }

        /// <summary>
        /// given a GameObject updating the references CurrentEnemy and LookAtEnemy
        /// </summary>
        /// <param name="enemy"></param>
        public static void SetCurrentEnemy(GameObject enemy) {
            CurrentEnemy = enemy;
            LookAtEnemy = enemy.transform;
            GameManager.switcher.enemyTarget.LookAt = LookAtEnemy;
        }

        /// <summary>
        /// updating the references Player and GameManager
        /// if active CurrentEnemy updating the LookAtEnemy reference
        /// </summary>
        private void FixedUpdate() {
            if(CurrentEnemy)
                LookAtEnemy = CurrentEnemy.transform;
            Player = GameObject.FindWithTag("Player");
            GameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }
    }
}