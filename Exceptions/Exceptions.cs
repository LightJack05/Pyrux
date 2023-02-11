namespace Pyrux.Exceptions;

class WallAheadException : Exception
{
    public WallAheadException() 
    { 
    
    }
    public WallAheadException(string message) : base(message)
    {

    }
}

class NoScrewOnTileException : Exception
{
    public NoScrewOnTileException()
    {

    }
    public NoScrewOnTileException(string message) : base(message)
    {

    }
}

class NoScrewInInventoryException : Exception
{
    public NoScrewInInventoryException()
    {

    }
    public NoScrewInInventoryException(string message) : base(message)
    {

    }
}