using UnityEngine;
using UnityEngine.SceneManagement;

namespace Toolbox.Utilities
{
    public static class SceneUtils
    {
        public static AsyncOperation LoadSpecificScene(int sceneIndex) => SceneManager.LoadSceneAsync(sceneIndex);
        public static AsyncOperation LoadSpecificScene(int sceneIndex, LoadSceneMode loadSceneMode) => SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);
        public static AsyncOperation ReloadScene() => SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        public static int GetActiveSceneIndex() => SceneManager.GetActiveScene().buildIndex;
    }
}
