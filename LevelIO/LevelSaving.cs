using IronPython.Modules;
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
            string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pyrux");
            string levelJson = JsonConvert.SerializeObject(activeLevel);
            string levelName = activeLevel.LevelName;

            if (Directory.Exists(appdataFolder))
            {
                string levelsFolder = Path.Combine(appdataFolder, "Levels");
                if (Directory.Exists(levelsFolder))
                {
                    string levelsFolderOrganization;

                    if (activeLevel.IsBuiltIn)
                    {
                        levelsFolderOrganization = Path.Combine(levelsFolder, "Builtins");
                    }
                    else
                    {
                        levelsFolderOrganization = Path.Combine(levelsFolder, "UserCreated");
                    }
                    string levelFolder = Path.Combine(levelsFolderOrganization, levelName);

                    if (!Directory.Exists(levelFolder))
                    {
                        Directory.CreateDirectory(levelFolder);
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

        public static async void Save(List<PyruxLevel> pyruxLevels)
        {
            foreach (PyruxLevel level in pyruxLevels)
            {
                Save(level);
            }
        }
        /// <summary>
        /// Write the required files for saving to the disk.
        /// </summary>
        /// <param name="activeLevel">The level to save.</param>
        /// <param name="levelFolder">The StorageFolder to save the level to. Determined by the "Save" method.</param>
        private static void WriteFilesToStorage(PyruxLevel activeLevel, string levelFolder)
        {
            string levelJson = JsonConvert.SerializeObject(activeLevel);
            string levelDataPath = Path.Combine(levelFolder, "LevelData.json");
            string levelScriptPath = Path.Combine(levelFolder, "LevelScript.py");

            if (!activeLevel.IsBuiltIn)
            {
                if (File.Exists(levelDataPath))
                {
                    File.Delete(levelDataPath);
                }

                using (StreamWriter sw = new(levelDataPath))
                {
                    sw.Write(levelJson);
                }
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
