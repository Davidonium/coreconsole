using System;
using CoreConsole.Exceptions;

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
        public string Description { get; private set; }
        private string _defaultValue;
        public string DefaultValue { 
            get
            {
                return _defaultValue;
            } 
            private set     
            {
                if (value != null && Mode == InputArgumentMode.Required)
                {
                    throw new RuntimeException("Cannot set a default value when the argument is required");
                }

                _defaultValue = value;
            } 
        }

        public InputArgument(string name, InputArgumentMode mode = InputArgumentMode.Required, string description = "", string defaultValue = null)
        {
            Name = name;
            Mode = mode;
            DefaultValue = defaultValue;
            Description = description;
        }
    }
}