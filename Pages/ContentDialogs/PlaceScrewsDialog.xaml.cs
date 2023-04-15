// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages.ContentDialogs
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlaceScrewsDialog : Page
    {
        public static int ScrewNumber = 0;
        internal static PositionVector2 Position;
        public PlaceScrewsDialog()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ScrewNumber = StaticDataStore.ActiveLevel.MapLayout.GetScrewNumberAtPosition(Position);
            nbxScrewAmount.Value = ScrewNumber;
        }

        private void nbxScrewAmount_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            ScrewNumber = (int)nbxScrewAmount.Value;
        }
    }
}
