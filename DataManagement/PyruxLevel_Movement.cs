using Pyrux.Pages;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pyrux.DataManagement;

internal partial class PyruxLevel
{
    public int ExecutionDelayInMilliseconds { get; set; } = 1000;
    public async Task TurnRight()
    {
        MapLayout.CurrentPlayerDirection++;
        ExercisePage.Instance.UpdateDisplay();
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task TurnLeft()
    {
        MapLayout.CurrentPlayerDirection--;
        ExercisePage.Instance.UpdateDisplay();
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task GoForward()
    {

        ExercisePage.Instance.UpdateDisplay();
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task TakeScrew()
    {

        ExercisePage.Instance.UpdateDisplay();
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task PlaceScrew()
    {

        ExercisePage.Instance.UpdateDisplay();
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task<bool> WallAhead()
    {

        ExercisePage.Instance.UpdateDisplay();
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
        return false;
    }

    public async Task<bool> ScrewThere()
    {

        ExercisePage.Instance.UpdateDisplay();
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
        return false;
    }


}