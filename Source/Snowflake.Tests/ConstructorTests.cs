using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Tests.Types;
using Xunit;

namespace Snowflake.Tests
{
    public class ConstructorTests : LanguageTestFixture
    {
        [Fact]
        public void Unknown_Constructor_Call_Is_Error()
        {
            AssertScriptIsException<ScriptExecutionException>("return new Person();");
        }

        [Fact]
        public void Known_Parameterless_Constructor_Can_Be_Called()
        {
            AssertScriptReturnValue<Person>(
                (x) =>
                {
                    Assert.IsType(typeof(Person), x);
                },
                (x) =>
                {
                    x.RegisterType("Person", typeof(Person));
                },
                "return new Person();");
        }

        [Fact]
        public void Known_Constructor_With_Parameters_Can_Be_Called()
        {
            AssertScriptReturnValue<Person>(
                (x) =>
                {
                    Assert.IsType(typeof(Person), x);
                    Assert.Equal("Bob", x.FirstName);
                    Assert.Equal("Freeman", x.LastName);
                },
                (x) =>
                {
                    x.RegisterType("Person", typeof(Person));
                },
                "return new Person(\"Bob\", \"Freeman\");");
        }
    }
}
