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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages.DocsPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class DocsPageWrapper : Page
    {
        /// <summary>
        /// List of pages in the navigation menu.
        /// </summary>
        public List<(string Tag, Type Page)> contentDictionary = new()
        {
            ("movement",typeof(MovementDocsPage)),
            ("chips",typeof(ChipsDocsPage)),
            ("python",typeof(PythonDocsPage)),
        };

        public DocsPageWrapper()
        {
            this.InitializeComponent();
        }
        private void ngvDocs_Loaded(object sender, RoutedEventArgs e)
        {
            ngvDocs.SelectedItem = ngvDocs.MenuItems[0];
            NavViewNavigate("movement", new Microsoft.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo());
        }

        private void ngvDocs_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
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


            Type preNavPageType = ctfDocs.CurrentSourcePageType;
            if (!(page is null) && !Type.Equals(preNavPageType, page))
            {
                ctfDocs.Navigate(page, null, transitionInfo);
            }
        }
    }
}
