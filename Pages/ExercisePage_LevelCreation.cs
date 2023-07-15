
using Pyrux.Pages.ContentDialogs;

namespace Pyrux.Pages;

public sealed partial class ExercisePage
{
    /// <summary>
    /// Asks the user for parameters for a new level.
    /// </summary>
    private async void CreateNewLevel()
    {
        ContentDialog createlevelDialogue = new();
        createlevelDialogue.XamlRoot = this.Content.XamlRoot;
        createlevelDialogue.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        createlevelDialogue.Title = "Create new level";
        createlevelDialogue.PrimaryButtonText = "Create";
        createlevelDialogue.SecondaryButtonText = "Cancel";
        createlevelDialogue.DefaultButton = ContentDialogButton.Primary;
        createlevelDialogue.Content = new LevelCreationDialogue();

        ContentDialogResult result = await createlevelDialogue.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            LevelCreationDialogueFinished();
        }

    }
    /// <summary>
    /// Creates a new level with given data from the last LevelCreationDialogue.
    /// </summary>
    private async void LevelCreationDialogueFinished()
    {
        if (!((LevelCreationDialogue.LevelName == String.Empty) ||
            (LevelCreationDialogue.LevelName.Trim().Length == 0) ||
            StaticDataStore.UserCreatedLevels.Any<PyruxLevel>((x) => x.LevelName == LevelCreationDialogue.LevelName.Trim()) ||
            StaticDataStore.BuiltInLevels.Any<PyruxLevel>((x) => x.LevelName == LevelCreationDialogue.LevelName.Trim())))
        {
            ActiveLevel = new PyruxLevel(
            LevelCreationDialogue.LevelName.Trim(),
            "",
            false,
            new PyruxLevelMapLayout(
                new bool[LevelCreationDialogue.PlayingFieldSize, LevelCreationDialogue.PlayingFieldSize],
                new int[LevelCreationDialogue.PlayingFieldSize, LevelCreationDialogue.PlayingFieldSize],
                new(0, 0),
                0),
            "");
            LoadLevelIntoPage();
            StaticDataStore.ActiveLevel = ActiveLevel;
            PrepareToolSelection();
        }
        else
        {
            ActiveLevel = null;

            ContentDialog levelCreationFailedDialogue = new();
            levelCreationFailedDialogue.XamlRoot = this.Content.XamlRoot;
            levelCreationFailedDialogue.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            levelCreationFailedDialogue.Title = "Level creation failed.";
            levelCreationFailedDialogue.PrimaryButtonText = "Close";
            levelCreationFailedDialogue.SecondaryButtonText = "Try Again";
            levelCreationFailedDialogue.DefaultButton = ContentDialogButton.Primary;
            levelCreationFailedDialogue.Content = new LevelCreationFailed();

            ContentDialogResult result = await levelCreationFailedDialogue.ShowAsync();
            if (result == ContentDialogResult.Secondary)
            {
                MainWindow.Instance.NavViewNavigate("levelSelect", new Microsoft.UI.Xaml.Media.Animation.CommonNavigationTransitionInfo());
                MainWindow.Instance.NavViewNavigate("exerciseView", new Microsoft.UI.Xaml.Media.Animation.CommonNavigationTransitionInfo());
                MainWindow.Instance.NavViewSetSelection(1);
            }

        }


    }

}