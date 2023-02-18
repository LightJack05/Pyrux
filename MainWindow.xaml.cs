// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.



// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static MainWindow Instance;
        /// <summary>
        /// List of pages in the navigation menu.
        /// </summary>
        public List<(string Tag, Type Page)> contentDictionary = new()
        {
            ("levelSelect",typeof(LevelSelectPage)),
            ("exerciseView",typeof(ExercisePage)),
            ("hint",typeof(HintPage)),
            ("docs",typeof(DocsPage)),
            ("about",typeof(AboutPage))
        };
        public MainWindow()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            Instance = this;
        }

        private void ngvMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ngvMainWindow.SelectedItem = ngvMainWindow.MenuItems[0];
            NavViewNavigate("levelSelect", new Microsoft.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo());

        }

        /// <summary>
        /// Run when an item is invoked on the navigationview.
        /// Switch the content frame to the page selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ngvMainWindow_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavViewNavigate("settings", args.RecommendedNavigationTransitionInfo);
            }
            else if (args.InvokedItemContainer != null)
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
            if (navItemTag == "settings")
            {
                page = typeof(SettingsPage);
            }
            else
            {
                var item = contentDictionary.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                page = item.Page;
            }

            var preNavPageType = ctfMain.CurrentSourcePageType;
            if (!(page is null) && !Type.Equals(preNavPageType, page))
            {
                ctfMain.Navigate(page, null, transitionInfo);
            }
        }

        public void NavViewSetSelection(int index)
        {
            ngvMainWindow.SelectedItem = ngvMainWindow.MenuItems[index];
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            LevelIO.AppdataManagement.CheckAppdata();
        }


    }
}
