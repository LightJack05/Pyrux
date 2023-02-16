using Microsoft.UI.Dispatching;
using Pyrux.Pages;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pyrux.DataManagement;

internal partial class PyruxLevel
{
    public int ExecutionDelayInMilliseconds { get; set; } = 1000;
    public void TurnRight()
    {
        MapLayout.CurrentPlayerDirection++;
        QueueUpdate();
        //await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
        Thread.Sleep(ExecutionDelayInMilliseconds);
    }

    public void TurnLeft()
    {
        MapLayout.CurrentPlayerDirection--;
        QueueUpdate();
        //await System.Threading.Tasks.Task.Delay(ExecutionDelayInMilliseconds);
        Thread.Sleep(ExecutionDelayInMilliseconds);
    }

    public void GoForward()
    {
        

        QueueUpdate();
        Thread.Sleep(ExecutionDelayInMilliseconds);
    }

    public void TakeScrew()
    {

        QueueUpdate();
        Thread.Sleep(ExecutionDelayInMilliseconds);
    }

    public void PlaceScrew()
    {

        QueueUpdate();
        Thread.Sleep(ExecutionDelayInMilliseconds);
    }

    public bool WallAhead()
    {

        QueueUpdate();
        Thread.Sleep(ExecutionDelayInMilliseconds);
        return false;
    }

    public bool ScrewThere()
    {
        QueueUpdate();
        Thread.Sleep(ExecutionDelayInMilliseconds);
        return false;
    }

    public void QueueUpdate()
    {
        DispatcherQueue dispatcherQueue = ExercisePage.Instance.DispatcherQueue;
        dispatcherQueue.TryEnqueue(() =>
        {
            ExercisePage.Instance.UpdateDisplay();
        });

    }
}