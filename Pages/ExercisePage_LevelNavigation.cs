namespace Pyrux.Pages
{
    public sealed partial class ExercisePage
    {

        public void NavigateToNextLevel()
        {

            if (StaticDataStore.ActiveLevel.IsBuiltIn)
            {
                int levelIndex = StaticDataStore.BuiltInLevels.FindIndex(x => x.Equals(StaticDataStore.ActiveLevel));
                int nextLevelIndex = levelIndex + 1;
                if (StaticDataStore.BuiltInLevels.Count - 1 >= nextLevelIndex)
                {
                    StaticDataStore.ActiveLevel = StaticDataStore.BuiltInLevels[nextLevelIndex];
                    StaticDataStore.OriginalActiveLevelMapLayout = StaticDataStore.ActiveLevel.MapLayout.Copy();
                }
            }
            else
            {
                int levelIndex = StaticDataStore.UserCreatedLevels.FindIndex(x => x.LevelName.Equals(StaticDataStore.ActiveLevel.LevelName));
                int nextLevelIndex = levelIndex + 1;
                if (StaticDataStore.UserCreatedLevels.Count - 1 >= nextLevelIndex)
                {
                    StaticDataStore.ActiveLevel = StaticDataStore.UserCreatedLevels[nextLevelIndex];
                    StaticDataStore.OriginalActiveLevelMapLayout = StaticDataStore.ActiveLevel.MapLayout.Copy();
                }
            }
            MainWindow.Instance.NavViewNavigate("levelSelect", new Microsoft.UI.Xaml.Media.Animation.CommonNavigationTransitionInfo());
            MainWindow.Instance.NavViewNavigate("exerciseView", new Microsoft.UI.Xaml.Media.Animation.CommonNavigationTransitionInfo());
            MainWindow.Instance.NavViewSetSelection(1);
        }

        public void NavigateToLevelSelection()
        {
            MainWindow.Instance.NavViewNavigate("levelSelect", new Microsoft.UI.Xaml.Media.Animation.CommonNavigationTransitionInfo());
            MainWindow.Instance.NavViewSetSelection(0);
        }

    }
}
