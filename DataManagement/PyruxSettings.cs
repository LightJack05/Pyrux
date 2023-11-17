namespace Pyrux.DataManagement
{
    public class PyruxSettings
    {
        public PyruxSettings()
        {

        }
        /// <summary>
        /// Constructor for importing a json file into the settings.
        /// Creates a new instance for the PyruxSettings class, that should be set as the Instance variable.
        /// </summary>
        /// <param name="executionSpeed">Speed of execution between instructions.</param>
        [JsonConstructor]
        public PyruxSettings(int executionSpeed, int resetDelayInMilliseconds, bool autoRestartOnFinish, bool addDelayToReset, bool skipTutorialEnabled, Keybinds keybinds)
        {
            _executionSpeed = executionSpeed;
            _delayBeforeAutoReset = resetDelayInMilliseconds;
            _autoRestartOnFinish = autoRestartOnFinish;
            _addDelayBeforeAutoReset = addDelayToReset;
            _skipTutorialEnabed = skipTutorialEnabled;
            _keybinds = keybinds;
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
        public static bool AutoRestartOnFinish { get => Instance._autoRestartOnFinish; set => Instance._autoRestartOnFinish = value; }
        public bool _autoRestartOnFinish { get; set; } = false;
        public static int DelayBeforeAutoReset { get => Instance._delayBeforeAutoReset; set => Instance._delayBeforeAutoReset = value; }
        public int _delayBeforeAutoReset { get; set; } = 100;
        public static bool AddDelayBeforeAutoReset { get => Instance._addDelayBeforeAutoReset; set => Instance._addDelayBeforeAutoReset = value; }
        public bool _addDelayBeforeAutoReset { get; set; } = false;
        public static bool SkipTutorialEnabled { get => Instance._skipTutorialEnabed; set => Instance._skipTutorialEnabed = value; }
        public bool _skipTutorialEnabed { get; set; } = false;
        public static int TutorialStateId { get => Instance._tutorialStateId; set => Instance._tutorialStateId = value; }
        public int _tutorialStateId { get; set; } = 0;
        public static Keybinds Keybinds { get => Instance._keybinds; set => Instance._keybinds = value; }
        public Keybinds _keybinds { get; set; } = new();

        /// <summary>
        /// Saves the settings into the Appdata file.
        /// </summary>
        public async static void SaveSettings()
        {
            string settingsJson = JsonConvert.SerializeObject(Instance);

            string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pyrux");

            if (Directory.Exists(appdataFolder))
            {
                try
                {
                    string settingsFile = Path.Combine(appdataFolder, "settings.json");
                    // Create file if it doesn't exist yet
                    if (!File.Exists(settingsFile))
                    {
                        File.Create(settingsFile);
                    }
                    using (StreamWriter sw = new(settingsFile))
                    {
                        sw.Write(settingsJson);
                    }
                }
                catch
                {

                }
            }
        }
        public static void LoadSettings()
        {
            string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pyrux");

            if (File.Exists(Path.Combine(appdataFolder, "settings.json")))
            {
                try
                {
                    string settingsFile = Path.Combine(appdataFolder, "settings.json");
                    using (StreamReader sr = new(settingsFile))
                    {
                        string fileContent = sr.ReadToEnd();
                        DataManagement.PyruxSettings.Instance = JsonConvert.DeserializeObject<PyruxSettings>(fileContent);
                        sr.Close();
                    }
                    if (Instance == null)
                    {
                        Instance = new PyruxSettings();
                    }
                    if (Instance._keybinds == null)
                    {
                        Instance._keybinds = new();
                    }
                }
                catch
                {
                    Instance = new PyruxSettings();
                }
            }
            else
            {
                Instance = new PyruxSettings();
            }
            SaveSettings();
        }
    }
}
