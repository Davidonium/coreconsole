using System;
using Xunit;
using CoreConsole.Input;
using System.Collections.Generic;
using Xunit.Extensions;

namespace CoreConsole.Tests
{

    public class InputTest
    {
        [Theory, MemberData("OneRequiredOption")]
        public void TestOptionParsedSuccess(string[] args)
        {
            InputDefinition reqs = new InputDefinition();
            reqs.AddOption(new InputOption("first", "f", InputOptionValueMode.Required));
            
            Assert.NotNull(reqs.GetOption("first"));
            
            BaseInput input = new BaseInput(args, reqs);

            input.Parse();
            Assert.True(input.HasOption("first"), "The input is missing the option first");
            Assert.Equal(input.GetOption("first"), "the first");
        }

        public static IEnumerable<object[]> OneRequiredOption
        {
            get
            {
                return new[]
                {
                    new object[] { new string[] { "--first", "the first" } },
                    new object[] { new string[] { "-f", "the first" } },
                };
            }
        }
    }
    
}
