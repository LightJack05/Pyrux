namespace Pyrux.Pages;
public sealed partial class ExercisePage
{
    public bool isGoalLayoutOverlayEnabled { get; set; }
    private void tswGoalLayoutOverlay_Toggled(object sender, RoutedEventArgs e)
    {
        if (((ToggleSwitch)sender).IsOn)
        {
            isGoalLayoutOverlayEnabled = true;
            btnStart.IsEnabled = false;
            btnStep.IsEnabled = false;
            btnReset.IsEnabled = false;
        }
        else
        {
            isGoalLayoutOverlayEnabled = false;
            btnStart.IsEnabled = true;
            btnStep.IsEnabled = true;
            btnReset.IsEnabled = true;
        }

        UpdateDisplay();
        PrepareToolSelection();
    }
}