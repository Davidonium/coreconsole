using System.IO;

namespace CoreConsole.Output
{
    interface IOutput
    {
        TextWriter Out { get; }
        TextWriter Error { get; }
     }
}