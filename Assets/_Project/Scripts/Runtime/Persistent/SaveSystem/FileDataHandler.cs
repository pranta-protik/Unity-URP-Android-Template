using System;
using System.IO;
using Toolbox.Utilities;
using UnityEngine;

namespace Project.Persistent.SaveSystem
{
    public class FileDataHandler
    {
        private const string ENCRYPTION_CODE_WORD = "ONEPIECE";
        private const string BACKUP_FILE_EXTENSION = ".bak";

        private readonly string _dataDirPath;
        private readonly string _dataFileName;
        private readonly bool _useEncryption = false;

        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
        {
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
            _useEncryption = useEncryption;
        }

        public GameData Load(bool allowRestoreFromBackup = true)
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);

            GameData loadedGameData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    // Load the serialized data from the file
                    var dataToLoad = File.ReadAllText(fullPath);

                    // Optionally decrypt the data
                    if (_useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    loadedGameData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    // Since we're calling Load(..) recursively, we need to account for the case where
                    // the rollback succeeds, but data is still failing to load for some other reason,
                    // which without this check may cause an infinite recursion loop.

                    if (allowRestoreFromBackup)
                    {
                        DebugUtils.LogWarning("Failed to load data file. Attempting to rollback.\n" + e.Message);

                        var rollbackSuccess = AttemptRollback(fullPath);

                        if (rollbackSuccess)
                        {
                            // Try to load again recursively
                            loadedGameData = Load(false);
                        }
                    }
                    else
                    {
                        DebugUtils.LogError("Error occured when trying to load file at path: " + fullPath + " and backup did not work.\n" + e.Message);
                    }
                }
            }

            return loadedGameData;
        }

        public void Save(GameData gameData)
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            var backupFilePath = fullPath + BACKUP_FILE_EXTENSION;

            try
            {
                // Create the directory the file will be written to if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // Serialize the C# game data object into json
                var dataToStore = JsonUtility.ToJson(gameData, true);

                // Optionally encrypt the data
                if (_useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                // Write the serialized data to the file
                File.WriteAllText(fullPath, dataToStore);

                // Verify the newly saved file can be loaded successfully
                var varifiedGameData = Load();

                // If the data can be varified, back it up
                if (varifiedGameData != null)
                {
                    File.Copy(fullPath, backupFilePath, true);
                }
                else
                {
                    throw new Exception("Save file could not be varified and backup could not be created.");
                }
            }
            catch (Exception e)
            {
                DebugUtils.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e.Message);
            }
        }

        // The below is a simple implementation of XOR encryption
        private string EncryptDecrypt(string data)
        {
            var modifiedData = "";

            for (var i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ ENCRYPTION_CODE_WORD[i % ENCRYPTION_CODE_WORD.Length]);
            }

            return modifiedData;
        }

        private bool AttemptRollback(string fullPath)
        {
            var success = false;
            var backupFilePath = fullPath + BACKUP_FILE_EXTENSION;

            try
            {
                // If the file exists, attempt to rollback to it by overwriting the original file
                if (File.Exists(backupFilePath))
                {
                    File.Copy(backupFilePath, fullPath, true);
                    success = true;
                    DebugUtils.LogWarning("Had to roll back to back file at: " + backupFilePath);
                }
                // Otherwise, there's no backup file - so there's nothing to rollback to
                else
                {
                    throw new Exception("Tried to rollback, but no backup file exists to rollback to.");
                }
            }
            catch (Exception e)
            {
                DebugUtils.LogError("Error occured when trying to rollback to backup file at: " + backupFilePath + "\n" + e.Message);
            }

            return success;
        }
    }
}
