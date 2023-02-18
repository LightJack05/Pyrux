// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Pyrux.DataManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LevelSelectPage : Page
    {
        public LevelSelectPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLevelIntoStaticStorage(true, "TestlevelModified");
        }
        async void LoadLevelIntoStaticStorage(bool isBuiltIn, string levelName)
        {
            PyruxLevel level = await LevelIO.LevelLoading.LoadLevel(isBuiltIn, levelName);
            DataManagement.StaticDataStore.ActiveLevel = level.Copy();
        }
    }
}
