namespace CoreConsole.Input
{
    public enum InputOptionValueMode 
    {
        Required,
        Optional,
        None
    }
    
    public class InputOption
    {
        public string Name { get; private set; }
        public string Shortcut { get; private set; }
        public InputOptionValueMode Mode { get; private set; }
        public string DefaultValue { get; private set; }


        public InputOption(string name, string shortcut, InputOptionValueMode mode, string defaultValue = null)
        {
            Name = name;
            Shortcut = shortcut;
            Mode = mode;
            DefaultValue = defaultValue;
        }


        public bool AcceptsValue()
        {
            return Mode == InputOptionValueMode.Required || Mode == InputOptionValueMode.Optional;
        }
    }
}