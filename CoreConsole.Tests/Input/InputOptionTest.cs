using System;
using Xunit;
using CoreConsole.Input;

namespace CoreConsole.Tests
{

    public class InputOptionTest
    {
        [Fact]
        public void TestInputOptionValues()
        {
            InputOption longOption = new InputOption("--foo");

            Assert.Equal("foo", longOption.Name);


            InputOption shortOption = new InputOption("--foo", "-f");

            Assert.Equal("f", shortOption.Shortcut);

        }
    }
}
