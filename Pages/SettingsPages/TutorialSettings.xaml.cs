// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages.SettingsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TutorialSettings : Page
    {
        public TutorialSettings()
        {
            this.InitializeComponent();
        }

        private void tswShowTutorial_Toggled(object sender, RoutedEventArgs e)
        {
            PyruxSettings.SkipTutorialEnabled = !tswShowTutorial.IsOn;
            if (!PyruxSettings.SkipTutorialEnabled)
            {
                PyruxSettings.TutorialStateId = 0;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tswShowTutorial.IsOn = !PyruxSettings.SkipTutorialEnabled;
        }
    }
}
