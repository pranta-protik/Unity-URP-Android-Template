using System.IO;
using UnityEditor;
using UnityEngine;

namespace Toolbox.EditorScripts.HotKeys
{
	public static class OpenSaveDirectory
	{
		[MenuItem("Edit/Open Save Directory")]
		private static void OpenDirectory()
		{
			EditorUtility.RevealInFinder(Path.Combine(Application.persistentDataPath, "data.sav"));
		}
	}
}