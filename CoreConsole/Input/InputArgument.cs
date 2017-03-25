namespace CoreConsole.Input
{
    public enum InputArgumentMode 
    {
        Required,
        Optional
    }
    public class InputArgument
    {
        public string Name { get; private set; }
        public InputArgumentMode Mode { get; private set; }
        public string DefaultValue { get; private set; }
        public InputArgument(string name, InputArgumentMode mode, string defaultValue = null)
        {
            Name = name;
            Mode = mode;
            DefaultValue = defaultValue;
        }
    }
}