// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using Pyrux.Pages.SettingsPages;

namespace Pyrux.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public List<(string Tag, Type Page)> contentDictionary = new()
        {
            ("execution",typeof(ExecutionSettings)),
            ("tutorial", typeof(TutorialSettings)),
            ("devtools",typeof(DevToolsPage))
        };

        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void ngvSettings_Loaded(object sender, RoutedEventArgs e)
        {
            ngvSettings.SelectedItem = ngvSettings.MenuItems[0];
            NavViewNavigate("execution", new Microsoft.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo());
        }

        private void ngvSettings_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer != null)
            {
                string navItemTag = args.InvokedItemContainer.Tag.ToString();
                NavViewNavigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        /// <summary>
        /// Navigate the content frame to the selected page.
        /// </summary>
        /// <param name="navItemTag">Tag of the selected page.</param>
        /// <param name="transitionInfo">Transitioninfo for transition animation.</param>
        public void NavViewNavigate(string navItemTag, Microsoft.UI.Xaml.Media.Animation.NavigationTransitionInfo transitionInfo)
        {
            Type page = null;

            (string Tag, Type Page) item = contentDictionary.FirstOrDefault(p => p.Tag.Equals(navItemTag));
            page = item.Page;


            Type preNavPageType = ctfSettings.CurrentSourcePageType;
            if (!(page is null) && !Type.Equals(preNavPageType, page))
            {
                ctfSettings.Navigate(page, null, transitionInfo);
            }
        }
    }
}
