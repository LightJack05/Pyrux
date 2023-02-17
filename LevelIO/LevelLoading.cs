using System.IO;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pyrux.DataManagement;
using Pyrux.Pages;
using Windows.Storage;

namespace Pyrux.LevelIO;


internal static class LevelLoading
{
    /// <summary>
    /// Load a level from disk.
    /// </summary>
    /// <param name="isBuiltIn">Determines whether the level is a built in level.</param>
    /// <param name="levelName">The name of the level to load.</param>
    /// <exception cref="LevelJsonNotFoundException">Thrown when the JSON file containing level data can not be found.</exception>
    /// <exception cref="LevelNotFoundException">Thrown when the levels folder could not be found.</exception>
    /// 
    public static async Task<PyruxLevel> LoadLevel(bool isBuiltIn, string levelName)
    {
        AppdataManagement.CheckAppdata();

        StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
        StorageFolder levelsFolder = ((StorageFolder)await appdataFolder.TryGetItemAsync("Levels"));

        StorageFolder levelsFolderOrganization;
        if (isBuiltIn)
        {
            levelsFolderOrganization = ((StorageFolder)await levelsFolder.TryGetItemAsync("Builtins"));
        }
        else
        {
            levelsFolderOrganization = ((StorageFolder)await levelsFolder.TryGetItemAsync("UserCreated"));
        }
        StorageFolder levelFolder = (StorageFolder)await levelsFolderOrganization.TryGetItemAsync(levelName);
        if(levelFolder != null)
        {
            PyruxLevel level;
            if (File.Exists(Path.Combine(levelFolder.Path, "LevelData.json")))
            {
                
                using (StreamReader sr = new StreamReader(Path.Combine(levelFolder.Path, "LevelData.json")))
                {
                    string levelJson = sr.ReadToEnd();
                    try
                    {

                    level = JsonConvert.DeserializeObject<PyruxLevel>(levelJson);
                    
                    }
                    catch (JsonException)
                    {
                        throw new InvalidLevelJsonException($"The JSON retrieved from the level file was invalid and could not be diserialized properly.");
                    }
                }
                
            }
            else
            {
                string path =  Path.Combine(levelFolder.Path, "LevelData.json");
                throw new LevelJsonNotFoundException($"The path {path} could not be found.");
            }

            if(File.Exists(Path.Combine(levelFolder.Path, "LevelScript.py")))
            {
                using (StreamReader sr = new StreamReader(Path.Combine(levelFolder.Path, "LevelScript.py")))
                {
                    string levelScript = sr.ReadToEnd();
                    level.Script = levelScript;
                }
            }
            else
            {
                await levelFolder.CreateFileAsync("LevelScript.py");
                using (StreamWriter sw = new StreamWriter(Path.Combine(levelFolder.Path, "LevelScript.py")))
                {
                    sw.Write(level.Script);
                }
            }
            return level;
        }
        else
        {
            throw new LevelNotFoundException($"The folder {levelName} could not be found in {levelsFolderOrganization.Path}.");
        }
    }    
}