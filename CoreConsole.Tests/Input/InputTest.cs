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
        public void TestGetOptionSuccess(string[] args)
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

        [Theory, MemberData("ParameterOptionValues")]
        public void TestGetParameterOptionSuccess(string[] args, string key, bool onlyParams, string expected)
        {
            BaseInput input = new BaseInput(args);
            Assert.Equal(input.GetParameterOption(key, null, onlyParams), expected);
        }

        public static IEnumerable<object[]> ParameterOptionValues
        {
            get
            {
                return new[]
                {
                    new object[] { new string[] { "app/console", "foo:bar", "-e", "dev" }, "-e", false, "dev" },
                    new object[] { new string[] { "app/console", "foo:bar", "--env=dev" }, "--env", false, "dev" },
                    new object[] { new string[] { "app/console", "foo:bar", "--env", "dev", "--lul=Kreygasm" }, "--lul", false, "Kreygasm" },
                    new object[] { new string[] { "app/console", "foo:bar", "--env", "dev", "--roflcopter" }, "--env", false, "dev" },
                    new object[] { new string[] { "app/console", "foo:bar", "--env", "dev" }, "--env", false, "dev" },
                    new object[] { new string[] { "app/console", "foo:bar", "--", "--env", "dev" }, "--env", false, "dev" },
                    new object[] { new string[] { "app/console", "foo:bar", "--", "--env", "dev" }, "--env", true, null },
                };
            }
        }

        [Fact]
        public void TestHasParameterOptionSuccess()
        {
            BaseInput input = new BaseInput(new string[] { "kappa", "-k", "keepo" });
            Assert.True(input.HasParameterOption("-k"));
            input = new BaseInput(new string[] { "kappa", "--kappaross", "keepo" });
            Assert.True(input.HasParameterOption("--kappaross"));
            Assert.False(input.HasParameterOption("--roflcopter"));

            input = new BaseInput(new string[] { "kappa", "--kappaross=keepo" });
            Assert.True(input.HasParameterOption("--kappaross"));
        }

    }

}
