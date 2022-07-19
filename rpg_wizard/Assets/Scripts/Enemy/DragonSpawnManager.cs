using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Enemy {
    public class DragonSpawnManager : MonoBehaviour {
        [SerializeField] private List<Enemy> dragonList;

        private int currentIdx;

        private void Awake() {
            SpawnDragon();
        }

        private void SpawnDragon() {
            GameObject.FindWithTag("GameManager").GetComponent<ReferenceTable>().SetCurrentEnemy(
                Instantiate(dragonList[currentIdx], transform.position, Quaternion.identity).gameObject
            );
            currentIdx++;
            CheckCurrentIdx();
        }

        private void CheckCurrentIdx() {
            if (currentIdx >= dragonList.Count)
                currentIdx = 0;
        }
    }
}