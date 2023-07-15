﻿using Microsoft.UI.Dispatching;

using System.Threading;

namespace Pyrux.DataManagement;

internal partial class PyruxLevel
{
    /// <summary>
    /// Make the player turn right.
    /// </summary>
    public void TurnRight()
    {
        MapLayout.CurrentPlayerDirection++;
        QueueUpdate();
        WaitAndCheckIfCancelled();
    }
    /// <summary>
    /// Make the player turn left.
    /// </summary>
    public void TurnLeft()
    {
        MapLayout.CurrentPlayerDirection--;
        QueueUpdate();
        WaitAndCheckIfCancelled();
    }
    /// <summary>
    /// Make the player go forward.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the angle of the player on the map is not a valid movement direction (up,down,left,right).</exception>
    /// <exception cref="Pyrux.UserEndExceptions.WallAheadException">Thrown when the movement results in a collision with a wall, or the position would move outside the map.</exception>
    public void GoForward()
    {
        PositionVector2 movementVector = new();
        switch (MapLayout.CurrentPlayerDirection * 90 % 360)
        {
            case 0:
                movementVector = new(1, 0);
                break;
            case 90:
                movementVector = new(0, 1);
                break;
            case 180:
                movementVector = new(-1, 0);
                break;
            case 270:
                movementVector = new(0, -1);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        PositionVector2 newPosition = MapLayout.CurrentPlayerPosition.Copy() + movementVector;
        if (WallAhead())
        {
            throw new Pyrux.UserEndExceptions.WallAheadException("A wall was hit.");
        }

        MapLayout.CurrentPlayerPosition = newPosition;

        QueueUpdate();
        WaitAndCheckIfCancelled();
    }
    /// <summary>
    /// Take one screw from the tile and place it into the inventory.
    /// </summary>
    /// <exception cref="Pyrux.UserEndExceptions.NoScrewOnTileException">Thrown if there is no screw on the tile.</exception>
    public void TakeScrew()
    {

        if (MapLayout.GetScrewNumberAtPosition(MapLayout.CurrentPlayerPosition) > 0)
        {
            MapLayout.SetScrewNumberAtPosition(MapLayout.CurrentPlayerPosition, MapLayout.GetScrewNumberAtPosition(MapLayout.CurrentPlayerPosition) - 1);
            MapLayout.PlayerScrewInventory++;
        }
        else
        {
            throw new Pyrux.UserEndExceptions.NoScrewOnTileException("There was no screw to pick up on the tile.");
        }

        QueueUpdate();
        WaitAndCheckIfCancelled();
    }
    /// <summary>
    /// Take a screw from the inventory and place it on the tile below the player.
    /// </summary>
    /// <exception cref="Pyrux.UserEndExceptions.NoScrewInInventoryException">Thrown when there is no screw in the inventory.</exception>
    public void PlaceScrew()
    {
        if (MapLayout.PlayerScrewInventory > 0)
        {
            MapLayout.SetScrewNumberAtPosition(MapLayout.CurrentPlayerPosition, MapLayout.GetScrewNumberAtPosition(MapLayout.CurrentPlayerPosition) + 1);
            MapLayout.PlayerScrewInventory--;
        }
        else
        {
            throw new Pyrux.UserEndExceptions.NoScrewInInventoryException("There was no screw to place in the inventory.");
        }

        QueueUpdate();
        WaitAndCheckIfCancelled();
    }
    /// <summary>
    /// Check if there is a wall ahead of the player.
    /// </summary>
    /// <returns>True if a wall is found ahead, false if not.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the angle of the player on the map is not a valid movement direction (up,down,left,right).</exception>
    public bool WallAhead()
    {
        PositionVector2 movementVector = new();
        switch (MapLayout.CurrentPlayerDirection * 90 % 360)
        {
            case 0:
                movementVector = new(1, 0);
                break;
            case 90:
                movementVector = new(0, 1);
                break;
            case 180:
                movementVector = new(-1, 0);
                break;
            case 270:
                movementVector = new(0, -1);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        PositionVector2 newPosition = MapLayout.CurrentPlayerPosition.Copy() + movementVector;
        if (newPosition.X < 0 || newPosition.Y < 0 || newPosition.X > MapLayout.SizeX - 1 || newPosition.Y > MapLayout.SizeY - 1)
        {
            return true;
        }
        else if (MapLayout.IsWallAtPosition(newPosition))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Check if there is a screw inside the players inventory.
    /// </summary>
    /// <returns>True if there is a screw in the inventory, otherwise false.</returns>
    public bool ScrewThere()
    {
        if (MapLayout.GetScrewNumberAtPosition(MapLayout.CurrentPlayerPosition) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Queue an update of the display grid.
    /// </summary>
    private void QueueUpdate()
    {
        DispatcherQueue dispatcherQueue = ExercisePage.Instance.DispatcherQueue;
        dispatcherQueue.TryEnqueue(() =>
        {
            ExercisePage.Instance.UpdateDisplay();
        });

    }
    /// <summary>
    /// Wait for the amount of milliseconds specified in _executionDelayInMilliseconds.
    /// 
    /// </summary>
    /// <exception cref="Pyrux.UserEndExceptions.ExecutionCancelledException">Thrown when the execution is cancelled. (Instance of the Exercise page get's it's ExecutionCanceled property set to true.)</exception>
    private void WaitAndCheckIfCancelled()
    {
        if(ExercisePage.Instance.IsStepModeEnabled)
        {
            while (!ExercisePage.Instance.IsNextStepRequested)
            {
                if (ExercisePage.Instance.ExecutionCancelled)
                {
                    ExercisePage.Instance.ExecutionCancelled = false;

                    throw new Pyrux.UserEndExceptions.ExecutionCancelledException();
                }
                Thread.Sleep(5);
            }
            ExercisePage.Instance.IsNextStepRequested = false;
        }
        else
        {
            for (int i = 0; i < DataManagement.PyruxSettings.ExecutionDelayInMilliseconds; i += 10)
            {
                if (ExercisePage.Instance.ExecutionCancelled)
                {
                    ExercisePage.Instance.ExecutionCancelled = false;

                    throw new Pyrux.UserEndExceptions.ExecutionCancelledException();
                }
                Thread.Sleep(10);
            }
        }
    }
}