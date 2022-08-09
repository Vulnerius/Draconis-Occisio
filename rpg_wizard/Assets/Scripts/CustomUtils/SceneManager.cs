using UnityEngine.SceneManagement;

namespace CustomUtils {
    /// <summary>
    /// Scene Loading Manager
    /// </summary>
    public static class SceneManager {
        public static Scene CurrentScene;
        /// <summary>
        /// Loading the scene by string
        /// setting Reference
        /// </summary>
        /// <param name="sceneName">the scene to be loaded</param>
        public static void LoadScene(string sceneName) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            CurrentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        }
        
    }
}