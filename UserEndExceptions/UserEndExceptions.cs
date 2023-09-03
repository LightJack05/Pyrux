namespace Pyrux.UserEndExceptions;

/// <summary>
/// Thrown if the player runs into a wall.
/// </summary>
internal class WallAheadException : Exception
{
    public WallAheadException()
    {

    }
    public WallAheadException(string message) : base(message)
    {

    }
}

/// <summary>
/// Thrown if the player tries to take a chip from an empty tile.
/// </summary>
internal class NoChipOnTileException : Exception
{
    public NoChipOnTileException()
    {

    }
    public NoChipOnTileException(string message) : base(message)
    {

    }
}

/// <summary>
/// Thrown if the player tries to place a chip with an empty inventory.
/// </summary>
internal class NoChipInInventoryException : Exception
{
    public NoChipInInventoryException()
    {

    }
    public NoChipInInventoryException(string message) : base(message)
    {

    }
}

/// <summary>
/// Thrown when the execution of the python thread is supposed to be cancelled.
/// </summary>
internal class ExecutionCancelledException : Exception
{
    public ExecutionCancelledException()
    {

    }
    public ExecutionCancelledException(string message) : base(message)
    {

    }
}