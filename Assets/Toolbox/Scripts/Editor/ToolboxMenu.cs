using UnityEditor;

namespace Toolbox.EditorScripts
{
    public static class ToolboxMenu
    {
        [MenuItem("Tools/Toolbox/Project Setup")]
        private static void ProjectSetup() => ProjectSetupWindow.InitWindow();

        [MenuItem("Tools/Toolbox/Scene Setup")]
        private static void SceneSetup() => SceneSetupWindow.InitWindow();

        [MenuItem("Tools/Toolbox/Android Setup")]
        private static void AndroidSetup() => AndroidSetupWindow.InitWindow();
    }
}