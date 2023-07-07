using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pyrux.LevelIO;

public static class LevelImporting
{
    public static async Task ImportLevel()
    {
        Windows.Storage.StorageFile storageFile = await GetFilePathAsync();
        string levelJson = ReadLevelData(storageFile);
        PyruxLevel level = ConstructPyruxLevel(levelJson);
        LevelSaving.Save(level);

    }
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
        return saveFile;
    }
}