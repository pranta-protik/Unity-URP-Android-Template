using Toolbox.Utilities;
using UnityEditor;

namespace Toolbox.EditorScripts.HotKeys
{
	public static class CloseWindowTab
	{
		[MenuItem("File/Close Window Tab %w")]
		private static void CloseTab()
		{
			var focusedWindow = EditorWindow.focusedWindow;

			if (focusedWindow != null)
			{
				CloseTab(focusedWindow);
			}
			else
			{
				DebugUtils.LogWarning("No focused window found.");
			}
		}

		private static void CloseTab(EditorWindow editorWindow)
		{
			editorWindow.Close();
		}
	}
}