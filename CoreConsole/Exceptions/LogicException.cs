using System;

namespace CoreConsole.Exceptions
{

    public class LogicException : ConsoleException
    {
        public LogicException()
        {
        }

        public LogicException(string message)
            : base(message)
        {
        }

        public LogicException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}