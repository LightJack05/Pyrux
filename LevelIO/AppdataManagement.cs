using System.Threading.Tasks;

namespace Pyrux.LevelIO
{
    // Check this:
    // localFolder = Task.Run(async () => await StorageFolder.GetFolderFromPathAsync(System.AppContext.BaseDirectory)).Result;
    internal class AppdataManagement
    {
        /// <summary>
        /// True if the current appdata folder appears to be corrupted. Otherwise false.
        /// </summary>
        public static bool AppdataCorrupted { get; set; } = false;
        /// <summary>
        /// True if the appdata validation has been completed. Otherwise false. Does not indicate that the validation was successful.
        /// </summary>
        public static bool AppdataValidationCompleted { get; private set; } = false;
        /// <summary>
        /// Check if the current Appdata is still consistent with the layout it should have, and rebuild it if it isn't.
        /// </summary>
        public static async Task CheckAppdata()
        {
            if (!await VerifyAppdataIntegrityAsync())
            {
                if (await IsAppdataEmpty())
                {
                    await ResetAppdata();
                }
                else
                {
                    AppdataCorrupted = true;
                    AppdataValidationCompleted = true;
                }
            }
            else
            {
                AppdataValidationCompleted = true;
            }
        }
        /// <summary>
        /// Check if the current appdata folder is empty.
        /// </summary>
        /// <returns>True if the appdata folder is empty.</returns>
        public static async Task<bool> IsAppdataEmpty()
        {
            string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pyrux");
            if (!Directory.Exists(appdataFolder))
            {
                Directory.CreateDirectory(appdataFolder);
            }
            if (Directory.GetFileSystemEntries(appdataFolder).Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Reset the appdata folder to defaults. Deletes all contents and then recreates the folder structure.
        /// </summary>
        public static async Task ResetAppdata()
        {
            AppdataCorrupted = false;
            AppdataValidationCompleted = true;
            await ClearAppdataAsync();
            await ConstructAppdataAsync();
        }
        /// <summary>
        /// Create the default Appdata folder structure.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AppdataFolderNotFoundException">Thrown when the top level appdata folder could not be found.</exception>
        public static async Task ConstructAppdataAsync()
        {
            string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pyrux");
            if (Directory.Exists(appdataFolder))
            {
                string levelsFolder = Path.Combine(appdataFolder, "Levels");

                if (!Directory.Exists(levelsFolder))
                {
                    Directory.CreateDirectory(levelsFolder);
                }

                string builtInLevelsFolder = Path.Combine(levelsFolder, "Builtins");
                if (!Directory.Exists(builtInLevelsFolder))
                {
                    Directory.CreateDirectory(builtInLevelsFolder);
                }
                string userCreatedLevelsFolder = Path.Combine(levelsFolder, "UserCreated");
                if (!Directory.Exists(userCreatedLevelsFolder))
                {
                    Directory.CreateDirectory(userCreatedLevelsFolder);
                }

                BuiltInLevels.ConstructLevels();
            }
            else
            {
                throw new AppdataFolderNotFoundException();
            }
        }

        /// <summary>
        /// Verify that the Appdata folder's integrity is given.
        /// </summary>
        /// <returns>True when integrity is given, false if not.</returns>
        public static async Task<bool> VerifyAppdataIntegrityAsync()
        {
            try
            {
                string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pyrux");

                string levelsFolder = Path.Combine(appdataFolder, "Levels");
                string builtInLevelsFolder = Path.Combine(levelsFolder, "Builtins");
                string userCreatedLevelsFolder = Path.Combine(levelsFolder, "UserCreated");

                if (!Directory.Exists(appdataFolder))
                {
                    return false;
                }

                if (!Directory.Exists(levelsFolder))
                {
                    return false;
                }

                if (!Directory.Exists(builtInLevelsFolder))
                {
                    return false;
                }

                if (!Directory.Exists(userCreatedLevelsFolder))
                {
                    return false;
                }

                return true;
            }
            catch (Exception) { return false; }
        }

        /// <summary>
        /// Delete the entire contents of the appdata folder, including the directory structure.
        /// </summary>
        /// <exception cref="AppdataFolderNotFoundException">Thrown when the top level appdata folder could not be found.</exception>
        public static async Task ClearAppdataAsync()
        {

            string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pyrux");

            if (!Directory.Exists(appdataFolder))
            {
                throw new AppdataFolderNotFoundException();
            }

            Directory.Delete(appdataFolder, true);
            Directory.CreateDirectory(appdataFolder);

        }
    }


}
