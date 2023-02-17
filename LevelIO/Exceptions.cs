using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrux.LevelIO
{
    class AppdataFolderNotFoundException : Exception
    {
        public AppdataFolderNotFoundException() { }
        public AppdataFolderNotFoundException(string message) : base(message) { }
    }

    class LevelsFolderNotFoundException : Exception
    {
        public LevelsFolderNotFoundException() { }
        public LevelsFolderNotFoundException(string message) : base(message) { }
    }

    class LevelNotFoundException : Exception
    {
        public LevelNotFoundException() { }
        public LevelNotFoundException(string message) : base(message) { }
    }

    class InvalidLevelJsonException : Exception 
    { 
        public InvalidLevelJsonException() { }
        public InvalidLevelJsonException(string message) : base(message) { }
    }

    class LevelJsonNotFoundException : Exception
    {
        public LevelJsonNotFoundException() { }
        public LevelJsonNotFoundException(string message) : base(message) { }
    }
}
