using Project.Utilities;
using Toolbox.Utilities;
using UnityEngine;

namespace Project.Persistent
{
    public class Initializer : MonoBehaviour
    {
        private void Awake()
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            if (PlayerPrefs.GetInt(ConstUtils.PREF_FIRST_TIME_PLAYING, 1) == 1)
            {
                PlayerPrefs.SetInt(ConstUtils.PREF_FIRST_TIME_PLAYING, 0);
                SceneUtils.LoadSpecificScene((int)SceneIndex.SPLASH);
            }
            else
            {
                SceneUtils.LoadSpecificScene(PlayerPrefs.GetInt(ConstUtils.PREF_LAST_PLAYED_SCENE_INDEX, (int)SceneIndex.GAME));
            }
        }
    }
}
