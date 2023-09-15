using Project.Utilities;
using Toolbox.Utilities;
using UnityEngine;

namespace Project.Persistent
{
	public class LevelLoader : MonoBehaviour
	{
		public static LevelLoader Instance { get; private set; }

		[SerializeField, Min(0)] private int _totalSceneCount;
		[SerializeField, Min(0)] private int _firstLevelSceneIndex = (int)SceneIndex.GAME;

		private void Awake()
		{
			if (Instance != null)
			{
				DebugUtils.LogWarning($"There's more than one instance of LevelLoader in the scene! Destroying the new instance!");
				Destroy(gameObject);
				return;
			}

			Instance = this;

			DontDestroyOnLoad(gameObject);
		}

		public int GetNextSceneIndex()
		{
			var nextSceneIndex = PlayerPrefs.GetInt(ConstUtils.PREF_LAST_PLAYED_SCENE_INDEX, (int)SceneIndex.GAME);
			var inGameLevelCount = PlayerPrefs.GetInt(ConstUtils.PREF_IN_GAME_LEVEL_COUNT, 1);

			if (nextSceneIndex >= _totalSceneCount - 1)
			{
				nextSceneIndex = _firstLevelSceneIndex;
			}
			else
			{
				nextSceneIndex++;
			}

			PlayerPrefs.SetInt(ConstUtils.PREF_LAST_PLAYED_SCENE_INDEX, nextSceneIndex);
			PlayerPrefs.SetInt(ConstUtils.PREF_IN_GAME_LEVEL_COUNT, inGameLevelCount + 1);

			return nextSceneIndex;
		}
	}
}