using System.Threading.Tasks;
using Windows.Storage;

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
            StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
            if ((await appdataFolder.GetFoldersAsync()).Count == 0)
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
            StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
            System.Diagnostics.Debug.WriteLine(appdataFolder.Path.ToString());
            if (appdataFolder != null)
            {
                StorageFolder levelsFolder = (StorageFolder)await appdataFolder.TryGetItemAsync("Levels");

                if (await appdataFolder.TryGetItemAsync("Levels") == null)
                {
                    levelsFolder = await appdataFolder.CreateFolderAsync("Levels");
                }
                else
                {
                    levelsFolder = (StorageFolder)await appdataFolder.TryGetItemAsync("Levels");
                }

                if (await levelsFolder.TryGetItemAsync("Builtins") == null)
                {
                    StorageFolder builtinsFolder = await levelsFolder.CreateFolderAsync("Builtins");
                }
                else
                {
                    StorageFolder builtinsFolder = (StorageFolder)await levelsFolder.TryGetItemAsync("Builtins");
                }

                if (await levelsFolder.TryGetItemAsync("UserCreated") == null)
                {
                    StorageFolder userCreatedFolder = await levelsFolder.CreateFolderAsync("UserCreated");
                }
                else
                {
                    StorageFolder userCreatedFolder = (StorageFolder)await levelsFolder.TryGetItemAsync("UserCreated");
                }


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
                StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
                StorageFolder levelsFolder = (StorageFolder)await appdataFolder.TryGetItemAsync("Levels");

                if (appdataFolder == null)
                {
                    return false;
                }

                if (await appdataFolder.TryGetItemAsync("Levels") == null)
                {
                    return false;
                }

                if (await levelsFolder.TryGetItemAsync("Builtins") == null)
                {
                    return false;
                }

                if (await levelsFolder.TryGetItemAsync("UserCreated") == null)
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

            StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
            if (appdataFolder == null)
            {
                throw new AppdataFolderNotFoundException();
            }

            IReadOnlyList<IStorageItem> storageItems = await appdataFolder.GetItemsAsync();
            foreach (IStorageItem item in storageItems)
            {
                await item.DeleteAsync();
            }

        }
    }


}
