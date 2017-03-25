using CoreConsole.Input;
using System.IO;

namespace CoreConsole
{
    public interface ICommand
    {
        string Identifier { get; }
        InputDefinition Definition { get; }
        void Execute(CommandInput input, TextWriter output);
    }
}