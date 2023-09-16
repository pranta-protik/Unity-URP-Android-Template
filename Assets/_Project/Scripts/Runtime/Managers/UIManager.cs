using System;
using System.Collections;
using KBCore.Refs;
using Project.Utilities;
using Toolbox.ES;
using Toolbox.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Managers
{
	public class UIManager : Singleton<UIManager>
	{
		[SerializeField, Anywhere] private EventChannelSO levelStartChannel;
		[SerializeField, Anywhere] private IntEventChannelSO levelCompleteChannel;
		[SerializeField, Anywhere] private EventChannelSO levelFailChannel;
		[SerializeField, Range(0f, 15f)] private float _uiScreenDelay = 2f;

#if UNITY_EDITOR
		private void OnValidate() => this.ValidateRefs();
#endif

		private void Start()
		{
			GameManager.Instance.OnLevelStarted += GameManager_OnLevelStarted;
			GameManager.Instance.OnLevelCompleted += GameManager_OnLevelCompleted;
			GameManager.Instance.OnLevelFailed += GameManager_OnLevelFailed;

			if (!SceneManager.GetSceneByBuildIndex((int)SceneIndex.UI).isLoaded)
			{
				SceneUtils.LoadSpecificScene((int)SceneIndex.UI, LoadSceneMode.Additive);
			}
		}

		private void OnDestroy()
		{
			GameManager.Instance.OnLevelStarted -= GameManager_OnLevelStarted;
			GameManager.Instance.OnLevelCompleted -= GameManager_OnLevelCompleted;
			GameManager.Instance.OnLevelFailed -= GameManager_OnLevelFailed;
		}

		private void GameManager_OnLevelStarted()
		{
			levelStartChannel.Invoke(new Empty());
		}

		private void GameManager_OnLevelCompleted(int sceneIndex)
		{
			StartCoroutine(DelayActionRoutine(() => levelCompleteChannel.Invoke(sceneIndex)));
		}

		private void GameManager_OnLevelFailed()
		{
			StartCoroutine(DelayActionRoutine(() => levelFailChannel.Invoke(new Empty())));
		}

		private IEnumerator DelayActionRoutine(Action action)
		{
			yield return new WaitForSeconds(_uiScreenDelay);
			action.Invoke();
		}
	}
}