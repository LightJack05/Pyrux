// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Pyrux.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.TargetedContent;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
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
            else if(args.InvokedItemContainer!= null)
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
        private void NavViewNavigate(string navItemTag, Microsoft.UI.Xaml.Media.Animation.NavigationTransitionInfo transitionInfo)
        {
            Type page = null;
            if(navItemTag == "settings")
            {
                page = typeof(SettingsPage);
            }
            else
            {
                var item = contentDictionary.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                page = item.Page;
            }

            var preNavPageType = ctfMain.CurrentSourcePageType;
            if(!(page is null) && !Type.Equals(preNavPageType, page))
            {
                ctfMain.Navigate(page, null, transitionInfo);
            }
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            LevelIO.AppdataManagement.CheckAppdata();
        }

        
    }
}
