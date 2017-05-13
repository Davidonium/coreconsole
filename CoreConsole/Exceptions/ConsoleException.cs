using System;

namespace CoreConsole.Exceptions
{

    public class ConsoleException : Exception
    {
        public ConsoleException()
        {
        }

        public ConsoleException(string message)
            : base(message)
        {
        }

        public ConsoleException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}