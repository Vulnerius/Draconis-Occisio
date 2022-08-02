using UnityEngine.SceneManagement;

namespace CustomUtils {
    public static class SceneManager {
        public static Scene CurrentScene;
        public static void LoadScene(string sceneName) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            CurrentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        }
        
    }
}