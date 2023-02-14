using Microsoft.UI.Dispatching;
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
        QueueUpdate();
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task TurnLeft()
    {
        MapLayout.CurrentPlayerDirection--;
        QueueUpdate();
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task GoForward()
    {

        QueueUpdate();
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task TakeScrew()
    {

        QueueUpdate();
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task PlaceScrew()
    {

        QueueUpdate();
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
    }

    public async Task<bool> WallAhead()
    {

        QueueUpdate();
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
        return false;
    }

    public async Task<bool> ScrewThere()
    {
        QueueUpdate();
        await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
        return false;
    }

    public async void QueueUpdate()
    {
        DispatcherQueue dispatcherQueue = ExercisePage.Instance.DispatcherQueue;
        dispatcherQueue.TryEnqueue(() =>
        {
            ExercisePage.Instance.UpdateDisplay();
        });

    }
}