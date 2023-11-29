using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace Toolbox.EditorScripts
{
    public class AndroidBuildSetupWindow : EditorWindow
    {
        private const string WINDOW_TITLE = "Android Setup";
        private const float WINDOW_WIDTH = 500f;
        private const float WINDOW_HEIGHT = 500f;
        private const float BUTTON_HEIGHT = 32f;
        private const float VERTICAL_SPACE = 10;
        private static AndroidBuildSetupWindow _AndroidBuildSetupWindow;
        private static GUIStyle _TitleLabelStyle;
        private Vector2 _scrollPosition;
        private bool _showEditorSettings;
        private bool _showPlayerSettings;
        private bool _showQualitySettings;

        private string _rootNamespace;
        private bool _enableEnterPlayModeOptions;
        private EnterPlayModeOptions _enterPlayModeOptions;
        private EditorSettings.NamingScheme _gameObjectNaming;

        private string _companyName;
        private string _productName;
        private UIOrientation _defaultOrientation;
        private string _packageName;
        private AndroidSdkVersions _minimumAPILevel;
        private AndroidSdkVersions _targetAPILevel;
        private ScriptingImplementation _scriptingBackend;
        private ApiCompatibilityLevel _apiCompatibilityLevel;
        private AndroidArchitecture _targetArchitectures;

        private enum VSyncCount
        {
            DontSync = 0,
            EveryVBlank = 1,
            EverySecondVBlank = 2
        }

        private VSyncCount _vSyncCount;

        private void OnEnable()
        {
            _rootNamespace = EditorSettings.projectGenerationRootNamespace;
            _enableEnterPlayModeOptions = EditorSettings.enterPlayModeOptionsEnabled;
            _enterPlayModeOptions = EditorSettings.enterPlayModeOptions;
            _gameObjectNaming = EditorSettings.gameObjectNamingScheme;

            _companyName = PlayerSettings.companyName;
            _productName = PlayerSettings.productName;
            _defaultOrientation = PlayerSettings.defaultInterfaceOrientation;
            _packageName = PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.Android);
            _minimumAPILevel = PlayerSettings.Android.minSdkVersion;
            _targetAPILevel = PlayerSettings.Android.targetSdkVersion;
            _scriptingBackend = PlayerSettings.GetScriptingBackend(NamedBuildTarget.Android);
            _apiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(NamedBuildTarget.Android);
            _targetArchitectures = PlayerSettings.Android.targetArchitectures;

            _vSyncCount = (VSyncCount)QualitySettings.vSyncCount;

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

            GUILayout.Label("Android Build Settings", _TitleLabelStyle);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandWidth(true));

            _showEditorSettings = EditorGUILayout.Foldout(_showEditorSettings, "Editor Settings");

            if (_showEditorSettings)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                _rootNamespace = EditorGUILayout.TextField("Root Namespace", _rootNamespace);

                _enableEnterPlayModeOptions = EditorGUILayout.Toggle("Enter Play Mode Options", _enableEnterPlayModeOptions);

                _enterPlayModeOptions = (EnterPlayModeOptions)EditorGUILayout.EnumFlagsField("Enter Play Mode Options", _enterPlayModeOptions);

                _gameObjectNaming = (EditorSettings.NamingScheme)EditorGUILayout.EnumPopup("Game Object Naming", _gameObjectNaming);

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }

            _showPlayerSettings = EditorGUILayout.Foldout(_showPlayerSettings, "Player Settings");

            if (_showPlayerSettings)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                _companyName = EditorGUILayout.TextField("Company Name", _companyName);

                _productName = EditorGUILayout.TextField("Product Name", _productName);

                _defaultOrientation = (UIOrientation)EditorGUILayout.EnumPopup("Default Orientation", _defaultOrientation);

                _packageName = EditorGUILayout.TextField("Package Name", _packageName);

                _minimumAPILevel = (AndroidSdkVersions)EditorGUILayout.EnumPopup("Minimum API Level", _minimumAPILevel);

                _targetAPILevel = (AndroidSdkVersions)EditorGUILayout.EnumPopup("Target API Level", _targetAPILevel);

                _scriptingBackend = (ScriptingImplementation)EditorGUILayout.EnumPopup("Scripting Backend", _scriptingBackend);

                _apiCompatibilityLevel = (ApiCompatibilityLevel)EditorGUILayout.EnumPopup("API Compatibility Level", _apiCompatibilityLevel);

                _targetArchitectures = (AndroidArchitecture)EditorGUILayout.EnumFlagsField("Target Architectures", _targetArchitectures);

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }

            _showQualitySettings = EditorGUILayout.Foldout(_showQualitySettings, "Quality Settings");

            if (_showQualitySettings)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                _vSyncCount = (VSyncCount)EditorGUILayout.EnumPopup("V Sync Count", _vSyncCount);

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }

            EditorGUILayout.Space(VERTICAL_SPACE);

            if (GUILayout.Button("Apply Settings", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                EditorSettings.projectGenerationRootNamespace = _rootNamespace;
                EditorSettings.enterPlayModeOptionsEnabled = _enableEnterPlayModeOptions;
                EditorSettings.enterPlayModeOptions = _enterPlayModeOptions;
                EditorSettings.gameObjectNamingScheme = _gameObjectNaming;

                PlayerSettings.companyName = _companyName;
                PlayerSettings.productName = _productName;
                PlayerSettings.defaultInterfaceOrientation = _defaultOrientation;
                PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, _packageName);
                PlayerSettings.Android.minSdkVersion = _minimumAPILevel;
                PlayerSettings.Android.targetSdkVersion = _targetAPILevel;
                PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, _scriptingBackend);
                PlayerSettings.SetApiCompatibilityLevel(NamedBuildTarget.Android, _apiCompatibilityLevel);
                PlayerSettings.Android.targetArchitectures = _targetArchitectures;

                QualitySettings.vSyncCount = (int)_vSyncCount;
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();

            if (_AndroidBuildSetupWindow) _AndroidBuildSetupWindow.Repaint();
        }

        public static void InitWindow()
        {
            _AndroidBuildSetupWindow = GetWindow<AndroidBuildSetupWindow>(true, WINDOW_TITLE, true);
            _AndroidBuildSetupWindow.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
            _AndroidBuildSetupWindow.Show();
        }
    }
}