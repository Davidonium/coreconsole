using System;
using Xunit;
using CoreConsole.Input;

namespace CoreConsole.Tests
{

    public class InputTest
    {
        [Fact]
        public void TestOptionParsedCorrectly()
        {
            string[] args = new string[] {
                "--first",
                "the first"
            };

            InputDefinition reqs = new InputDefinition();
            reqs.AddOption(new InputOption("first", "f", InputOptionValueMode.Required));
            
            Assert.NotNull(reqs.GetOption("first"));
            
            BaseInput input = new BaseInput(args, reqs);

            input.Parse();
            Assert.True(input.HasOption("first"), "The input is missing the option first");
            Assert.Equal(input.GetOption("first"), "the first");
        }
    }
}
