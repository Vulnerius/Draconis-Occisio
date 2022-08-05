using System.Collections;
using CustomUtils;
using UnityEngine;

namespace UserInterface {
    public class GameModeChooseMenu : MonoBehaviour {
        [SerializeField] private GameObject souleaterImage;
        [SerializeField] private GameObject nightmareImage;
        [SerializeField] private GameObject usurperImage;
        [SerializeField] private GameObject terrorBringerImage;
        private void StandardBehaviour() {
            ReferenceTable.GameManager.gameModeChoose.SetActive(false);
            SceneManager.LoadScene("PlayScene");
        }

        public void OnHoverDragon1Enter() {
            souleaterImage.SetActive(true);
        }
        public void OnHoverDragon1Exit() {
            souleaterImage.SetActive(false);
        }
        public void OnHoverDragon2Enter() {
            nightmareImage.SetActive(true);
        }
        public void OnHoverDragon2Exit() {
            nightmareImage.SetActive(false);
        }
        public void OnHoverDragon3Enter() {
            usurperImage.SetActive(true);
        }
        public void OnHoverDragon3Exit() {
            usurperImage.SetActive(false);
        }
        public void OnHoverDragon4Enter() {
            terrorBringerImage.SetActive(true);
        }
        public void OnHoverDragon4Exit() {
            terrorBringerImage.SetActive(false);
        }

        public void OnHoverRandom() {
            StartCoroutine(ImageRandom());
        }
        public void OnHoverRandomExit() {
            StopCoroutine(ImageRandom());
        }

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

        public void OnRandom() {
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Random);
        }
        
        public void OnSoloDragon1() {
            ReferenceTable.GameManager.currentEnemyIdx = 1;
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
        }
        
        public void OnSoloDragon2() {
            ReferenceTable.GameManager.currentEnemyIdx = 2;
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
        }
        
        public void OnSoloDragon3() {
            ReferenceTable.GameManager.currentEnemyIdx = 3;
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
        }
        
        public void OnSoloDragon4() {
            ReferenceTable.GameManager.currentEnemyIdx = 4;
            StandardBehaviour();
            ReferenceTable.GameManager.SetGameMode(GameManager.GameMode.Solo);
        }
    }
}