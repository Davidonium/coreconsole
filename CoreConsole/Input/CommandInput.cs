using System;
using System.Collections.Generic;

namespace CoreConsole.Input
{
    public class CommandInput
    {
        private readonly string[] _tokens;
        private readonly IList<string> _remainingTokens;
        private readonly InputDefinition _definition;
        private readonly IDictionary<string, string> _arguments;
        private readonly IDictionary<string, string> _options;
        
        public CommandInput(string[] tokens, InputDefinition definition = null)
        {
            _tokens = tokens;
            _remainingTokens = new List<string>(_tokens);
            _definition = definition ?? new InputDefinition();
            _arguments = new Dictionary<string, string>();
            _options = new Dictionary<string, string>();
        }


        public void Parse()
        {
            bool parseOptions = true;
            
            while (_remainingTokens.Count > 0)
            {
                string token = _remainingTokens[0];
                _remainingTokens.Remove(token);

                if (parseOptions && token == "--")
                {
                    parseOptions = false;
                }
                else if (parseOptions && token.StartsWith("--"))
                {
                    ParseLongOption(token);
                }
                else if (parseOptions && token.StartsWith("-") && token.Length > 1)
                {
                    ParseShortOption(token);
                }
                else
                {
                    ParseArgument(token);
                }

            }
        }
        
        private void ParseShortOption(string token)
        {
            string name = token.Substring(1);

            if (name.Length > 1)
            {
                string firstLetter = name.Substring(0, 1);
                if (_definition.HasShortcut(firstLetter) && _definition.GetOptionForShortcut(firstLetter).AcceptsValue())
                {
                    AddShortOption(firstLetter, name.Substring(1));
                }
                else
                {
                    ParseShortOptionSet(name);
                }
            }
            else
            {
                AddShortOption(name);
            }


        }
        private void ParseLongOption(string token)
        {
            string name = token.Substring(2);

            int pos = name.IndexOf('=');
            if (pos != -1)
            {
                string val = name.Substring(pos + 1);
                AddLongOption(name.Substring(0, pos), val);
            }
            else
            {
                AddLongOption(name);
            }

        }

        private void ParseArgument(string token)
        {
            int nArguments = _arguments.Count;

            if (_definition.HasArgument(nArguments))
            {
                InputArgument argument = _definition.GetArgument(nArguments);
                _arguments.Add(argument.Name, token);
            }
            else
            {
                var definedArguments = _definition.GetArguments();
                if (definedArguments.Count > 0)
                {
                    throw new InvalidOperationException(String.Format("Too many arguments, expected {0}", String.Join(" ", definedArguments.Keys)));
                }

                throw new InvalidOperationException("No arguments expected");
            }

        }

        private void ParseShortOptionSet(string name)
        {

            char[] itName = name.ToCharArray();

            for(int i = 0; i < itName.Length; ++i)
            {
                string currentOption = itName[i].ToString();
                if (_definition.HasShortcut(currentOption) == false)
                {
                    throw new InvalidOperationException(String.Format("The option \"{0}\" does not exist", currentOption));
                }

                InputOption option = _definition.GetOptionForShortcut(currentOption);

                if (option.AcceptsValue())
                {
                    AddLongOption(option.Name, i == itName.Length - 1 ? null : name.Substring(i + 1));
                    break;
                }
                else
                {
                    AddLongOption(option.Name);
                }
            }
        }
        
        private void AddShortOption(string name, string val = null)
        {
            if (_definition.HasShortcut(name) == false)
            {
                throw new InvalidOperationException(String.Format("The option \"{0}\" does not exist", name));
            }

            AddLongOption(_definition.GetOptionForShortcut(name).Name, val);
        }

        private void AddLongOption(string name, string val = null)
        {
            if (_definition.HasOption(name) == false)
            {
                 throw new InvalidOperationException(String.Format("The option \"{0}\" does not exist", name));
            }

            InputOption option = _definition.GetOption(name);


            if (option.AcceptsValue() == false && val != null)
            {
                throw new InvalidOperationException(String.Format("The option \"{0}\" does not accept a value", option.Name));
            }

            if (val == null && option.AcceptsValue() && _remainingTokens.Count > 0)
            {
                string next = _remainingTokens[0];
                _remainingTokens.RemoveAt(0);
                if (next.Length > 0 && next.StartsWith("-") == false)
                {
                    val = next;
                }
                else if (String.IsNullOrEmpty(next))
                {
                    val = null;
                }
                else
                {
                    next.Insert(0, next);
                }

            }

            if (val == null)
            {
                if (option.Mode == InputOptionValueMode.Required)
                {
                    throw new InvalidOperationException(String.Format("The option {0} requires a value", name));
                }
                val = option.Mode == InputOptionValueMode.Optional ? option.DefaultValue : "";
            }

            _options.Add(name, val);
        }

        public string GetOption(string name)
        {
            return _options[name];
        }

        public string GetArgument(string name)
        {
            return _arguments[name];
        }

        public bool HasOption(string name)
        {
            return _options.ContainsKey(name);
        }

        public bool HasArgument(string name)
        {
            return _arguments.ContainsKey(name);
        }
    }

}