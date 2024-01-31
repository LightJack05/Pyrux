namespace Pyrux.DataManagement.Restrictions;

public enum UserFunction
{
    GoForward,
    TurnLeft,
    TurnRight,
    ChipThere,
    PlaceChip,
    TakeChip,
    WallAhead
}

public static class UserFunctionExtensions
{
    /// <summary>
    /// Append a '()' to the ToString result of the function.
    /// </summary>
    /// <param name="userFuntion">Function to convert to call string</param>
    /// <returns>A string representing a call to the function from python</returns>
    public static string ToCallString(this UserFunction userFuntion)
    {
        return userFuntion.ToString() + "()";
    }

}