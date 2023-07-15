using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using System.IO.Compression;
using Windows.Storage;

namespace Pyrux.LevelIO
{
    /// <summary>
    /// Contains methods used for exporting levels to a file.
    /// </summary>
    internal class LevelExporting
    {
        /// <summary>
        /// Export the level to a file. This will also ask the user for a location.
        /// </summary>
        /// <param name="exportingLevel">Level to export</param>
        public static async void ExportProcess(PyruxLevel exportingLevel)
        {
            Windows.Storage.StorageFile saveFile = await GetSavePathAsync(exportingLevel.LevelName);
            if (saveFile != null)
            {
                SaveData(JsonConvert.SerializeObject(exportingLevel), saveFile);
            }

        }
        /// <summary>
        /// Save the level decoded to json to a file on the disk.
        /// </summary>
        /// <param name="levelJson">The JSON representation of a pyrux level that should be saved.</param>
        /// <param name="storageFile">The storage file to write the file to.</param>
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
        /// <summary>
        /// Get the path a level should be saved to.
        /// </summary>
        /// <param name="levelName">The name of the saved level. This is the default filename.</param>
        /// <returns>A storage file to save the data to.</returns>
        private static async Task<Windows.Storage.StorageFile> GetSavePathAsync(string levelName)
        {
            Windows.Storage.Pickers.FileSavePicker fileSavePicker = new();
            fileSavePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            fileSavePicker.FileTypeChoices.Add("Pyrux Level", new List<string>() { ".prxlvl" });
            fileSavePicker.SuggestedFileName = levelName;

            // Get the current window's HWND by passing in the Window object
            var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.Instance);

            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(fileSavePicker, windowHandle);

            Windows.Storage.StorageFile saveFile = await fileSavePicker.PickSaveFileAsync();
            return saveFile;
        }
    }
}
