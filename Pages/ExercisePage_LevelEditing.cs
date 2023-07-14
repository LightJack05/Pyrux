using Pyrux.Pages.ContentDialogs;

namespace Pyrux.Pages;

public sealed partial class ExercisePage
{

    /// <summary>
    /// On clicking a tile, apply the selected tool to the tile that was clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Tile_Clicked(object sender, RoutedEventArgs e)
    {
        if (ExecutionRanState)
        {
            return;
        }

        Border clickedBorder = VisualTreeHelper.GetParent((Image)sender) as Border;
        switch (SelectedToolIndex)
        {
            case 0:
                SwitchWall(new PositionVector2(Grid.GetColumn(clickedBorder), Grid.GetRow(clickedBorder)));
                break;
            case 1:
                ChangeScrewsTool(new PositionVector2(Grid.GetColumn(clickedBorder), Grid.GetRow(clickedBorder)));
                break;
            case 2:
                PositionVector2 newPlayerPosition = new(Grid.GetColumn(clickedBorder), Grid.GetRow(clickedBorder));
                MovePlayer(newPlayerPosition);
                break;
            default:
                break;
        }

    }

    private void btnWallTool_Click(object sender, RoutedEventArgs e)
    {
        SelectedToolIndex = 0;
        btnWallTool.IsEnabled = false;

        btnScrewTool.IsEnabled = true;
        btnPlayerTool.IsEnabled = true;
    }

    private void btnScrewTool_Click(object sender, RoutedEventArgs e)
    {
        SelectedToolIndex = 1;
        btnScrewTool.IsEnabled = false;

        btnWallTool.IsEnabled = true;
        btnPlayerTool.IsEnabled = true;
    }

    private void btnPlayerTool_Click(object sender, RoutedEventArgs e)
    {
        SelectedToolIndex = 2;
        btnPlayerTool.IsEnabled = false;

        btnWallTool.IsEnabled = true;
        btnScrewTool.IsEnabled = true;
    }

    private void btnRotate_Click(object sender, RoutedEventArgs e)
    {
        TurnPlayer();
    }

    /// <summary>
    /// On the given position, switch between placed wall and empty square.
    /// Refuse if there are screws on the tile or the player is on it.
    /// </summary>
    /// <param name="position">Position of the tile that should be changed.</param>
    private void SwitchWall(PositionVector2 position)
    {
        if (position != ActiveLevel.MapLayout.CurrentPlayerPosition && ActiveLevel.MapLayout.GetScrewNumberAtPosition(position) == 0)
        {
            ActiveLevel.MapLayout.WallLayout[position.Y, position.X] = !ActiveLevel.MapLayout.WallLayout[position.Y, position.X];
        }
        ActiveLevel.GoalMapLayout.WallLayout = ActiveLevel.MapLayout.Copy().WallLayout;
        UpdateDisplay();
        SaveNewLayout();
    }
    /// <summary>
    /// Move the player to the given position.
    /// </summary>
    /// <param name="position">The new position to move to.</param>
    private void MovePlayer(PositionVector2 position)
    {
        if (!ActiveLevel.MapLayout.WallLayout[position.Y, position.X])
        {
            ActiveLevel.MapLayout.CurrentPlayerPosition = position.Copy();
        }
        UpdateDisplay();
        SaveNewLayout();
    }
    /// <summary>
    /// Turn the player around.
    /// </summary>
    private void TurnPlayer()
    {
        ActiveLevel.MapLayout.CurrentPlayerDirection++;
        UpdateDisplay();
        SaveNewLayout();
    }
    /// <summary>
    /// Save the new layout to the static data store.
    /// </summary>
    private void SaveNewLayout()
    {
        StaticDataStore.OriginalActiveLevelMapLayout = ActiveLevel.MapLayout.Copy();
    }



    private void btnEditTask_Click(object sender, RoutedEventArgs e)
    {
        ShowTaskEditDialogAsync();
    }

    private async void ShowTaskEditDialogAsync()
    {
        ContentDialog taskEditDialog = new();
        taskEditDialog.XamlRoot = this.Content.XamlRoot;
        taskEditDialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        taskEditDialog.Title = "Change the level description";
        taskEditDialog.PrimaryButtonText = "Save";
        taskEditDialog.SecondaryButtonText = "Cancel";
        taskEditDialog.DefaultButton = ContentDialogButton.Primary;
        taskEditDialog.Content = new ContentDialogs.EditTaskContentDialog();

        ContentDialogResult result = await taskEditDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            ChangeTaskDialogCompleted();
        }
    }

    private void ChangeTaskDialogCompleted()
    {
        UpdateTaskText(ContentDialogs.EditTaskContentDialog.LevelDescription);
    }
    /// <summary>
    /// Set the task text to the given text.
    /// </summary>
    /// <param name="text">The text to set the task to.</param>
    private void UpdateTaskText(string text)
    {
        ActiveLevel.Task = text;
        txtLevelTask.Text = ActiveLevel.Task;
    }
    /// <summary>
    /// Change the screws on the given position.
    /// </summary>
    /// <param name="position">The position to change the screws at.</param>
    private void ChangeScrewsTool(PositionVector2 position)
    {
        if (!ActiveLevel.MapLayout.WallLayout[position.Y, position.X])
        {
            PlaceScrewsDialog.Position = position;
            ShowScrewNumberChangeDialog();
        }
    }
    /// <summary>
    /// Provides a dialogue for changing the number of screws on a tile.
    /// </summary>
    private async void ShowScrewNumberChangeDialog()
    {
        ContentDialog screwNumberChangeDialog = new();
        screwNumberChangeDialog.XamlRoot = this.Content.XamlRoot;
        screwNumberChangeDialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        screwNumberChangeDialog.Title = "Screws";
        screwNumberChangeDialog.PrimaryButtonText = "Save";
        screwNumberChangeDialog.SecondaryButtonText = "Cancel";
        screwNumberChangeDialog.DefaultButton = ContentDialogButton.Primary;
        screwNumberChangeDialog.Content = new PlaceScrewsDialog();

        ContentDialogResult result = await screwNumberChangeDialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            ScrewNumberChangeDialogFinished();
        }

    }
    /// <summary>
    /// Updates the screw count on a tile with the position and screw number from the last PlaceScrewsDialogue.
    /// </summary>
    private void ScrewNumberChangeDialogFinished()
    {
        UpdateScrewCount(PlaceScrewsDialog.Position, PlaceScrewsDialog.ScrewNumber);
    }
    /// <summary>
    /// Update the screw count at a position.
    /// </summary>
    /// <param name="position">The position to update.</param>
    /// <param name="count">The new count.</param>
    private void UpdateScrewCount(PositionVector2 position, int count)
    {
        ActiveLevel.MapLayout.SetScrewNumberAtPosition(position, count);
        SaveNewLayout();
        UpdateDisplay();
    }

}