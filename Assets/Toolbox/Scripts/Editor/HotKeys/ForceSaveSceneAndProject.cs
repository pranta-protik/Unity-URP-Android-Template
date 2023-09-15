using Toolbox.Utilities;
using UnityEditor;

namespace Toolbox.EditorScripts.HotKeys
{
	public static class ForceSaveSceneAndProject
	{
		[MenuItem("File/Save Scene And Project %#&s")]
		public static void FunctionForceSaveSceneAndProject()
		{
			EditorApplication.ExecuteMenuItem("File/Save");
			EditorApplication.ExecuteMenuItem("File/Save Project");
			DebugUtils.Log("Save scene and project.");
		}
	}
}