using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    public class DragonSpawnManager : MonoBehaviour {
        [SerializeField] private List<Enemy> dragonList;

        private int currentIdx;

        private void Awake() {
            SpawnDragon();
        }

        public void SpawnDragon() {
            Instantiate(dragonList[currentIdx], transform.position, Quaternion.identity);
            currentIdx++;

            CheckCurrentIdx();
        }

        private void CheckCurrentIdx() {
            if (currentIdx >= dragonList.Count)
                currentIdx = 0;
        }
    }
}