using System.IO;
using Newtonsoft.Json;
using Pyrux.DataManagement;
using Pyrux.Pages;

namespace Pyrux.LevelIO;


internal static class LevelLoading
{
    public static void LoadLevel(bool isBuiltIn, string levelName)
    {
        foreach (char character in Path.GetInvalidFileNameChars())
        {
            levelName = levelName.Replace(character, '_');
        }

        if (isBuiltIn)
        {
            string filePath = Path.Combine(Environment.SpecialFolder.ApplicationData.ToString(), "Pyrux", "builtin", $"{levelName}.json");
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string levelJson = sr.ReadToEnd();
                    PyruxLevel level = JsonConvert.DeserializeObject<PyruxLevel>(levelJson);
                }
            }
            else
            {
                throw new FileNotFoundException($"This file was not found: {filePath}");
            }
        }
    }    
}