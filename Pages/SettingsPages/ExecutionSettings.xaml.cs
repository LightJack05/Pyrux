using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages.SettingsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExecutionSettings : Page
    {
        public ExecutionSettings()
        {
            int delayBeforeAutoReset = PyruxSettings.DelayBeforeAutoReset;
            this.InitializeComponent();
            PyruxSettings.DelayBeforeAutoReset = delayBeforeAutoReset;
            sldDelayBeforeReset.Value = delayBeforeAutoReset;
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
            tswAddDelay.IsEnabled = PyruxSettings.AutoRestartOnFinish;
            sldDelayBeforeReset.IsEnabled = PyruxSettings.AutoRestartOnFinish && PyruxSettings.AddDelayBeforeAutoReset;
            PyruxSettings.SaveSettings();
        }

        private void sldDelayBeforeReset_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            PyruxSettings.DelayBeforeAutoReset = (int)sldDelayBeforeReset.Value;
            UpdateSettingsDisplay();
        }

        private void tswAddDelay_Toggled(object sender, RoutedEventArgs e)
        {
            PyruxSettings.AddDelayBeforeAutoReset = tswAddDelay.IsOn;
            UpdateSettingsDisplay();
        }
    }
}
