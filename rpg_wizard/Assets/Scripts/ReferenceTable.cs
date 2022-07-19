using System;
using UnityEngine;

namespace DefaultNamespace {
    public class ReferenceTable : MonoBehaviour {
        [SerializeField] private GameObject player;
        [field: SerializeField] public GameObject DragonSpawner;

        public static GameObject Player;
        public static GameObject CurrentEnemy;
        public static Transform LookAtPlayer;
        public static Transform LookAtEnemy;

        private void Start() {
            Player = player;
            LookAtPlayer = player.transform;
        }

        public void SetCurrentEnemy(GameObject enemy) {
            CurrentEnemy = enemy;
            LookAtEnemy = enemy.transform;
        }

        private void Update() {
            LookAtEnemy = CurrentEnemy.transform;
            LookAtPlayer = player.transform;
        }
    }
}