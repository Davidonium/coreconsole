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
        public string Description { get; private set; }


        public InputOption(string name,
                           string shortcut = null,
                           InputOptionValueMode mode = InputOptionValueMode.None,
                           string description = "",
                           string defaultValue = null)
        {

            if (name.StartsWith("--"))
            {
                Name = name.Substring(2);
            }
            else
            {
                Name = name;
            }

            if (shortcut != null)
            {
                if (shortcut.StartsWith("-"))
                {
                    Shortcut = shortcut.Substring(1);
                }
                else
                {
                    Shortcut = shortcut.TrimStart('-');
                }
            }

            Mode = mode;
            DefaultValue = defaultValue;
            Description = description;
        }


        public bool AcceptsValue()
        {
            return Mode == InputOptionValueMode.Required || Mode == InputOptionValueMode.Optional;
        }
    }
}