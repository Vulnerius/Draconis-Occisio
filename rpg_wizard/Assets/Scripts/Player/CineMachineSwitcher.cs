using System.Collections;
using Cinemachine;
using CustomUtils;
using UnityEngine;

namespace Player {
    /// <summary>
    /// behaviour for switching 3rd person to 1st person with camera locked to dragon
    /// </summary>
    public class CineMachineSwitcher : MonoBehaviour {
        [SerializeField] private CinemachineFreeLook playerTarget;
        [SerializeField] public CinemachineVirtualCamera enemyTarget;

        /// <summary>
        /// assigning this as GameManager switcher
        /// assigning VirtualCamera.LookAt (1st Person) 
        /// </summary>
        private void Awake() {
            ReferenceTable.GameManager.switcher = this;
            if(!ReferenceTable.CurrentEnemy) StartCoroutine(waitLittle());
            enemyTarget.LookAt = ReferenceTable.LookAtEnemy;
        }

        /// <summary>
        /// waiting for CurrentEnemy to be instantiated
        /// </summary>
        /// <returns></returns>
        private IEnumerator waitLittle() {
            yield return new WaitForSeconds(.3f);
        }

        /// <summary>
        /// enabling 1st Person disabling 3rd Person
        ///     or
        /// enabling 3rd Person disabling 1st Person
        /// </summary>
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