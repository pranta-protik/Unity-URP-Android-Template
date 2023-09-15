using UnityEngine;

namespace Toolbox.Utilities
{
	public static class DebugUtils
	{
		public static void Log(string message)
		{
#if UNITY_EDITOR
			Debug.Log(message);
#endif
		}

		public static void LogWarning(string message)
		{
#if UNITY_EDITOR
			Debug.LogWarning(message);
#endif
		}

		public static void LogError(string message)
		{
#if UNITY_EDITOR
			Debug.LogError(message);
#endif
		}
	}
}