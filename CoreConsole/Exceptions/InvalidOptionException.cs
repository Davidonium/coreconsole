using System;

namespace CoreConsole.Exceptions
{

    public class InvalidOptionException : ConsoleException
    {
        public InvalidOptionException()
        {
        }

        public InvalidOptionException(string message)
            : base(message)
        {
        }

        public InvalidOptionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}