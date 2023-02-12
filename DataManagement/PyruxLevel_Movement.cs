using System.Threading;
namespace Pyrux.DataManagement;

internal partial class PyruxLevel
{
    public int ExecutionDelayInMilliseconds { get; set; } = 1000;
    public void TurnRight()
    {
        MapLayout.CurrentPlayerDirection++;
        Thread.Sleep(ExecutionDelayInMilliseconds);
    }

    public void TurnLeft()
    {
        MapLayout.CurrentPlayerDirection--;

        Thread.Sleep(ExecutionDelayInMilliseconds);
    }

    public void GoForward()
    {

        Thread.Sleep(ExecutionDelayInMilliseconds);
    }

    public void TakeScrew()
    {

        Thread.Sleep(ExecutionDelayInMilliseconds);
    }

    public void PlaceScrew()
    {

        Thread.Sleep(ExecutionDelayInMilliseconds);
    }

    public bool WallAhead()
    {

        Thread.Sleep(ExecutionDelayInMilliseconds);
        return false;
    }

    public bool ScrewThere()
    {

        Thread.Sleep(ExecutionDelayInMilliseconds);
        return false;
    }


}