using System;
using System.Collections.Generic;

namespace CoreConsole.Input
{
    public class InputDefinition
    {
        private readonly IDictionary<string,InputArgument> _arguments;
        private readonly IDictionary<string,InputOption> _options;
        private readonly IDictionary<string,string> _shortcuts;
        private readonly IList<string> _argumentPositions;
        private bool _hasOptionalArgument;

        public InputDefinition()
        {
            _arguments = new Dictionary<string,InputArgument>();
            _options = new Dictionary<string,InputOption>();
            _shortcuts = new Dictionary<string,string>();
            _argumentPositions = new List<string>();
            _hasOptionalArgument = false;
        }

        public void AddArgument(InputArgument argument)
        {
            if (_arguments.ContainsKey(argument.Name))
            {
                throw new InvalidOperationException(String.Format("An argument with name {0} already exists", argument.Name));
            }
            
            if (argument.Mode == InputArgumentMode.Optional)
            {
                _hasOptionalArgument = true;
            } 
            else if (_hasOptionalArgument)
            {
                throw new InvalidOperationException("Cannot add an argument after an optional, arguments are ordered");
            }


            _arguments.Add(argument.Name, argument);
            _argumentPositions.Add(argument.Name);
        }

        public void AddOption(InputOption option)
        {
            if (_options.ContainsKey(option.Name))
            {
                throw new InvalidOperationException(String.Format("An option with \"{0}\" name already exists", option.Name));
            }

            if (_shortcuts.ContainsKey(option.Shortcut))
            {
                throw new InvalidOperationException(String.Format("There already is a \"{0}\" shortcut", option.Shortcut));
            }

            _shortcuts.Add(option.Shortcut, option.Name);
            
            _options.Add(option.Name, option);
        }

        public InputArgument GetArgument(string name)
        {
            return _arguments[name];
        }

        public InputOption GetOption(string name)
        {
            return _options[name];
        }

        public bool HasArgument(string name)
        {
            return _arguments.ContainsKey(name);
        }

        public bool HasArgument(int pos)
        {
            return _arguments.ContainsKey(_argumentPositions[pos]);
        }

        public bool HasOption(string name)
        {
            return _options.ContainsKey(name);
        }
        
        public IDictionary<string,InputArgument> GetArguments()
        {
            return new Dictionary<string,InputArgument>(_arguments);
        }

        public IDictionary<string,InputOption> GetOptions()
        {
            return new Dictionary<string,InputOption>(_options);
        }

        public IDictionary<string,string> GetShortcuts()
        {
            return new Dictionary<string,string>(_shortcuts);
        }

        public bool HasShortcut(string name)
        {
            return _shortcuts.ContainsKey(name);
        }

        public string ShortcutToName(string shortcut)
        {
            return _shortcuts[shortcut];
        }

        public InputOption GetOptionForShortcut(string shortcut)
        {
            return GetOption(ShortcutToName(shortcut));
        }

        public InputArgument GetArgument(int pos)
        {
            return _arguments[_argumentPositions[pos]];
        }
    }
}