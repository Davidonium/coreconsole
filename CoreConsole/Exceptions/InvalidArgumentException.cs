using System;

namespace CoreConsole.Exceptions
{
    public class InvalidArgumentException : ConsoleException
    {
        public InvalidArgumentException()
        {
        }

        public InvalidArgumentException(string message)
            : base(message)
        {
        }

        public InvalidArgumentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}