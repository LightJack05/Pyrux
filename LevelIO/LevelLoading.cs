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
        await AppdataManagement.CheckAppdata();

        string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pyrux");
        string levelsFolder = Path.Combine(appdataFolder, "Levels");

        string levelsFolderOrganization;
        if (isBuiltIn)
        {
            levelsFolderOrganization = Path.Combine(levelsFolder,"Builtins");
        }
        else
        {
            levelsFolderOrganization = Path.Combine(levelsFolder, "UserCreated");
        }
        string levelFolder = Path.Combine(levelsFolderOrganization,levelName);
        if (Directory.Exists(levelFolder))
        {
            PyruxLevel level;
            if (File.Exists(Path.Combine(levelFolder, "LevelData.json")))
            {

                using (StreamReader sr = new(Path.Combine(levelFolder, "LevelData.json")))
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
                string path = Path.Combine(levelFolder, "LevelData.json");
                throw new LevelJsonNotFoundException($"The path {path} could not be found.");
            }

            if (File.Exists(Path.Combine(levelFolder, "LevelScript.py")))
            {
                using (StreamReader sr = new(Path.Combine(levelFolder, "LevelScript.py")))
                {
                    string levelScript = sr.ReadToEnd();
                    level.Script = levelScript;
                }
            }
            else
            {
                File.Create(Path.Combine(levelFolder,"LevelScript.py"));
                using (StreamWriter sw = new(Path.Combine(levelFolder, "LevelScript.py")))
                {
                    sw.Write(level.Script);
                }
            }
            return level;
        }
        else
        {
            throw new LevelNotFoundException($"The folder {levelName} could not be found in {levelsFolderOrganization}.");
        }
    }
    /// <summary>
    /// Create a Pyrux level from a folder in AppData.
    /// </summary>
    /// <param name="levelFolder">Folder of the level in the appdata folder.</param>
    /// <returns>An instance of the pyrux level.</returns>
    /// <exception cref="InvalidLevelJsonException">Thrown if the level JSON read is invalid.</exception>
    /// <exception cref="LevelJsonNotFoundException">Thrown if no json file was found.</exception>
    public static async Task<PyruxLevel> LoadLevel(string levelFolder)
    {

        PyruxLevel level;

        if (File.Exists(Path.Combine(levelFolder, "LevelData.json")))
        {
            using (StreamReader sr = new(Path.Combine(levelFolder, "LevelData.json")))
            {
                string levelJson = sr.ReadToEnd();
                try
                {

                    level = JsonConvert.DeserializeObject<PyruxLevel>(levelJson);

                }
                catch (JsonException)
                {
                    throw new InvalidLevelJsonException($"The JSON retrieved from the level file was invalid and could not be deserialized properly.");
                }
            }

        }
        else
        {
            string path = Path.Combine(levelFolder, "LevelData.json");
            throw new LevelJsonNotFoundException($"The path {path} could not be found.");
        }

        if (File.Exists(Path.Combine(levelFolder, "LevelScript.py")))
        {
            using (StreamReader sr = new(Path.Combine(levelFolder, "LevelScript.py")))
            {
                string levelScript = sr.ReadToEnd();
                level.Script = levelScript;
            }
        }
        else
        {
            File.Create(Path.Combine(levelFolder,"LevelScript.py"));
            using (StreamWriter sw = new(Path.Combine(levelFolder, "LevelScript.py")))
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
        await AppdataManagement.CheckAppdata();
        string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pyrux");

        string levelsFolder = Path.Combine(appdataFolder, "Levels");
        string builtInsFolder = Path.Combine(levelsFolder, "Builtins");

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
        await AppdataManagement.CheckAppdata();
        string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pyrux");
        string levelsFolder = Path.Combine(appdataFolder, "Levels");
        string userCreatedFolder = Path.Combine(levelsFolder, "UserCreated");

        levels = await FindLevelsIn(userCreatedFolder);
        return levels;
    }

    /// <summary>
    /// Find all levels in the given StorageFolder.
    /// </summary>
    /// <param name="levelOrganizationFolder">The folder to search.</param>
    /// <returns>A list of levels in the given folder.</returns>
    public static async Task<List<PyruxLevel>> FindLevelsIn(string levelOrganizationFolder)
    {
        List<PyruxLevel> levels = new();
        await AppdataManagement.CheckAppdata();

        string[] levelFolders = Directory.GetFileSystemEntries(levelOrganizationFolder);

        foreach (string levelFolder in levelFolders)
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