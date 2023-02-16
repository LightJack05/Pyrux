using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Pyrux.LevelIO
{
    internal class AppdataManagement
    {
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
                throw new FileNotFoundException();
            }
        }

        public static async Task<bool> VerifyAppdataIntegrityAsync()
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

            if(await levelsFolder.TryGetItemAsync("Builtins") == null)
            {
                return false;
            }

            if(await levelsFolder.TryGetItemAsync("UserCreated") == null)
            {
                return false;
            }

            return true;

        }

        public static async Task ClearAppdataAsync()
        {

            StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;

            var storageFolders = await appdataFolder.GetFoldersAsync();
            foreach (StorageFolder folder in storageFolders)
            {
                await folder.DeleteAsync();
            }

        }
    }


}
