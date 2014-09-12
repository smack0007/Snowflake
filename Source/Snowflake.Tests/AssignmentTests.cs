using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Tests.Types;
using Xunit;

namespace Snowflake.Tests
{
	public class AssignmentTests : LanguageTestFixture
	{
		[Fact]
		public void Assigned_Variable_Gets_New_Value()
		{
			AssertScriptReturnValue(42, @"
var x = 21;
x = 42;
return x;");
		}

		[Fact]
		public void Add_And_Assign_Variable_Gets_New_Value()
		{
			AssertScriptReturnValue(42, @"
var x = 21;
x += 21;
return x;");
		}

		[Fact]
		public void Subtract_And_Assign_Variable_Gets_New_Value()
		{
			AssertScriptReturnValue(42, @"
var x = 63;
x -= 21;
return x;");
		}

		[Fact]
		public void Multiply_And_Assign_Variable_Gets_New_Value()
		{
			AssertScriptReturnValue(42, @"
var x = 21;
x *= 2;
return x;");
		}

		[Fact]
		public void Divide_And_Assign_Variable_Gets_New_Value()
		{
			AssertScriptReturnValue(42, @"
var x = 84;
x /= 2;
return x;");
		}

		[Fact]
		public void Assignment_Can_Be_Chained()
		{
			AssertScriptReturnValue(true, @"
var x;
var y;
x = y = 42;
return x == y;");
		}

		[Fact]
		public void Assignment_Of_Object_Members()
		{
			AssertScriptReturnValue<Person>(
				assert: (x) =>
				{
					Assert.IsType(typeof(Person), x);
					Assert.Equal("Bob", ((Person)x).FirstName);
					Assert.Equal("Freeman", ((Person)x).LastName);
					Assert.Equal(42, ((Person)x).Age);
				},
				setup: (x) =>
				{
					x.SetGlobalFunction("createPerson", () => new Person());
				},
				script: @"
var person = createPerson();
person.FirstName = ""Bob"";
person.LastName = ""Freeman"";
person.Age = 42;
return person;");
		}
	}
}
