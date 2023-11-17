// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages.SettingsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExecutionSettings : Page
    {
        public Dictionary<string, int> DelayStringDictionary { get; set; } = new Dictionary<string, int>()
        {
            {"0.1s", 100 },
            {"0.2s", 200 },
            {"0.5s", 500 },
            {"1s", 1000 },
            {"2s", 2000 },
            {"3s", 3000 },
            {"5s", 5000 },
            {"7s", 7000 },
            {"10s", 10000 }
        };

        public ExecutionSettings()
        {
            int delayBeforeAutoReset = PyruxSettings.DelayBeforeAutoReset;
            this.InitializeComponent();
            PyruxSettings.DelayBeforeAutoReset = delayBeforeAutoReset;
            //sldDelayBeforeReset.Value = delayBeforeAutoReset;
            string ComboBoxItemString = DelayStringDictionary.FirstOrDefault((item) => item.Value == delayBeforeAutoReset).Key;
            cbxResetDelay.SelectedItem = cbxResetDelay.Items.FirstOrDefault((item) => (string)((ComboBoxItem)item).Content == ComboBoxItemString);
            UpdateSettingsDisplay();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSettingsDisplay();
        }

        private void tswAutoReset_Toggled(object sender, RoutedEventArgs e)
        {
            PyruxSettings.AutoRestartOnFinish = tswAutoReset.IsOn;
            UpdateSettingsDisplay();

        }
        private void UpdateSettingsDisplay()
        {
            tswAutoReset.IsOn = PyruxSettings.AutoRestartOnFinish;
            tswAddDelay.IsOn = PyruxSettings.AddDelayBeforeAutoReset;
            tswAddDelay.IsEnabled = PyruxSettings.AutoRestartOnFinish;
            cbxResetDelay.IsEnabled = PyruxSettings.AutoRestartOnFinish && PyruxSettings.AddDelayBeforeAutoReset;
            //sldDelayBeforeReset.IsEnabled = PyruxSettings.AutoRestartOnFinish && PyruxSettings.AddDelayBeforeAutoReset;
            PyruxSettings.SaveSettings();
        }

        //private void sldDelayBeforeReset_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        //{
        //    //PyruxSettings.DelayBeforeAutoReset = (int)sldDelayBeforeReset.Value;
        //    UpdateSettingsDisplay();
        //}

        private void tswAddDelay_Toggled(object sender, RoutedEventArgs e)
        {
            PyruxSettings.AddDelayBeforeAutoReset = tswAddDelay.IsOn;
            UpdateSettingsDisplay();
        }

        private void cbxResetDelay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PyruxSettings.DelayBeforeAutoReset = DelayStringDictionary[(string)((ComboBoxItem)cbxResetDelay.SelectedItem).Content];
        }
    }
}
