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
        /// <summary>
        /// Constructor for importing a json file into the settings.
        /// Creates a new instance for the PyruxSettings class, that should be set as the Instance variable.
        /// </summary>
        /// <param name="executionSpeed">Speed of execution between instructions.</param>
        [JsonConstructor]
        public PyruxSettings(int executionSpeed) 
        { 
            _executionSpeed = executionSpeed;
        }
        /// <summary>
        /// The current instance of the settings. This saves the actual data.
        /// </summary>
        public static PyruxSettings Instance { get; set; }
        /// <summary>
        /// The delay between instruction execution in milliseconds.
        /// Retrieved from ExecutionSpeed.
        /// </summary>
        public static int ExecutionDelayInMilliseconds { get => 1001 - ExecutionSpeed; }
        /// <summary>
        /// Execution speed for individual Instructions.
        /// </summary>
        public static int ExecutionSpeed { get => Instance._executionSpeed; set => Instance._executionSpeed = value; }
        /// <summary>
        /// Storage for the execution speed in the instance variable.
        /// </summary>
        public int _executionSpeed { get; set; } = 200;
        /// <summary>
        /// Saves the settings into the Appdata file.
        /// </summary>
        public async static void SaveSettings()
        {
            string settingsJson = JsonConvert.SerializeObject(Instance);

            StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
            if (appdataFolder != null)
            {
                try
                {
                    // Create file if it doesn't exist yet
                    if (File.Exists(Path.Combine(appdataFolder.Path, "settings.json")))
                    {
                        await appdataFolder.CreateFileAsync("settings.json");
                    }
                    // Write the settings to the storage file
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
