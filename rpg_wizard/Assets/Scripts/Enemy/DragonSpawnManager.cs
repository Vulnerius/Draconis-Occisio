using System;
using DefaultNamespace;
using UnityEngine;

namespace Enemy {
    public class DragonSpawnManager : MonoBehaviour {
        [SerializeField] private GameObject usurper;
        [SerializeField] private GameObject soulEater;
        [SerializeField] private GameObject nightmare;
        [SerializeField] private GameObject terrorBringer;
        
        public void SpawnDragon(int dragonNumber) {
            if(dragonNumber > 4) dragonNumber = 1;
            ReferenceTable.GameManager.currentEnemyIdx = dragonNumber;
            switch (dragonNumber) {
                case 1:
                    SpawnSoulEater();
                    break;
                case 2:
                    SpawnNightmare();
                    break;
                case 3:
                    SpawnTerrorBringer();
                    break;
                case 4:
                    SpawnUsurper();
                    break;
            }
        }

        private void SpawnUsurper() {
            ReferenceTable.SetCurrentEnemy(
                Instantiate(usurper, transform.position, Quaternion.identity)
            );
        }

        private void SpawnTerrorBringer() {
            ReferenceTable.SetCurrentEnemy(
                Instantiate(terrorBringer, transform.position, Quaternion.identity)
            );
        }

        private void SpawnNightmare() {
            ReferenceTable.SetCurrentEnemy(
                Instantiate(nightmare, transform.position, Quaternion.identity)
            );
        }

        private void SpawnSoulEater() {
            ReferenceTable.SetCurrentEnemy(
                Instantiate(soulEater, transform.position, Quaternion.identity)
            );
        }
    }
}