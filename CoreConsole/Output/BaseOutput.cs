using System;
using System.IO;

namespace CoreConsole.Output
{
    public class BaseOutput : IOutput
    {
        public TextWriter Out { get; private set; }

        public TextWriter Error { get; private set; }

        public BaseOutput(TextWriter output, TextWriter error)
        {
            Out = output;
            Error = error;
        }
    }
}