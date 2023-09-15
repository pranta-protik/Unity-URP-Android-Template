using Project.Persistent;
using UnityEditor;

namespace Project.EditorScripts.Persistent
{
    [CustomEditor(typeof(LevelLoader))]
    public class LevelLoaderEditor : Editor
    {
        private SerializedProperty _totalSceneCountProperty;
        private SerializedProperty _firstLevelSceneIndexProperty;

        private void OnEnable()
        {
            _totalSceneCountProperty = serializedObject.FindProperty("_totalSceneCount");
            _firstLevelSceneIndexProperty = serializedObject.FindProperty("_firstLevelSceneIndex");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(true);

            _totalSceneCountProperty.intValue = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes).Length;

            EditorGUILayout.PropertyField(_totalSceneCountProperty);

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(_firstLevelSceneIndexProperty);

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
