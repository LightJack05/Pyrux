using IronPython.Runtime.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Pyrux.DataManagement
{
    public class PyruxSettings
    {
        [JsonConstructor]
        public PyruxSettings(int executionSpeed) 
        { 
            _executionSpeed = executionSpeed;
        }
        public static PyruxSettings Instance { get; set; }
        public static int ExecutionDelayInMilliseconds { get => 1001 - ExecutionSpeed; }
        public static int ExecutionSpeed { get => Instance._executionSpeed; set => Instance._executionSpeed = value; }
        public int _executionSpeed { get; set; } = 200;

        public async static void SaveSettings()
        {
            string settingsJson = JsonConvert.SerializeObject(Instance);

            StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
            if (appdataFolder != null)
            {
                try
                {
                    try
                    {
                        await appdataFolder.CreateFileAsync("settings.json");
                    }
                    catch { }
                    StorageFile settingsFile = await appdataFolder.GetFileAsync("settings.json");
                    using (StreamWriter sw = new(settingsFile.Path))
                    {
                        sw.Write(settingsJson);
                    }
                }
                catch
                {
                    
                }
            }
        }
    }
}
