
using Pyrux.Pages.ContentDialogs;

namespace Pyrux.Pages;

public sealed partial class ExercisePage
{


    async void CreateNewLevel()
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

    void LevelCreationDialogueFinished()
    {
        if (!(LevelCreationDialogue.LevelName == String.Empty) || (LevelCreationDialogue.LevelName.Trim().Length == 0))
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
        }


    }

}