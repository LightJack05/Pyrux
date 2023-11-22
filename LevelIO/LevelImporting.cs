using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pyrux.LevelIO;

public static class LevelImporting
{
    /// <summary>
    /// Import a level form storage.
    /// </summary>
    public static async Task ImportLevel()
    {
        string storageFile = await GetFilePathAsync();
        if (!File.Exists(storageFile))
        {
            return;
        }
        string levelJson = ReadLevelData(storageFile);
        PyruxLevel level = ConstructPyruxLevel(levelJson);
        if (level == null)
        {
            return;
        }
        LevelSaving.Save(level);

    }
    /// <summary>
    /// Construct a Pyrux level from given JSON.
    /// </summary>
    /// <param name="levelJson">The JSON data to construct the level from.</param>
    /// <returns>An instance of a Pyrux level constructed from the JSON. If unable to construct an Instance, returns null.</returns>
    private static PyruxLevel ConstructPyruxLevel(string levelJson)
    {
        try
        {
            PyruxLevel level = JsonConvert.DeserializeObject<PyruxLevel>(levelJson);
            return level;
        }
        catch (JsonException) { }

        return null;
    }
    /// <summary>
    /// Read level data from a storage file.
    /// </summary>
    /// <param name="storageFile">File to read data from.</param>
    /// <returns>The level data as JSON</returns>
    private static string ReadLevelData(string storageFile)
    {
        try
        {
            using (StreamReader sr = new(storageFile))
            {
                string levelJson = sr.ReadToEnd();
                return levelJson;
            }
        }
        catch
        {

        }
        return "";


    }
    /// <summary>
    /// Get the file to read data from.
    /// </summary>
    private static async Task<string> GetFilePathAsync()
    {
        Windows.Storage.Pickers.FileOpenPicker fileOpenPicker = new();
        fileOpenPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
        fileOpenPicker.FileTypeFilter.Add(".prxlvl");

        // Get the current window's HWND by passing in the Window object
        nint hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.Instance);

        // Associate the HWND with the file picker
        WinRT.Interop.InitializeWithWindow.Initialize(fileOpenPicker, hwnd);
        var file = (await fileOpenPicker.PickSingleFileAsync());

        string saveFile = String.Empty;
        if (file != null)
        {
             saveFile = file.Path;
        }
        
        if (File.Exists(saveFile))
        {
            return saveFile;
        }
        else
        {
            return null;
        }
    }
}