using Enemy;
using UnityEngine;

namespace CustomUtils {
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

        public static void SetCurrentEnemy(GameObject enemy) {
            CurrentEnemy = enemy;
            LookAtEnemy = enemy.transform;
            GameManager.switcher.enemyTarget.LookAt = LookAtEnemy;
        }

        private void FixedUpdate() {
            if(CurrentEnemy)
                LookAtEnemy = CurrentEnemy.transform;
            Player = GameObject.FindWithTag("Player");
            GameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }
    }
}