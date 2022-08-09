using System;
using CustomUtils;
using UnityEngine;

namespace Enemy {
    /// <summary>
    /// ScriptableObject for modifying dragons that will be spawned
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "DragonSpawner", menuName = "ScriptableObjects/DragonSpawner")]
    public class DragonSpawnManager : ScriptableObject {
        [SerializeField] private GameObject usurper;
        [SerializeField] private GameObject soulEater;
        [SerializeField] private GameObject nightmare;
        [SerializeField] private GameObject terrorBringer;
        
        /// <summary>
        /// setting currentEnemyIdx reference in GameManager
        /// spawning GameObject prefab according to the number
        /// </summary>
        /// <param name="dragonNumber"></param>
        public void SpawnDragon(int dragonNumber) {
            if (dragonNumber > 4) ReferenceTable.GameManager.SetTutorialDone();
            ReferenceTable.GameManager.currentEnemyIdx = dragonNumber;
            switch (dragonNumber) {
                case 1:
                    SpawnSoulEater();
                    break;
                case 2:
                    SpawnNightmare();
                    break;
                case 4:
                    SpawnTerrorBringer();
                    break;
                case 3:
                    SpawnUsurper();
                    break;
            }
        }

        private void SpawnUsurper() {
            ReferenceTable.SetCurrentEnemy(
                Instantiate(usurper, usurper.transform.position, Quaternion.identity)
            );
        }

        private void SpawnTerrorBringer() {
            ReferenceTable.SetCurrentEnemy(
                Instantiate(terrorBringer, terrorBringer.transform.position, Quaternion.identity)
            );
        }

        private void SpawnNightmare() {
            ReferenceTable.SetCurrentEnemy(
                Instantiate(nightmare, nightmare.transform.position, Quaternion.identity)
            );
        }

        private void SpawnSoulEater() {
            ReferenceTable.SetCurrentEnemy(
                Instantiate(soulEater, soulEater.transform.position, Quaternion.identity)
            );
        }
    }
}