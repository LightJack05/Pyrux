// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using Pyrux.LevelIO;
using System.Threading.Tasks;

namespace Pyrux
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public string TitleText
        {
            get => "[PRE-ALPHA] Pyrux v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static MainWindow Instance;
        /// <summary>
        /// List of pages in the navigation menu.
        /// </summary>
        public List<(string Tag, Type Page)> contentDictionary = new()
        {
            ("levelSelect",typeof(LevelSelectPage)),
            ("exerciseView",typeof(ExercisePage)),
            ("goalView",typeof(GoalPageView)),
            ("hint",typeof(HintPage)),
            ("docs",typeof(DocsPage)),
            ("about",typeof(AboutPage)),
            ("devtools",typeof(DevToolsPage))
        };
        public MainWindow()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            Instance = this;
        }
        /// <summary>
        /// Initialize the basic layout of the navigation view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ngvMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await CheckAppdataIntegrity();
            if (!AppdataManagement.AppdataCorrupted)
            {
                ngvMainWindow.SelectedItem = ngvMainWindow.MenuItems[0];
                NavViewNavigate("levelSelect", new Microsoft.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo());
            }
            else
            {
                DisplayAppdataError();
            }
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
            if (ExercisePage.Instance != null)
            {
                ExercisePage.Instance.CancelScriptExecution();
                ExercisePage.Instance.NavigationLayoutReset();
            }

            Type page = null;
            if (navItemTag == "settings")
            {
                page = typeof(SettingsPage);
            }
            else
            {
                (string Tag, Type Page) item = contentDictionary.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                page = item.Page;
            }

            Type preNavPageType = ctfMain.CurrentSourcePageType;
            if (!(page is null) && !Type.Equals(preNavPageType, page))
            {
                ctfMain.Navigate(page, null, transitionInfo);
            }
        }
        /// <summary>
        /// Set the navigation view selection to the given index.
        /// </summary>
        /// <param name="index">Index to move the selection to.</param>
        public void NavViewSetSelection(int index)
        {
            ngvMainWindow.SelectedItem = ngvMainWindow.MenuItems[index];
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
        }
        /// <summary>
        /// Run a check to see if the appdata folder is still intact.
        /// </summary>
        private async Task CheckAppdataIntegrity()
        {
            await AppdataManagement.CheckAppdata();
        }
        /// <summary>
        /// Set the navigation view enabled or disabled.
        /// </summary>
        /// <param name="enabled">Enable or disable the nav view.</param>
        public void NavViewSetEnabled(bool enabled)
        {
            ngvMainWindow.IsEnabled = enabled;
        }
        /// <summary>
        /// Display an error indicating that the appdata folder has been corrupted.
        /// </summary>
        public async void DisplayAppdataError()
        {
            if (AppdataManagement.AppdataCorrupted)
            {
                ContentDialog appdataErrorDialogue = new();
                appdataErrorDialogue.XamlRoot = this.Content.XamlRoot;
                appdataErrorDialogue.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                appdataErrorDialogue.Title = "Appdata corrupted.";
                appdataErrorDialogue.PrimaryButtonText = "RESET";
                appdataErrorDialogue.SecondaryButtonText = "Exit";
                appdataErrorDialogue.DefaultButton = ContentDialogButton.Primary;
                appdataErrorDialogue.Content = new Pages.ContentDialogs.ConfirmRepairAppdataFolderDialogue();

                ContentDialogResult result = await appdataErrorDialogue.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    await AppdataManagement.ResetAppdata();
                    ngvMainWindow_Loaded(null, null);
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

    }
}
