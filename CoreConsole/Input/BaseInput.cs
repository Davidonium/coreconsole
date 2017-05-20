using System;
using System.Collections.Generic;
using CoreConsole.Exceptions;

namespace CoreConsole.Input
{
    public class BaseInput : IInput
    {
        private readonly string[] _tokens;
        private readonly IList<string> _remainingTokens;
        private InputDefinition _definition;
        private IDictionary<string, string> _arguments = new Dictionary<string, string>();
        private IDictionary<string, string> _options = new Dictionary<string, string>();

        public BaseInput(string[] tokens, InputDefinition definition = null)
        {
            _tokens = tokens;
            _remainingTokens = new List<string>(_tokens);

            if (definition != null)
            {
                Bind(definition);
                Validate();
            }
            else
            {
                _definition = new InputDefinition();
            }
        }

        public void Bind(InputDefinition definition)
        {
            _arguments = new Dictionary<string, string>();
            _options = new Dictionary<string, string>();
            _definition = definition;
            Parse();
        }

        public void Validate()
        {
            int missingArguments = 0;

            foreach (var argument in _definition.GetArguments())
            {
                if (!_arguments.ContainsKey(argument.Key) && argument.Value.Mode == InputArgumentMode.Required)
                {
                    missingArguments++;
                }
            }

            if (missingArguments > 0)
            {
                throw new RuntimeException("Not enough arguments");
            }
        }

        protected void Parse()
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
                    throw new RuntimeException(String.Format("Too many arguments, expected {0}", String.Join(" ", definedArguments.Keys)));
                }

                throw new RuntimeException("No arguments expected");
            }

        }

        private void ParseShortOptionSet(string name)
        {

            char[] itName = name.ToCharArray();

            for (int i = 0; i < itName.Length; ++i)
            {
                string currentOption = itName[i].ToString();
                if (_definition.HasShortcut(currentOption) == false)
                {
                    throw new RuntimeException(String.Format("The option \"{0}\" does not exist", currentOption));
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
                throw new RuntimeException(String.Format("The option \"{0}\" does not exist", name));
            }

            AddLongOption(_definition.GetOptionForShortcut(name).Name, val);
        }

        private void AddLongOption(string name, string val = null)
        {
            if (_definition.HasOption(name) == false)
            {
                throw new RuntimeException(String.Format("The option \"{0}\" does not exist", name));
            }

            InputOption option = _definition.GetOption(name);


            if (option.AcceptsValue() == false && val != null)
            {
                throw new RuntimeException(String.Format("The option \"{0}\" does not accept a value", option.Name));
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
                    throw new RuntimeException(String.Format("The option {0} requires a value", name));
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

        public void SetOption(string name, string value)
        {
            if (!_definition.HasOption(name))
            {
                throw new InvalidArgumentException(String.Format("The \"{0}\" option does not exist", name));
            }

            _options[name] = value;
        }

        public void SetArgument(string name, string value)
        {
            if (!_definition.HasArgument(name))
            {
                throw new InvalidArgumentException(String.Format("The \"{0}\" argument does not exist", name));
            }

            _arguments[name] = value;
        }

        public bool HasOption(string name)
        {
            return _options.ContainsKey(name);
        }

        public bool HasArgument(string name)
        {
            return _arguments.ContainsKey(name);
        }

        public string GetFirstArgument()
        {
            foreach (string token in _tokens)
            {
                if (!String.IsNullOrEmpty(token) && token.Substring(0, 1) == "-")
                {
                    continue;
                }

                return token;
            }

            return null;
        }

        public bool HasParameterOption(string value, bool onlyParams = false)
        {
            foreach (string token in _tokens)
            {
                if (onlyParams && token == "--")
                {
                    return false;
                }

                if (token == value || token.IndexOf(value + "=") == 0)
                {
                    return true;
                }

            }

            return false;
        }

        public string GetParameterOption(string value, string defaultValue = null, bool onlyParams = false)
        {
            for (int i = 0; i < _tokens.Length; i++)
            {
                string token = _tokens[i];
                if (onlyParams && token == "--")
                {
                    return null;
                }
                
                int position = token.IndexOf(value);
                if (token == value || position == 0)
                {
                    int equalsPosition = token.IndexOf("=");
                    if (equalsPosition != -1)
                    {
                        return token.Substring(equalsPosition + 1);
                    }

                    return _tokens[i + 1];
                }

            }

            return defaultValue;
        }
    }

}