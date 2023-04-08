// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        public string Version
        {
            get => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public string AboutText
        {
            get => """
                Pyrux is created by LightJack05.

                [Copyright owned by LightJack05. Do not distribute. Pre-Production Version, to be considered confidential.]

                """;
        }
        public AboutPage()
        {
            this.InitializeComponent();
        }

        
    }
}
