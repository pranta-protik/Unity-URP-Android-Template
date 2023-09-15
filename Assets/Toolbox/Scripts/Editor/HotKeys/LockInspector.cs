using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Toolbox.EditorScripts.HotKeys
{
	public static class LockInspector
	{
		[MenuItem("Edit/Toggle Inspector Lock %l")]
		private static void Lock()
		{
			ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;

			foreach (var activeEditor in ActiveEditorTracker.sharedTracker.activeEditors)
			{
				if (activeEditor.target is not Transform) continue;

				var transform = (Transform)activeEditor.target;

				var propInfo = transform.GetType().GetProperty("constrainProportionsScale", BindingFlags.NonPublic | BindingFlags.Instance);

				if (propInfo == null) continue;

				var value = (bool)propInfo.GetValue(transform, null);

				propInfo.SetValue(transform, !value, null);
			}

			ActiveEditorTracker.sharedTracker.ForceRebuild();
		}

		[MenuItem("Edit/Toggle Inspector Lock %l", true)]
		private static bool Valid()
		{
			return ActiveEditorTracker.sharedTracker.activeEditors.Length != 0;
		}
	}
}