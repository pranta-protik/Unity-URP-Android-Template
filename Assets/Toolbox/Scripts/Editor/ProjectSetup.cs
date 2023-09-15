using System.Collections.Generic;
using System.IO;
using Toolbox.Utilities;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Toolbox.EditorScripts
{
    public static class ProjectSetup
    {
        public static void CreateDefaultFolders(string[] folders)
        {
            Folders.CreateDefault("_Project", folders);
            AssetDatabase.Refresh();
        }

        public static void CreateDefaultScenes(string[] scenes)
        {
            var rootPath = "_Project/Scenes";
            EditorBuildSettings.scenes = Scenes.CreateDefault(rootPath, scenes);

            DebugUtils.Log("Scenes added to editor build settings.");

            EditorSceneManager.OpenScene("Assets/" + rootPath + "/" + scenes[0] + ".unity");

            AssetDatabase.Refresh();
            GUIUtility.ExitGUI();
        }

        public static void ImportAssetStoreAssets(string rootFolder, string[] assets)
        {
            foreach (string asset in assets) Assets.ImportAsset(rootFolder, asset);
        }

        public static void ImportLocalDriveAssets(string rootFolder, string[] assets)
        {
            foreach (string asset in assets) Assets.ImportAsset(rootFolder, asset);
        }

        public static void InstallUnityPackages(string[] packages) => Packages.InstallPackages(packages);
        public static void InstallOpenSources(string[] openSources) => Packages.InstallPackages(openSources);

        private static class Folders
        {
            public static void CreateDefault(string root, string[] folders)
            {
                var fullpath = Path.Combine(Application.dataPath, root);

                if (!Directory.Exists(fullpath))
                {
                    Directory.CreateDirectory(fullpath);
                }

                foreach (var folder in folders)
                {
                    CreateSubFolders(fullpath, folder);
                }
            }

            private static void CreateSubFolders(string rootPath, string folderHierarchy)
            {
                var folders = folderHierarchy.Split('/');
                var currentPath = rootPath;

                foreach (var folder in folders)
                {
                    currentPath = Path.Combine(currentPath, folder);

                    if (!Directory.Exists(currentPath))
                    {
                        Directory.CreateDirectory(currentPath);
                    }
                }

                currentPath = rootPath;

                foreach (var folder in folders)
                {
                    currentPath = Path.Combine(currentPath, folder);

                    if (Directory.Exists(currentPath) && Directory.GetDirectories(currentPath).Length <= 0)
                    {
                        File.WriteAllText(currentPath + "/" + folder + ".keep", "");
                    }
                }
            }
        }

        private static class Scenes
        {
            public static EditorBuildSettingsScene[] CreateDefault(string root, string[] scenes)
            {
                var fullpath = Path.Combine(Application.dataPath, root);

                if (!Directory.Exists(fullpath))
                {
                    Directory.CreateDirectory(fullpath);
                }

                List<EditorBuildSettingsScene> buildSettingsScenesList = new();

                foreach (var scene in scenes)
                {
                    var createdScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                    EditorSceneManager.SaveScene(createdScene, fullpath + "/" + scene + ".unity", true);

                    buildSettingsScenesList.Add(new EditorBuildSettingsScene("Assets/" + root + "/" + scene + ".unity", true));
                }

                return buildSettingsScenesList.ToArray();
            }
        }

        private static class Assets
        {
            public static void ImportAsset(string rootFolder, string asset)
            {
                AssetDatabase.ImportPackage(Path.Combine(rootFolder, asset), false);
            }
        }

        private static class Packages
        {
            private static AddRequest Request;
            private static readonly Queue<string> PackagesToInstall = new();

            public static void InstallPackages(string[] packages)
            {
                DebugUtils.Log("Installing ... ...");

                foreach (var package in packages)
                {
                    PackagesToInstall.Enqueue(package);
                }

                // Start the installation of the first package
                if (PackagesToInstall.Count > 0)
                {
                    Request = Client.Add(PackagesToInstall.Dequeue());
                    EditorApplication.update += Progress;
                }
            }

            private static void Progress()
            {
                if (Request.IsCompleted)
                {
                    if (Request.Status == StatusCode.Success)
                    {
                        DebugUtils.Log("Installed: " + Request.Result.packageId);
                    }
                    else if (Request.Status >= StatusCode.Failure)
                    {
                        DebugUtils.LogError(Request.Error.message);
                    }

                    EditorApplication.update -= Progress;

                    // If there are more packages to install, start the next one
                    if (PackagesToInstall.Count > 0)
                    {
                        Request = Client.Add(PackagesToInstall.Dequeue());
                        EditorApplication.update += Progress;
                    }
                    else
                    {
                        DebugUtils.Log("All packages installed.");
                    }
                }
            }
        }
    }
}
