using System;
using System.IO;
using CoreConsole.Input;

namespace CoreConsole
{
    public abstract class Command : ICommand
    {
        public string Identifier { get; protected set; }
        public InputDefinition Definition { get; private set; }

        public Command(string identifier = null)
        {
            if (!String.IsNullOrEmpty(identifier))
            {
                Identifier = identifier;
            }
            Definition = new InputDefinition();

            Configure();

            if (String.IsNullOrEmpty(Identifier))
            {
                throw new NullReferenceException(String.Format("The command for class {0} needs an identifier", GetType().Name));
            }
            
        }

        public abstract void Execute(CommandInput input, TextWriter output);

        protected abstract void Configure();


        protected void AddArgument(string name, InputArgumentMode mode, string defaultValue = null)
        {
            var argument = new InputArgument(name, mode, defaultValue);
            Definition.AddArgument(argument);
        }

        protected void AddOption(string name, string shortcut, InputOptionValueMode mode, string defaultValue = null)
        {
            var option = new InputOption(name, shortcut, mode, defaultValue);
            Definition.AddOption(option);
        }
    }
}