using System.Collections;
using CustomUtils;
using UnityEngine;

namespace UserInterface {
    /// <summary>
    /// Behaviour for GameModeChooseMenu
    /// </summary>
    public class GameModeChooseMenu : MonoBehaviour {
        [SerializeField] private GameObject souleaterImage;
        [SerializeField] private GameObject nightmareImage;
        [SerializeField] private GameObject usurperImage;
        [SerializeField] private GameObject terrorBringerImage;

        /// <summary>
        /// Disabling the Canvas
        /// Loading PlayScene
        /// </summary>
        private void StandardBehaviour() {
            ReferenceTable.GameManager.gameModeChoose.SetActive(false);
            SceneManager.LoadScene("PlayScene");
        }

        /// <summary>
        /// Hover-Enter Event for SoulEater Button
        /// Enabling dragonImage
        /// </summary>
        public void OnHoverDragon1Enter() {
            souleaterImage.SetActive(true);
        }
        
        /// <summary>
        /// Hover-Exit Event for SoulEater Button
        /// Disabling dragonImage
        /// </summary>
        public void OnHoverDragon1Exit() {
            souleaterImage.SetActive(false);
        }
        
        /// <summary>
        /// Hover-Enter Event for SoulEater Button
        /// Enabling dragonImage
        /// </summary>
        public void OnHoverDragon2Enter() {
            nightmareImage.SetActive(true);
        }
        
        /// <summary>
        /// Hover-Exit Event for SoulEater Button
        /// Disabling dragonImage
        /// </summary>
        public void OnHoverDragon2Exit() {
            nightmareImage.SetActive(false);
        }
                
        /// <summary>
        /// Hover-Exit Event for SoulEater Button
        /// Disabling dragonImage
        /// </summary>
        public void OnHoverDragon3Enter() {
            usurperImage.SetActive(true);
        }
        
        /// <summary>
        /// Hover-Exit Event for SoulEater Button
        /// Disabling dragonImage
        /// </summary>
        public void OnHoverDragon3Exit() {
            usurperImage.SetActive(false);
        }
                
        /// <summary>
        /// Hover-Exit Event for SoulEater Button
        /// Disabling dragonImage
        /// </summary>
        public void OnHoverDragon4Enter() {
            terrorBringerImage.SetActive(true);
        }
        
        /// <summary>
        /// Hover-Exit Event for SoulEater Button
        /// Disabling dragonImage
        /// </summary>
        public void OnHoverDragon4Exit() {
            terrorBringerImage.SetActive(false);
        }

        /// <summary>
        /// Enabling Cycling Dragon Images
        /// </summary>
        public void OnHoverRandom() {
            StartCoroutine(ImageRandom());
        }
        
        /// <summary>
        /// Disabling Cycling Dragon Images
        /// </summary>
        public void OnHoverRandomExit() {
            StopCoroutine(ImageRandom());
        }

        /// <summary>
        /// Cycling one-time Dragon Images 1,3,2,4
        /// </summary>
        /// <returns>Waiting</returns>
        private IEnumerator ImageRandom() {
            souleaterImage.SetActive(true);
            yield return new WaitForSeconds(2f);
            souleaterImage.SetActive(false);
            usurperImage.SetActive(true);
            yield return new WaitForSeconds(2f);
            usurperImage.SetActive(false);
            nightmareImage.SetActive(true);
            yield return new WaitForSeconds(2f);
            nightmareImage.SetActive(false);
            terrorBringerImage.SetActive(true);
            yield return new WaitForSeconds(2f);
            terrorBringerImage.SetActive(false);
        }

        /// <summary>
        /// Button functionality for Random
        /// Setting GameMode to Random
        /// </summary>
        public void OnRandom() {
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Random);
        }
        
        /// <summary>
        /// Button functionality for SoulEater
        /// setting currentEnemyIdx to 1
        /// Setting GameMode to Solo
        /// </summary>
        public void OnSoloDragon1() {
            ReferenceTable.GameManager.currentEnemyIdx = 1;
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
        }
        
        /// <summary>
        /// Button functionality for Nightmare
        /// setting currentEnemyIdx to 2
        /// Setting GameMode to Solo
        /// </summary>
        public void OnSoloDragon2() {
            ReferenceTable.GameManager.currentEnemyIdx = 2;
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
        }
        
        /// <summary>
        /// Button functionality for Usurper
        /// setting currentEnemyIdx to 3
        /// Setting GameMode to Solo
        /// </summary>
        public void OnSoloDragon3() {
            ReferenceTable.GameManager.currentEnemyIdx = 3;
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
        }
        
        /// <summary>
        /// Button functionality for TerrorBringer
        /// setting currentEnemyIdx to 4
        /// Setting GameMode to Solo
        /// </summary>
        public void OnSoloDragon4() {
            ReferenceTable.GameManager.currentEnemyIdx = 4;
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
        }
    }
}