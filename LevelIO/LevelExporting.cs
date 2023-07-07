using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using System.IO.Compression;
using Windows.Storage;

namespace Pyrux.LevelIO
{
    internal class LevelExporting
    {
        public static async void ExportProcess(PyruxLevel exportingLevel)
        {
            Windows.Storage.StorageFile saveFile = await GetSavePathAsync(exportingLevel.LevelName);
            if (saveFile != null)
            {
                SaveData(JsonConvert.SerializeObject(exportingLevel), saveFile);
            }

        }

        public static void SaveData(string levelJson, StorageFile storageFile)
        {
            if (File.Exists(storageFile.Path))
            {
                File.Delete(storageFile.Path);
            }
            using (StreamWriter sw = new(storageFile.Path))
            {
                sw.Write(levelJson);
            }

        }

        public static async Task<Windows.Storage.StorageFile> GetSavePathAsync(string levelName)
        {
            Windows.Storage.Pickers.FileSavePicker fileSavePicker = new();
            fileSavePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            fileSavePicker.FileTypeChoices.Add("Pyrux Level", new List<string>() { ".prxlvl" });
            fileSavePicker.SuggestedFileName = levelName;

            // Get the current window's HWND by passing in the Window object
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.Instance);

            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(fileSavePicker, hwnd);

            Windows.Storage.StorageFile saveFile = await fileSavePicker.PickSaveFileAsync();
            return saveFile;
        }
    }
}
