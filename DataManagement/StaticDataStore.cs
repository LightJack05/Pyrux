namespace Pyrux.DataManagement
{
    internal static class StaticDataStore
    {
        /// <summary>
        /// The active level currently being displayed by the application.
        /// </summary>
        public static PyruxLevel ActiveLevel { get; set; }
        /// <summary>
        /// The execution delay in milliseconds for each movement step.
        /// </summary>
        public static int ExecutionDelayInMilliseconds { get; set; }
    }
}
