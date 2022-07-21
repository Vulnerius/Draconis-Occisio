using Enemy;
using UnityEngine;

namespace DefaultNamespace {
    public class ReferenceTable : MonoBehaviour {
        [SerializeField] private GameObject player;
        [SerializeField] private GameManager gameManager;
        [field: SerializeField] private  DragonSpawnManager dragonSpawner;

        public static DragonSpawnManager DragonSpawner;
        public static GameManager GameManager;
        public static GameObject Player;
        public static GameObject CurrentEnemy;

        public static Transform LookAtEnemy;

        private void Start() {
            GameManager = gameManager;
            DragonSpawner = dragonSpawner;
            Player = player;
        }

        public static void SetCurrentEnemy(GameObject enemy) {
            CurrentEnemy = enemy;
            LookAtEnemy = enemy.transform;
            GameManager.switcher.enemyTarget.LookAt = LookAtEnemy;
        }

        private void FixedUpdate() {
            if(CurrentEnemy)
                LookAtEnemy = CurrentEnemy.transform;
        }
    }
}