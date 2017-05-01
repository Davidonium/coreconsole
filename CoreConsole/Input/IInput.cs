namespace CoreConsole.Input
{
    interface IInput
    {
        void Bind(InputDefinition definition);
        void Validate();
        void Parse();

        bool HasOption(string name);
        string GetOption(string name);

        bool HasArgument(string name);
        string GetArgument(string name);
    }
}