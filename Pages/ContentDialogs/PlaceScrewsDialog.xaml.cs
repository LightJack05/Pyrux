// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages.ContentDialogs
{

    /// <summary>
    /// Provides the dialogue for placing chips.
    /// </summary>
    public sealed partial class PlaceChipsDialog : Page
    {
        public static int ChipNumber = 0;
        internal static PositionVector2 Position;
        public PlaceChipsDialog()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(MainWindow.Instance.NavViewSelectedIndex == 2)
            {
                ChipNumber = StaticDataStore.ActiveLevel.GoalMapLayout.GetChipNumberAtPosition(Position);
            }
            else
            {
                ChipNumber = StaticDataStore.ActiveLevel.MapLayout.GetChipNumberAtPosition(Position);
            }
            nbxChipAmount.Value = ChipNumber;
        }

        private void nbxChipAmount_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            ChipNumber = (((int)nbxChipAmount.Value) >= 0) ? (int)nbxChipAmount.Value : 0;
        }
    }
}
