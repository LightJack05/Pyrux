namespace Pyrux.LevelIO
{
    internal class AppdataFolderNotFoundException : Exception
    {
        public AppdataFolderNotFoundException() { }
        public AppdataFolderNotFoundException(string message) : base(message) { }
    }

    internal class LevelsFolderNotFoundException : Exception
    {
        public LevelsFolderNotFoundException() { }
        public LevelsFolderNotFoundException(string message) : base(message) { }
    }

    internal class LevelNotFoundException : Exception
    {
        public LevelNotFoundException() { }
        public LevelNotFoundException(string message) : base(message) { }
    }

    internal class InvalidLevelJsonException : Exception
    {
        public InvalidLevelJsonException() { }
        public InvalidLevelJsonException(string message) : base(message) { }
    }

    internal class LevelJsonNotFoundException : Exception
    {
        public LevelJsonNotFoundException() { }
        public LevelJsonNotFoundException(string message) : base(message) { }
    }
}
