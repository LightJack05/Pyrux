using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pyrux.LevelIO;

public static class LevelImporting
{
    /// <summary>
    /// Import a level form storage.
    /// </summary>
    public static async Task ImportLevel()
    {
        Windows.Storage.StorageFile storageFile = await GetFilePathAsync();
        if(storageFile == null)
        {
            return;
        }
        string levelJson = ReadLevelData(storageFile);
        PyruxLevel level = ConstructPyruxLevel(levelJson);
        if(level == null)
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
    private static string ReadLevelData(Windows.Storage.StorageFile storageFile)
    {
        try
        {
            using (StreamReader sr = new(storageFile.Path))
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
    private static async Task<Windows.Storage.StorageFile> GetFilePathAsync()
    {

        Windows.Storage.Pickers.FileOpenPicker fileOpenPicker = new();
        fileOpenPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
        fileOpenPicker.FileTypeFilter.Add(".prxlvl");

        // Get the current window's HWND by passing in the Window object
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.Instance);

        // Associate the HWND with the file picker
        WinRT.Interop.InitializeWithWindow.Initialize(fileOpenPicker, hwnd);

        Windows.Storage.StorageFile saveFile = await fileOpenPicker.PickSingleFileAsync();
        if(saveFile != null)
        {

        return saveFile;
        }
        else
        {
            return null;
        }
    }
}