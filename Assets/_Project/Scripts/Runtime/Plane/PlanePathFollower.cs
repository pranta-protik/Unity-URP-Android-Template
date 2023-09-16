using KBCore.Refs;
using Project.Persistent.SaveSystem;
using UnityEngine;
using UnityEngine.Splines;
using Sirenix.OdinInspector;
using Project.Managers;
using Toolbox.Utilities;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Project
{
	public class PlanePathFollower : ValidatedMonoBehaviour, IDataPersistence
	{
		private enum LoopMode
		{
			Once,
			Continuous
		}

		[Header("References")]
		[SerializeField, Anywhere] private SplineContainer _spline;

		[Header("Follow Settings")]
		[EnableIf("@_spline != null")][EnumToggleButtons][SerializeField] private LoopMode _loopMode = LoopMode.Once;
		[ShowIf("@_spline != null")][SerializeField] private float _speed = 15f;

		private float _distanceTravelled;
		private float _splineLength;

		private void Start()
		{
			_splineLength = _spline.CalculateLength();
			transform.SetPositionAndRotation(_spline.EvaluatePosition(_distanceTravelled), Quaternion.LookRotation(_spline.EvaluateTangent(_distanceTravelled)));
		}

		private void Update()
		{
			if (GameManager.Instance.CurrentGameState == GameManager.GameState.Running)
			{
				HandlePathFollow();
			}
		}

		private void HandlePathFollow()
		{
			switch (_loopMode)
			{
				case LoopMode.Once:
					if (_distanceTravelled >= 1f)
					{
						return;
					}
					break;

				case LoopMode.Continuous:
					if (_distanceTravelled >= 1f)
					{
						_distanceTravelled = 0f;
					}
					break;
			}

			_distanceTravelled += _speed * Time.deltaTime / _splineLength;

			var currentPosition = _spline.EvaluatePosition(_distanceTravelled);
			transform.position = currentPosition;

			var currentTangent = _spline.EvaluateTangent(_distanceTravelled);
			transform.rotation = Quaternion.LookRotation(currentTangent);
		}

		public void ResetDistanceTravelled() => _distanceTravelled = 0f;

		public void LoadData(GameData gameData)
		{
			if (gameData.planePositionDictionary.TryGetValue(SceneUtils.GetActiveSceneIndex(), out var planePosition))
			{
				_distanceTravelled = planePosition;
			}
		}

		public void SaveData(GameData gameData)
		{
			if (gameData.planePositionDictionary.ContainsKey(SceneUtils.GetActiveSceneIndex()))
			{
				gameData.planePositionDictionary.Remove(SceneUtils.GetActiveSceneIndex());
			}

			if (GameManager.Instance.IsGameOver()) return;

			gameData.planePositionDictionary.Add(SceneUtils.GetActiveSceneIndex(), _distanceTravelled);
		}

		[Button, PropertySpace]
		private void MovePlaneToStartPosition()
		{
			transform.SetPositionAndRotation(_spline.EvaluatePosition(0f), Quaternion.LookRotation(_spline.EvaluateTangent(0f)));
			EditorUtility.SetDirty(gameObject);
		}
	}
}