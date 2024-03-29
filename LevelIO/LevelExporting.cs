﻿using System.Threading.Tasks;

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
            string saveFile = await GetSavePathAsync(exportingLevel.LevelName);

            SaveData(JsonConvert.SerializeObject(exportingLevel), saveFile);


        }
        /// <summary>
        /// Save the level decoded to json to a file on the disk.
        /// </summary>
        /// <param name="levelJson">The JSON representation of a pyrux level that should be saved.</param>
        /// <param name="storageFile">The storage file to write the file to.</param>
        public static void SaveData(string levelJson, string storageFile)
        {
            if (File.Exists(storageFile))
            {
                File.Delete(storageFile);
            }
            using (StreamWriter sw = new(storageFile))
            {
                sw.Write(levelJson);
            }

        }
        /// <summary>
        /// Get the path a level should be saved to.
        /// </summary>
        /// <param name="levelName">The name of the saved level. This is the default filename.</param>
        /// <returns>A storage file to save the data to.</returns>
        private static async Task<string> GetSavePathAsync(string levelName)
        {
            Windows.Storage.Pickers.FileSavePicker fileSavePicker = new();
            fileSavePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            fileSavePicker.FileTypeChoices.Add("Pyrux Level", new List<string>() { ".prxlvl" });
            fileSavePicker.SuggestedFileName = levelName;

            // Get the current window's HWND by passing in the Window object
            nint windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.Instance);

            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(fileSavePicker, windowHandle);

            //TODO: handle no file selected
            string saveFile = (await fileSavePicker.PickSaveFileAsync()).Path;
            return saveFile;
        }
    }
}
