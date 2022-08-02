using System.Collections;
using Cinemachine;
using CustomUtils;
using UnityEngine;

namespace Player {
    public class CineMachineSwitcher : MonoBehaviour {
        [SerializeField] private CinemachineFreeLook playerTarget;
        [SerializeField] public CinemachineVirtualCamera enemyTarget;

        private void Awake() {
            ReferenceTable.GameManager.switcher = this;
            if(!ReferenceTable.CurrentEnemy) StartCoroutine(waitLittle());
            enemyTarget.LookAt = ReferenceTable.LookAtEnemy;
        }

        private IEnumerator waitLittle() {
            yield return new WaitForSeconds(.3f);
        }

        public void SwitchLock() {
            if (playerTarget.isActiveAndEnabled) {
                playerTarget.gameObject.SetActive(false);
                enemyTarget.gameObject.SetActive(true);
            }
            else {
                playerTarget.gameObject.SetActive(true);
                enemyTarget.gameObject.SetActive(false);
            }
        }
    }
}