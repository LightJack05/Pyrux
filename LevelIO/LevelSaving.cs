using Windows.Storage;

namespace Pyrux.LevelIO
{
    internal static class LevelSaving
    {
        /// <summary>
        /// Save the given level to the Appdata folder it belongs in.
        /// </summary>
        /// <param name="activeLevel">The level to save.</param>
        /// <exception cref="NotImplementedException">Thrown if the appdata folder is inaccessible or corrupted.</exception>
        public static async void Save(PyruxLevel activeLevel)
        {
            await AppdataManagement.CheckAppdata();
            StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
            string levelJson = JsonConvert.SerializeObject(activeLevel);
            string levelName = activeLevel.LevelName;

            if (appdataFolder != null)
            {
                StorageFolder levelsFolder = (StorageFolder)await appdataFolder.TryGetItemAsync("Levels");
                if (levelsFolder != null)
                {
                    StorageFolder levelsFolderOrganization = null;

                    if (activeLevel.IsBuiltIn)
                    {
                        levelsFolderOrganization = (StorageFolder)await levelsFolder.TryGetItemAsync("Builtins");
                    }
                    else
                    {
                        levelsFolderOrganization = (StorageFolder)await levelsFolder.TryGetItemAsync("UserCreated");
                    }
                    StorageFolder levelFolder = (StorageFolder)await levelsFolderOrganization.TryGetItemAsync(levelName);

                    if (levelFolder == null)
                    {
                        levelFolder = await levelsFolderOrganization.CreateFolderAsync(levelName);
                    }

                    WriteFilesToStorage(activeLevel, levelFolder);

                }
                else
                {
                    throw new LevelsFolderNotFoundException();
                }
            }
            else
            {
                throw new AppdataFolderNotFoundException();
            }
        }
        /// <summary>
        /// Write the required files for saving to the disk.
        /// </summary>
        /// <param name="activeLevel">The level to save.</param>
        /// <param name="levelFolder">The StorageFolder to save the level to. Determined by the "Save" method.</param>
        private static void WriteFilesToStorage(PyruxLevel activeLevel, StorageFolder levelFolder)
        {
            string levelJson = JsonConvert.SerializeObject(activeLevel);
            string levelDataPath = Path.Combine(levelFolder.Path, "LevelData.json");
            string levelScriptPath = Path.Combine(levelFolder.Path, "LevelScript.py");

            if (!activeLevel.IsBuiltIn)
            {
                if (File.Exists(levelDataPath))
                {
                    File.Delete(levelDataPath);
                }

                using StreamWriter sw = new(levelDataPath);
                sw.Write(levelJson);
            }

            if (File.Exists(levelScriptPath))
            {
                File.Delete(levelScriptPath);
            }


            using (StreamWriter sw = new(levelScriptPath))
            {
                sw.Write(activeLevel.Script);
            }
        }
    }
}
