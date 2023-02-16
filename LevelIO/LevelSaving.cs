using Newtonsoft.Json;
using Pyrux.DataManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using Windows.Media.AppBroadcasting;
using Windows.Storage;

namespace Pyrux.LevelIO
{
    internal static class LevelSaving
    {
        public static async void Save(PyruxLevel activeLevel)
        {
            StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
            string levelJson = JsonConvert.SerializeObject(activeLevel);
            string levelName = activeLevel.LevelName;
            
            if(appdataFolder != null)
            {
                StorageFolder levelsFolder = ((StorageFolder)await appdataFolder.TryGetItemAsync("Levels"));
                if(levelsFolder != null)
                {
                    StorageFolder levelsFolderOrganization = null;
                    string levelDataPath;
                    string levelScriptPath;
                    
                    if(activeLevel.IsBuiltIn)
                    {
                        levelsFolderOrganization =  ((StorageFolder)await levelsFolder.TryGetItemAsync("Builtins"));
                    }
                    else
                    {
                        levelsFolderOrganization =  ((StorageFolder)await levelsFolder.TryGetItemAsync("UserCreated"));
                    }
                    StorageFolder levelFolder = (StorageFolder)await levelsFolderOrganization.TryGetItemAsync(levelName);

                    if (levelFolder == null)
                    {    
                        levelFolder = await levelsFolderOrganization.CreateFolderAsync(levelName);
                    }

                    levelDataPath = Path.Combine(levelFolder.Path, "LevelData.json");
                    levelScriptPath = Path.Combine(levelFolder.Path, "LevelScript.py");

                    if(File.Exists(levelDataPath))
                    {
                        File.Delete(levelDataPath);
                    }

                    if (File.Exists(levelScriptPath))
                    {
                        File.Delete(levelScriptPath);
                    }

                    using (StreamWriter sw = new(levelDataPath))
                    {
                        sw.Write(levelJson);
                    }

                    using (StreamWriter sw = new(levelScriptPath))
                    {
                        sw.Write(activeLevel.Script);
                    }
                    
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
