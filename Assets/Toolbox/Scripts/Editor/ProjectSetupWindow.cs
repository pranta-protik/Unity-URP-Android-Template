using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Toolbox.EditorScripts
{
    public class ProjectSetupWindow : EditorWindow
    {
        private const string WINDOW_TITLE = "Project Setup";
        private const float WINDOW_WIDTH = 500f;
        private const float WINDOW_HEIGHT = 880f;
        private const float BUTTON_WIDTH = 100f;
        private const float BUTTON_HEIGHT = 32f;
        private const float SCROLL_VIEW_WIDTH = 500f;
        private const float SCROLL_VIEW_HEIGHT = 65f;
        private const float VERTICAL_SPACE = 10;
        private static ProjectSetupWindow _ProjectSetupWindow;
        private static GUIStyle _TitleLabelStyle;
        private static Vector2 _ScrollPosForDefaultFolders;
        private static Vector2 _ScrollPosForDefaultScenes;
        private static Vector2 _ScrollPosForUnityPackages;
        private static Vector2 _ScrollPosForOpenSources;
        private static Vector2 _ScrollPosForAssetStoreAssets;
        private static Vector2 _ScrollPosForLocalDriveAssets;

        private void OnEnable()
        {
            _TitleLabelStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            _TitleLabelStyle.normal.textColor = Color.white;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.Space(VERTICAL_SPACE);

            CreateDefaultFolders();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            CreateDefaultScenes();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            InstallUnityPackages();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            InstallOpenSources();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            ImportAssetStoreAssets();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            ImportLocalDriveAssets();

            EditorGUILayout.EndVertical();

            if (_ProjectSetupWindow) _ProjectSetupWindow.Repaint();
        }

        private static void CreateDefaultFolders()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Create Default Folders", _TitleLabelStyle);

            _ScrollPosForDefaultFolders = EditorGUILayout.BeginScrollView(_ScrollPosForDefaultFolders, GUILayout.Width(SCROLL_VIEW_WIDTH), GUILayout.Height(SCROLL_VIEW_HEIGHT));

            EditorGUI.BeginDisabledGroup(true);

            var defaultFoldersListFileData = ReadFromFile("Assets/Toolbox/Presets/DefaultFoldersList.txt");
            var defaultFoldersNameStr = "";

            if (defaultFoldersListFileData != null)
            {
                foreach (var defaultFolderName in defaultFoldersListFileData)
                {
                    defaultFoldersNameStr += defaultFolderName + "\n";
                }
            }

            EditorGUILayout.TextArea(defaultFoldersNameStr.TrimEnd(), GUILayout.ExpandHeight(true));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndScrollView();


            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Create", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                if (defaultFoldersListFileData != null)
                {
                    ProjectSetup.CreateDefaultFolders(defaultFoldersListFileData);
                }
            }

            if (GUILayout.Button("Edit", GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(BUTTON_HEIGHT)))
            {
                var path = Application.dataPath + "/Toolbox/Presets/DefaultFoldersList.txt";

                try
                {
                    System.Diagnostics.Process.Start(path);
                }
                catch
                {
                    Debug.LogError(path + " not found.");
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private static void CreateDefaultScenes()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Create Default Scenes", _TitleLabelStyle);

            _ScrollPosForDefaultScenes = EditorGUILayout.BeginScrollView(_ScrollPosForDefaultScenes, GUILayout.Width(SCROLL_VIEW_WIDTH), GUILayout.Height(SCROLL_VIEW_HEIGHT));

            EditorGUI.BeginDisabledGroup(true);

            var defaultScenesListFileData = ReadFromFile("Assets/Toolbox/Presets/DefaultScenesList.txt");
            var defaultScenesNameStr = "";

            if (defaultScenesListFileData != null)
            {
                foreach (var defaultSceneName in defaultScenesListFileData)
                {
                    defaultScenesNameStr += defaultSceneName + "\n";
                }
            }

            EditorGUILayout.TextArea(defaultScenesNameStr.TrimEnd(), GUILayout.ExpandHeight(true));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndScrollView();


            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Create", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                if (defaultScenesListFileData != null)
                {
                    ProjectSetup.CreateDefaultScenes(defaultScenesListFileData);
                }
            }

            if (GUILayout.Button("Edit", GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(BUTTON_HEIGHT)))
            {
                var path = Application.dataPath + "/Toolbox/Presets/DefaultScenesList.txt";

                try
                {
                    System.Diagnostics.Process.Start(path);
                }
                catch
                {
                    Debug.LogError(path + " not found.");
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private static void InstallUnityPackages()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Install Unity Packages", _TitleLabelStyle);

            _ScrollPosForUnityPackages = EditorGUILayout.BeginScrollView(_ScrollPosForUnityPackages, GUILayout.Width(SCROLL_VIEW_WIDTH), GUILayout.Height(SCROLL_VIEW_HEIGHT));

            EditorGUI.BeginDisabledGroup(true);

            var unityPackageListFileData = ReadFromFile("Assets/Toolbox/Presets/UnityPackagesList.txt");
            var packageNameStr = "";

            if (unityPackageListFileData != null)
            {
                foreach (var packageName in unityPackageListFileData)
                {
                    packageNameStr += "\"" + packageName + "\"" + "\n";
                }
            }

            EditorGUILayout.TextArea(packageNameStr.TrimEnd(), GUILayout.ExpandHeight(true));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Install", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                if (unityPackageListFileData != null)
                {
                    ProjectSetup.InstallUnityPackages(unityPackageListFileData);
                }
            }

            if (GUILayout.Button("Edit", GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(BUTTON_HEIGHT)))
            {
                var path = Application.dataPath + "/Toolbox/Presets/UnityPackagesList.txt";
                try
                {
                    System.Diagnostics.Process.Start(path);
                }
                catch
                {
                    Debug.LogError(path + " not found.");
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private static void InstallOpenSources()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Install Open Sources", _TitleLabelStyle);

            _ScrollPosForOpenSources = EditorGUILayout.BeginScrollView(_ScrollPosForOpenSources, GUILayout.Width(SCROLL_VIEW_WIDTH), GUILayout.Height(SCROLL_VIEW_HEIGHT));

            EditorGUI.BeginDisabledGroup(true);

            var openSourcesListFileData = ReadFromFile("Assets/Toolbox/Presets/OpenSourcesList.txt");
            var openSourceNameStr = "";

            if (openSourcesListFileData != null)
            {
                foreach (var openSourceName in openSourcesListFileData)
                {
                    openSourceNameStr += "\"" + openSourceName + "\"" + "\n";
                }
            }

            EditorGUILayout.TextArea(openSourceNameStr.TrimEnd(), GUILayout.ExpandHeight(true));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Install", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                if (openSourcesListFileData != null)
                {
                    ProjectSetup.InstallOpenSources(openSourcesListFileData);
                }
            }

            if (GUILayout.Button("Edit", GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(BUTTON_HEIGHT)))
            {
                var path = Application.dataPath + "/Toolbox/Presets/OpenSourcesList.txt";
                try
                {
                    System.Diagnostics.Process.Start(path);
                }
                catch
                {
                    Debug.LogError(path + " not found.");
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private static void ImportAssetStoreAssets()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Import Asset Store Assets (Optional)", _TitleLabelStyle);

            EditorGUILayout.BeginHorizontal();

            var assetStoreAssetsRootFileData = ReadFromFile("Assets/Toolbox/Presets/AssetStoreAssetsRoot.txt");
            var assetStoreAssetsRootStr = "";

            if (assetStoreAssetsRootFileData != null && assetStoreAssetsRootFileData.Length > 0)
            {
                assetStoreAssetsRootStr = assetStoreAssetsRootFileData[0];
            }

            GUILayout.Label(assetStoreAssetsRootStr, GUILayout.Height(BUTTON_HEIGHT));

            if (GUILayout.Button("Change Path", GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(32)))
            {
                var directoryPath = EditorUtility.OpenFolderPanel("Select Directory", "", "");
                if (directoryPath != "")
                {
                    assetStoreAssetsRootStr = directoryPath;
                    WriteToFile("Assets/Toolbox/Presets/AssetStoreAssetsRoot.txt", assetStoreAssetsRootStr);
                }
            }

            EditorGUILayout.EndHorizontal();

            _ScrollPosForAssetStoreAssets = EditorGUILayout.BeginScrollView(_ScrollPosForAssetStoreAssets, GUILayout.Width(SCROLL_VIEW_WIDTH), GUILayout.Height(SCROLL_VIEW_HEIGHT));

            EditorGUI.BeginDisabledGroup(true);

            var assetStoreAssetsListFileData = ReadFromFile("Assets/Toolbox/Presets/AssetStoreAssetsList.txt");
            var assetStoreAssetNameStr = "";

            if (assetStoreAssetsListFileData != null)
            {
                foreach (var assetStoreAssetName in assetStoreAssetsListFileData)
                {
                    assetStoreAssetNameStr += assetStoreAssetName + "\n";
                }
            }

            EditorGUILayout.TextArea(assetStoreAssetNameStr.TrimEnd(), GUILayout.ExpandHeight(true));

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Install", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                if (assetStoreAssetsListFileData != null)
                {
                    ProjectSetup.ImportAssetStoreAssets(assetStoreAssetsRootStr, assetStoreAssetsListFileData);
                }
            }

            if (GUILayout.Button("Edit", GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(BUTTON_HEIGHT)))
            {
                var path = Application.dataPath + "/Toolbox/Presets/AssetStoreAssetsList.txt";
                try
                {
                    System.Diagnostics.Process.Start(path);
                }
                catch
                {
                    Debug.LogError(path + " not found.");
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private static void ImportLocalDriveAssets()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Import Local Drive Assets (Optional)", _TitleLabelStyle);

            EditorGUILayout.BeginHorizontal();

            var localDriveAssetsRootFileData = ReadFromFile("Assets/Toolbox/Presets/LocalDriveAssetsRoot.txt");
            var localDriveAssetsRootStr = "";

            if (localDriveAssetsRootFileData != null && localDriveAssetsRootFileData.Length > 0)
            {
                localDriveAssetsRootStr = localDriveAssetsRootFileData[0];
            }

            GUILayout.Label(localDriveAssetsRootStr, GUILayout.Height(BUTTON_HEIGHT));

            if (GUILayout.Button("Change Path", GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(32)))
            {
                var directoryPath = EditorUtility.OpenFolderPanel("Select Directory", "", "");
                if (directoryPath != "")
                {
                    localDriveAssetsRootStr = directoryPath;
                    WriteToFile("Assets/Toolbox/Presets/LocalDriveAssetsRoot.txt", localDriveAssetsRootStr);
                }
            }

            EditorGUILayout.EndHorizontal();

            _ScrollPosForLocalDriveAssets = EditorGUILayout.BeginScrollView(_ScrollPosForLocalDriveAssets, GUILayout.Width(SCROLL_VIEW_WIDTH), GUILayout.Height(SCROLL_VIEW_HEIGHT));

            EditorGUI.BeginDisabledGroup(true);

            var localDriveAssetsListFileData = ReadFromFile("Assets/Toolbox/Presets/LocalDriveAssetsList.txt");
            var localDriveAssetNameStr = "";

            if (localDriveAssetsListFileData != null)
            {
                foreach (var localDriveAssetName in localDriveAssetsListFileData)
                {
                    localDriveAssetNameStr += localDriveAssetName + "\n";
                }
            }

            EditorGUILayout.TextArea(localDriveAssetNameStr.TrimEnd(), GUILayout.ExpandHeight(true));

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Install", GUILayout.ExpandWidth(true), GUILayout.Height(BUTTON_HEIGHT)))
            {
                if (localDriveAssetsListFileData != null)
                {
                    ProjectSetup.ImportAssetStoreAssets(localDriveAssetsRootStr, localDriveAssetsListFileData);
                }
            }

            if (GUILayout.Button("Edit", GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(BUTTON_HEIGHT)))
            {
                var path = Application.dataPath + "/Toolbox/Presets/LocalDriveAssetsList.txt";
                try
                {
                    System.Diagnostics.Process.Start(path);
                }
                catch
                {
                    Debug.LogError(path + " not found.");
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private static string[] ReadFromFile(string path)
        {
            try
            {
                var outputStr = File.ReadAllText(path);
                List<string> outputList = new();

                // Remove empty lines
                var pattern = @"^\s*$";
                var rg = new Regex(pattern);

                foreach (var output in outputStr.Split(','))
                {
                    if (!rg.IsMatch(output))
                    {
                        outputList.Add(output.Trim());
                    }
                }
                return outputList.ToArray();
            }
            catch
            {
                Debug.LogError(path + " not found.");
                return null;
            }
        }

        private static void WriteToFile(string path, string data)
        {
            try
            {
                File.WriteAllText(path, data);
            }
            catch
            {
                Debug.LogError(path + " not found.");
            }
        }

        public static void InitWindow()
        {
            _ProjectSetupWindow = GetWindow<ProjectSetupWindow>(true, WINDOW_TITLE, true);
            _ProjectSetupWindow.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
            _ProjectSetupWindow.Show();
        }
    }
}
