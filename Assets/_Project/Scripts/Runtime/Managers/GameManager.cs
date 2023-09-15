using Project.Persistent;
using Project.Persistent.SaveSystem;
using Sirenix.OdinInspector;
using Toolbox.Utilities;
using UnityEngine.Events;

namespace Project.Managers
{
	public class GameManager : Singleton<GameManager>
	{
		public enum GameState
		{
			Waiting,
			Running,
			Completed,
			Failed
		}

		public event UnityAction OnLevelStarted;
		public event UnityAction<int> OnLevelCompleted;
		public event UnityAction OnLevelFailed;

		[ShowInInspector, DisplayAsString] public GameState CurrentGameState { get; private set; }

		protected override void OnAwake()
		{
			base.OnAwake();
			CurrentGameState = GameState.Waiting;
		}

		public void LevelStarted()
		{
			CurrentGameState = GameState.Running;
			OnLevelStarted?.Invoke();
		}

		public void LevelCompleted()
		{
			CurrentGameState = GameState.Completed;
			OnLevelCompleted?.Invoke(LevelLoader.Instance.GetNextSceneIndex());
			DataPersistenceManager.Instance.SaveGame();
		}

		public void LevelFailed()
		{
			CurrentGameState = GameState.Failed;
			OnLevelFailed?.Invoke();
			DataPersistenceManager.Instance.SaveGame();
		}

		public bool IsGameOver() => CurrentGameState == GameState.Completed || CurrentGameState == GameState.Failed;
	}
}