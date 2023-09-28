using System;
using System.Collections;
using Cinemachine;
using KBCore.Refs;
using Project.Managers;
using Project.Persistent.SaveSystem;
using Sirenix.OdinInspector;
using Toolbox.Utilities;
using UnityEngine;

namespace DemoScene
{
	public class ControlSwitcher : Singleton<ControlSwitcher>, IDataPersistence
	{
		private enum ControlType
		{
			Human,
			Plane
		}

		[EnumToggleButtons][SerializeField] private ControlType _activeControlType = ControlType.Human;
		[SerializeField, Anywhere] private GameObject _playerGO;
		[SerializeField, Anywhere] private GameObject _planeGO;
		[SerializeField, Anywhere] private CinemachineVirtualCamera _playerVCam;
		[SerializeField, Anywhere] private CinemachineVirtualCamera _planeVCam;
		[SerializeField, Anywhere] private CinemachineBrain _cinemachineBrain;

		private float _defaultBlendTime;

#if UNITY_EDITOR
		private void OnValidate() => this.ValidateRefs();
#endif

		private void Start()
		{
			_defaultBlendTime = _cinemachineBrain.m_DefaultBlend.m_Time;
			_cinemachineBrain.m_DefaultBlend.m_Time = 0f;

			switch (_activeControlType)
			{
				case ControlType.Human:
					SwitchToPlayerControl();
					break;

				case ControlType.Plane:
					SwitchToPlaneControl();
					break;
			}
			StartCoroutine(ResetCameraBlendTimeRoutine());
		}

		private IEnumerator ResetCameraBlendTimeRoutine()
		{
			yield return new WaitForEndOfFrame();
			_cinemachineBrain.m_DefaultBlend.m_Time = _defaultBlendTime;
		}

		public void SwitchToPlayerControl()
		{
			_activeControlType = ControlType.Human;

			_planeGO.SetActive(false);
			_playerGO.SetActive(true);

			_planeVCam.Priority = 5;
			_playerVCam.Priority = 10;
		}

		public void SwitchToPlaneControl()
		{
			_activeControlType = ControlType.Plane;

			_playerGO.SetActive(false);
			_planeGO.SetActive(true);

			_playerVCam.Priority = 5;
			_planeVCam.Priority = 10;
		}

		public void LoadData(GameData gameData)
		{
			if (gameData.activeControlTypeDictionary.TryGetValue(SceneUtils.GetActiveSceneIndex(), out var activeControlType))
			{
				if (Enum.TryParse(activeControlType, out ControlType controlType))
				{
					_activeControlType = controlType;
				}
			}
		}

		public void SaveData(GameData gameData)
		{
			if (gameData.activeControlTypeDictionary.ContainsKey(SceneUtils.GetActiveSceneIndex()))
			{
				gameData.activeControlTypeDictionary.Remove(SceneUtils.GetActiveSceneIndex());
			}

			if (GameManager.Instance.IsGameOver()) return;

			gameData.activeControlTypeDictionary.Add(SceneUtils.GetActiveSceneIndex(), _activeControlType.ToString());
		}
	}
}