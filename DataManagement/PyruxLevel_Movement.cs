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
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task TurnLeft()
    {
        MapLayout.CurrentPlayerDirection--;

        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task GoForward()
    {

        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task TakeScrew()
    {

        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task PlaceScrew()
    {

        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task<bool> WallAhead()
    {

        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
        return false;
    }

    public async Task<bool> ScrewThere()
    {

        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
        return false;
    }


}