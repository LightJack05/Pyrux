// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.




// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using Microsoft.Windows.AppLifecycle;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

namespace Pyrux
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            //If the app is started by a level file, import that file.
            AppActivationArguments appActivationArguments = AppInstance.GetCurrent().GetActivatedEventArgs();

            if (appActivationArguments.Kind is ExtendedActivationKind.File &&
                appActivationArguments.Data is IFileActivatedEventArgs fileActivatedEventArgs &&
                fileActivatedEventArgs.Files.FirstOrDefault() is IStorageFile storageFile
                )
            {
                using (StreamReader sr = new(storageFile.Path))
                {
                    string file = sr.ReadToEnd();
                    try
                    {
                        PyruxLevel level = JsonConvert.DeserializeObject<PyruxLevel>(file);
                        //TODO: Actually import the level.
                    }
                    catch (JsonException)
                    {

                    }
                }
            }
            //Load the current settings from the appdata folder.
            StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
            if (appdataFolder != null)
            {
                try
                {
                    StorageFile settingsFile = await appdataFolder.GetFileAsync("settings.json");
                    using (StreamReader sr = new(settingsFile.Path))
                    {
                        string fileContent = sr.ReadToEnd();
                        DataManagement.PyruxSettings.Instance = JsonConvert.DeserializeObject<PyruxSettings>(fileContent);
                    }
                }
                catch
                {
                    DataManagement.PyruxSettings.Instance = new PyruxSettings(200);
                }
            }
            else
            {
                DataManagement.PyruxSettings.Instance = new PyruxSettings(200);
            }


            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window;



    }
}
