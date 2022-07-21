using UnityEngine.SceneManagement;

namespace DefaultNamespace {
    public static class SceneManager {
        public static Scene currentScene;
        public static void LoadScene(string sceneName) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            currentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        }
        
    }
}