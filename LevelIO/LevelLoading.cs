using Pyrux.DataManagement;
using System.Threading.Tasks;
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
        if (levelFolder != null)
        {
            PyruxLevel level;
            if (File.Exists(Path.Combine(levelFolder.Path, "LevelData.json")))
            {

                using (StreamReader sr = new(Path.Combine(levelFolder.Path, "LevelData.json")))
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
                string path = Path.Combine(levelFolder.Path, "LevelData.json");
                throw new LevelJsonNotFoundException($"The path {path} could not be found.");
            }

            if (File.Exists(Path.Combine(levelFolder.Path, "LevelScript.py")))
            {
                using (StreamReader sr = new(Path.Combine(levelFolder.Path, "LevelScript.py")))
                {
                    string levelScript = sr.ReadToEnd();
                    level.Script = levelScript;
                }
            }
            else
            {
                await levelFolder.CreateFileAsync("LevelScript.py");
                using (StreamWriter sw = new(Path.Combine(levelFolder.Path, "LevelScript.py")))
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
    public static async Task<PyruxLevel> LoadLevel(StorageFolder levelFolder)
    {
        
        PyruxLevel level;

        if (File.Exists(Path.Combine(levelFolder.Path, "LevelData.json")))
        {
            using (StreamReader sr = new(Path.Combine(levelFolder.Path, "LevelData.json")))
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
            string path = Path.Combine(levelFolder.Path, "LevelData.json");
            throw new LevelJsonNotFoundException($"The path {path} could not be found.");
        }

        if (File.Exists(Path.Combine(levelFolder.Path, "LevelScript.py")))
        {
            using (StreamReader sr = new(Path.Combine(levelFolder.Path, "LevelScript.py")))
            {
                string levelScript = sr.ReadToEnd();
                level.Script = levelScript;
            }
        }
        else
        {
            await levelFolder.CreateFileAsync("LevelScript.py");
            using (StreamWriter sw = new(Path.Combine(levelFolder.Path, "LevelScript.py")))
            {
                sw.Write(level.Script);
            }
        }
        return level;
        
        
    }

    /// <summary>
    /// Retrieve all valid levels stored in the builtin levels folder.
    /// Will ignore invalid levels.
    /// </summary>
    /// <returns>A list of levels found in the builtin folder.</returns>
    public static async Task<List<PyruxLevel>> FindBuiltInLevels()
    {
        List<PyruxLevel> levels = new();
        AppdataManagement.CheckAppdata();
        StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
        StorageFolder levelsFolder = ((StorageFolder)await appdataFolder.TryGetItemAsync("Levels"));
        StorageFolder builtInsFolder = ((StorageFolder)await levelsFolder.TryGetItemAsync("Builtins"));

        levels = await FindLevelsIn(builtInsFolder);

        return levels;
    }
    /// <summary>
    /// Retrieve all valid levels stored in the user created levels folder.
    /// Will ignore invalid levels.
    /// </summary>
    /// <returns>A list of levels found in the builtin folder.</returns>
    public static async Task<List<PyruxLevel>> FindUserCreatedLevels()
    {
        List<PyruxLevel> levels = new();
        AppdataManagement.CheckAppdata();
        StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
        StorageFolder levelsFolder = ((StorageFolder)await appdataFolder.TryGetItemAsync("Levels"));
        StorageFolder userCreatedFolder = ((StorageFolder)await levelsFolder.TryGetItemAsync("UserCreated"));

        levels = await FindLevelsIn(userCreatedFolder);

        return levels;
    }

    /// <summary>
    /// Find all levels in the given StorageFolder.
    /// </summary>
    /// <param name="levelOrganizationFolder">The folder to search.</param>
    /// <returns>A list of levels in the given folder.</returns>
    public static async Task<List<PyruxLevel>> FindLevelsIn(StorageFolder levelOrganizationFolder)
    {
        List<PyruxLevel> levels = new();
        AppdataManagement.CheckAppdata();

        var levelFolders = await levelOrganizationFolder.GetFoldersAsync();

        foreach (StorageFolder levelFolder in levelFolders)
        {
            PyruxLevel level;
            try
            {
                level = await LoadLevel(levelFolder);
                levels.Add(level);
            }
            catch (LevelJsonNotFoundException)
            {

            }
            catch (InvalidLevelJsonException)
            {

            }
        }

        return levels;
    }
}