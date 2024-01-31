// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using CommunityToolkit.WinUI.UI;
using Pyrux.DataManagement.Restrictions;
using System.Collections.ObjectModel;
using Windows.UI;

namespace Pyrux.Pages.ContentDialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LevelFailedDueToRestrictionsDialog : Page
    {
        private ObservableCollection<Restriction> _failedRestrictions = new();
        public LevelFailedDueToRestrictionsDialog()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _failedRestrictions.Clear();
            foreach (Restriction restriction in StaticDataStore.ActiveLevel.CompletionRestrictions)
            {
                if (!restriction.IsSatisfied(StaticDataStore.ActiveLevel))
                {
                    _failedRestrictions.Add(restriction);
                }
            }
        }

        private void irpRestrictions_ElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
        {

            if (args.Element is FrameworkElement element)
            {
                element.DataContext = _failedRestrictions[args.Index];
                Restriction restriction = element.DataContext as Restriction;

                TextBlock textBlock = ((TextBlock)element.FindChildren().ToList()[1]);
                textBlock.Text = ((Restriction)element.DataContext).ToString();
                SymbolIcon symbolIcon = ((SymbolIcon)element.FindChildren().ToList()[0]);
                symbolIcon.Symbol = restriction.IsSatisfied(StaticDataStore.ActiveLevel) ? Symbol.Accept : Symbol.Cancel;
                symbolIcon.Foreground = restriction.IsSatisfied(StaticDataStore.ActiveLevel) ? new SolidColorBrush(Color.FromArgb(255, 0, 255, 0)) : new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                ToolTipService.SetToolTip(symbolIcon, restriction.IsSatisfied(StaticDataStore.ActiveLevel) ? "Satisfied" : "Not satisfied");
                ToolTipService.SetToolTip(symbolIcon, "Not Satisfied");
            }
        }
    }
}
