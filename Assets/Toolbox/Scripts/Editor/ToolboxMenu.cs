using UnityEditor;

namespace Toolbox.EditorScripts
{
    public static class ToolboxMenu
    {
        [MenuItem("Tools/Toolbox/Project Setup", false, 1)]
        private static void ProjectSetup() => ProjectSetupWindow.InitWindow();

        [MenuItem("Tools/Toolbox/Scene Setup", false, 2)]
        private static void SceneSetup() => SceneSetupWindow.InitWindow();

        [MenuItem("Tools/Toolbox/Android Build Setup", false, 3)]
        private static void AndroidBuildSetup() => AndroidBuildSetupWindow.InitWindow();
    }
}