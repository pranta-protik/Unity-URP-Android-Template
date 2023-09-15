using UnityEngine;

namespace Toolbox.Utilities
{
	public class Singleton<T> : MonoBehaviour where T : Component
	{
		[SerializeField] protected bool _dontDestroyOnLoad;

		public static T Instance { get; private set; }

		private void Awake()
		{
			if (Instance != null)
			{
				DebugUtils.LogWarning($"There's more than one instance of {typeof(T).Name} in the scene. Destroying the new instance.");
				Destroy(gameObject);
				return;
			}

			Instance = this as T;

			if (_dontDestroyOnLoad)
			{
				transform.parent = null;
				DontDestroyOnLoad(gameObject);
			}

			OnAwake();
		}

		protected virtual void OnAwake() { }
	}
}