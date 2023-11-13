using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace Toolbox.EditorScripts
{
    public class AndroidSetupWindow : EditorWindow
    {
        private const string WINDOW_TITLE = "Android Setup";
        private const float WINDOW_WIDTH = 500f;
        private const float WINDOW_HEIGHT = 500f;
        private const float BUTTON_HEIGHT = 32f;
        private const float VERTICAL_SPACE = 10;
        private static AndroidSetupWindow _AndroidSetupWindow;
        private static GUIStyle _TitleLabelStyle;

        private string _rootNamespace = "Project";
        private bool _enterPlayModeOptions = true;
        private string _companyName = "Manabreak";
        private string _productName = "New Game";
        private UIOrientation _defaultOrientation = UIOrientation.Portrait;
        private string _packageName = "com.manabreak.newgame";
        private AndroidSdkVersions _minimumAPILevel = AndroidSdkVersions.AndroidApiLevel22;
        private AndroidSdkVersions _targetAPILevel = AndroidSdkVersions.AndroidApiLevelAuto;
        private ScriptingImplementation _scriptingBackend = ScriptingImplementation.IL2CPP;
        private ApiCompatibilityLevel _apiCompatibilityLevel = ApiCompatibilityLevel.NET_Standard;
        private AndroidArchitecture _targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;

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

            GUILayout.Label("Android Settings", _TitleLabelStyle);

            _rootNamespace = EditorGUILayout.TextField("Root Namespace", _rootNamespace);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _enterPlayModeOptions = EditorGUILayout.Toggle("Enter Play Mode Options", _enterPlayModeOptions);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _companyName = EditorGUILayout.TextField("Company Name", _companyName);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _productName = EditorGUILayout.TextField("Product Name", _productName);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _defaultOrientation = (UIOrientation)EditorGUILayout.EnumPopup("Default Orientation", _defaultOrientation);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _packageName = EditorGUILayout.TextField("Package Name", _packageName);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _minimumAPILevel = (AndroidSdkVersions)EditorGUILayout.EnumPopup("Minimum API Level", _minimumAPILevel);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _targetAPILevel = (AndroidSdkVersions)EditorGUILayout.EnumPopup("Target API Level", _targetAPILevel);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _scriptingBackend = (ScriptingImplementation)EditorGUILayout.EnumPopup("Scripting Backend", _scriptingBackend);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _apiCompatibilityLevel = (ApiCompatibilityLevel)EditorGUILayout.EnumPopup("API Compatibility Level", _apiCompatibilityLevel);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _targetArchitectures = (AndroidArchitecture)EditorGUILayout.EnumFlagsField("Target Architectures", _targetArchitectures);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if (GUILayout.Button("Apply Settings", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                EditorSettings.projectGenerationRootNamespace = _rootNamespace;
                EditorSettings.enterPlayModeOptionsEnabled = _enterPlayModeOptions;
                EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.None;
                EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload;
                PlayerSettings.companyName = _companyName;
                PlayerSettings.productName = _productName;
                PlayerSettings.defaultInterfaceOrientation = _defaultOrientation;
                PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, _packageName);
                PlayerSettings.Android.minSdkVersion = _minimumAPILevel;
                PlayerSettings.Android.targetSdkVersion = _targetAPILevel;
                PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, _scriptingBackend);
                PlayerSettings.SetApiCompatibilityLevel(NamedBuildTarget.Android, _apiCompatibilityLevel);
                PlayerSettings.Android.targetArchitectures = _targetArchitectures;
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // SetupUIScene();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // SetupGameScene();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.EndVertical();

            if (_AndroidSetupWindow) _AndroidSetupWindow.Repaint();
        }

        private static void SetupAndroidSettings()
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

        public static void InitWindow()
        {
            _AndroidSetupWindow = GetWindow<AndroidSetupWindow>(true, WINDOW_TITLE, true);
            _AndroidSetupWindow.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
            _AndroidSetupWindow.Show();
        }
    }
}