using Microsoft.UI.Xaml.Input;
using Pyrux.Pages.ContentDialogs;

namespace Pyrux.Pages;

public sealed partial class ExercisePage
{
    private bool? _dragStartedWithAdding = null;

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
                ChangeChipsTool(new PositionVector2(Grid.GetColumn(clickedBorder), Grid.GetRow(clickedBorder)));
                break;
            case 2:
                PositionVector2 newPlayerPosition = new(Grid.GetColumn(clickedBorder), Grid.GetRow(clickedBorder));
                MovePlayer(newPlayerPosition);
                break;
            default:
                break;
        }

    }


    private void Player_Clicked(object sender, RoutedEventArgs e)
    {
        if (ExecutionRanState)
        {
            return;
        }
        if (SelectedToolIndex == 1 && !PythonScriptRunning)
        {
            ChangeChipsTool(ActiveLevel.MapLayout.CurrentPlayerPosition);
        }

    }

    private void Tile_Entered(object sender, PointerRoutedEventArgs e)
    {
        if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed && SelectedToolIndex == 0)
        {
            Border clickedBorder = VisualTreeHelper.GetParent((Image)sender) as Border;
            PositionVector2 position = new(Grid.GetColumn(clickedBorder), Grid.GetRow(clickedBorder));

            if (_dragStartedWithAdding is null)
            {
                _dragStartedWithAdding = !ActiveLevel.MapLayout.IsWallAtPosition(position);
            }

            if (_dragStartedWithAdding == ActiveLevel.MapLayout.IsWallAtPosition(position))
            {
                SwitchWall(position);
            }

            Tile_Clicked(sender, e);
        }
    }
    private void Page_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        _dragStartedWithAdding = null;
    }

    private void btnWallTool_Click(object sender, RoutedEventArgs e)
    {
        SelectedToolIndex = 0;
        btnWallTool.IsEnabled = false;

        btnChipTool.IsEnabled = true;
        btnPlayerTool.IsEnabled = true;
    }

    private void btnChipTool_Click(object sender, RoutedEventArgs e)
    {
        SelectedToolIndex = 1;
        btnChipTool.IsEnabled = false;

        btnWallTool.IsEnabled = true;
        btnPlayerTool.IsEnabled = true;
    }

    private void btnPlayerTool_Click(object sender, RoutedEventArgs e)
    {
        SelectedToolIndex = 2;
        btnPlayerTool.IsEnabled = false;

        btnWallTool.IsEnabled = true;
        btnChipTool.IsEnabled = true;
    }

    private void btnRotate_Click(object sender, RoutedEventArgs e)
    {
        TurnPlayer();
    }

    /// <summary>
    /// On the given position, switch between placed wall and empty square.
    /// Refuse if there are chips on the tile or the player is on it.
    /// </summary>
    /// <param name="position">Position of the tile that should be changed.</param>
    private void SwitchWall(PositionVector2 position)
    {
        if (position != ActiveLevel.MapLayout.CurrentPlayerPosition && ActiveLevel.MapLayout.GetChipNumberAtPosition(position) == 0)
        {
            ActiveLevel.MapLayout.WallLayout[position.Y, position.X] = !ActiveLevel.MapLayout.WallLayout[position.Y, position.X];
        }
        ActiveLevel.GoalMapLayout.WallLayout = ActiveLevel.MapLayout.Copy().WallLayout;
        UpdateDisplay();
        SaveNewLayout();
        StaticDataStore.UnsavedChangesPresent = true;
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
        StaticDataStore.UnsavedChangesPresent = true;
    }
    /// <summary>
    /// Turn the player around.
    /// </summary>
    private void TurnPlayer()
    {
        ActiveLevel.MapLayout.CurrentPlayerDirection++;
        UpdateDisplay();
        SaveNewLayout();
        StaticDataStore.UnsavedChangesPresent = true;
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
        StaticDataStore.UnsavedChangesPresent = true;
    }
    /// <summary>
    /// Change the chips on the given position.
    /// </summary>
    /// <param name="position">The position to change the chips at.</param>
    private void ChangeChipsTool(PositionVector2 position)
    {
        if (!ActiveLevel.MapLayout.WallLayout[position.Y, position.X])
        {
            PlaceChipsDialog.Position = position;
            ShowChipNumberChangeDialog();
        }
    }
    /// <summary>
    /// Provides a dialogue for changing the number of chips on a tile.
    /// </summary>
    private async void ShowChipNumberChangeDialog()
    {
        ContentDialog chipNumberChangeDialog = new();
        chipNumberChangeDialog.XamlRoot = this.Content.XamlRoot;
        chipNumberChangeDialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        chipNumberChangeDialog.Title = "Chips";
        chipNumberChangeDialog.PrimaryButtonText = "Save";
        chipNumberChangeDialog.SecondaryButtonText = "Cancel";
        chipNumberChangeDialog.DefaultButton = ContentDialogButton.Primary;
        chipNumberChangeDialog.Content = new PlaceChipsDialog();

        ContentDialogResult result = await chipNumberChangeDialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            ChipNumberChangeDialogFinished();
        }

    }
    /// <summary>
    /// Updates the chip count on a tile with the position and chip number from the last PlaceChipsDialogue.
    /// </summary>
    private void ChipNumberChangeDialogFinished()
    {
        UpdateChipCount(PlaceChipsDialog.Position, PlaceChipsDialog.ChipNumber);
    }
    /// <summary>
    /// Update the chip count at a position.
    /// </summary>
    /// <param name="position">The position to update.</param>
    /// <param name="count">The new count.</param>
    private void UpdateChipCount(PositionVector2 position, int count)
    {
        ActiveLevel.MapLayout.SetChipNumberAtPosition(position, count);
        SaveNewLayout();
        UpdateDisplay();
        StaticDataStore.UnsavedChangesPresent = true;
    }

}