using UnityEditor;

namespace Toolbox.EditorScripts
{
    public static class EditorUtils
    {
        public static bool DisplayDialogBox(string title, string message) => EditorUtility.DisplayDialog(title, message, "OK");
        public static bool DisplayDialogBoxWithOptions(string title, string message) => EditorUtility.DisplayDialog(title, message, "Yes", "No");
    }
}
