using System.Reflection;

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
        public static PyruxLevelMapLayout OriginalActiveLevelMapLayout { get; set; }
        /// <summary>
        /// List of built in levels.
        /// </summary>
        public static List<PyruxLevel> BuiltInLevels { get; set; }
        /// <summary>
        /// List of user created levels.
        /// </summary>
        public static List<PyruxLevel> UserCreatedLevels { get; set; }

        public static bool UnsavedChangesPresent = false;

        public static string VersionNumber 
        {
            get => GetApplicationVersion();
        }

        private static string GetApplicationVersion()
        {
            var version = Windows.ApplicationModel.Package.Current.Id.Version;
            string applicationVersion = string.Format("{0}.{1}.{2}.{3}",
                version.Major,
                version.Minor,
                version.Build,
                version.Revision);
            return applicationVersion;
        }
    }
}
