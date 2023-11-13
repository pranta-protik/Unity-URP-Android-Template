using UnityEditor;
using UnityEngine;

namespace Toolbox.EditorScripts
{
    public class SceneSetupWindow : EditorWindow
    {
        private const string WINDOW_TITLE = "Scene Setup";
        private const float WINDOW_WIDTH = 500f;
        private const float WINDOW_HEIGHT = 500f;
        private const float BUTTON_HEIGHT = 32f;
        private const float VERTICAL_SPACE = 10;
        private static SceneSetupWindow _SceneSetupWindow;
        private static GUIStyle _TitleLabelStyle;

        private void OnEnable()
        {
            _TitleLabelStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal =
                {
                    textColor = Color.white
                }
            };
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.Space(VERTICAL_SPACE);

            SetupPersistentScene();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            SetupSplashScene();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            SetupUIScene();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            SetupGameScene();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.EndVertical();

            if (_SceneSetupWindow) _SceneSetupWindow.Repaint();
        }

        private static void SetupPersistentScene()
        {
            GUILayout.Label("Setup Persistent Scene", _TitleLabelStyle);

            EditorGUILayout.BeginHorizontal();

            var relativePath = "_Project/Scenes/Persistent.unity";

            if (GUILayout.Button("Setup Scene", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                SceneSetup.SetupPersistentScene(relativePath);
            }

            if (GUILayout.Button("Open Scene", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                SceneSetup.TryOpenScene(relativePath);
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void SetupSplashScene()
        {
            GUILayout.Label("Setup Splash Scene", _TitleLabelStyle);

            EditorGUILayout.BeginHorizontal();

            var relativePath = "_Project/Scenes/Splash.unity";

            if (GUILayout.Button("Setup Scene", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                SceneSetup.SetupSplashScene(relativePath);
            }

            if (GUILayout.Button("Open Scene", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                SceneSetup.TryOpenScene(relativePath);
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void SetupUIScene()
        {
            GUILayout.Label("Setup UI Scene", _TitleLabelStyle);

            EditorGUILayout.BeginHorizontal();

            var relativePath = "_Project/Scenes/UI.unity";

            if (GUILayout.Button("Setup Scene", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                SceneSetup.SetupUIScene(relativePath);
            }

            if (GUILayout.Button("Open Scene", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                SceneSetup.TryOpenScene(relativePath);
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void SetupGameScene()
        {
            GUILayout.Label("Setup Game Scene", _TitleLabelStyle);

            EditorGUILayout.BeginHorizontal();

            var relativePath = "_Project/Scenes/Game.unity";

            if (GUILayout.Button("Setup Scene", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                SceneSetup.SetupGameScene(relativePath);
            }

            if (GUILayout.Button("Open Scene", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                SceneSetup.TryOpenScene(relativePath);
            }

            EditorGUILayout.EndHorizontal();
        }

        public static void InitWindow()
        {
            _SceneSetupWindow = GetWindow<SceneSetupWindow>(true, WINDOW_TITLE, true);
            _SceneSetupWindow.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
            _SceneSetupWindow.Show();
        }
    }
}