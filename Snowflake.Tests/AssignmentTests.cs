using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
	[TestFixture]
	public class AssignmentTests : LanguageTestFixture
	{
		[Test]
		public void Assigned_Variable_Gets_New_Value()
		{
			AssertScriptReturnValue(42, @"
var x = 21;
x = 42;
return x;");
		}

		[Test]
		public void Add_And_Assign_Variable_Gets_New_Value()
		{
			AssertScriptReturnValue(42, @"
var x = 21;
x += 21;
return x;");
		}

		[Test]
		public void Subtract_And_Assign_Variable_Gets_New_Value()
		{
			AssertScriptReturnValue(42, @"
var x = 63;
x -= 21;
return x;");
		}

		[Test]
		public void Multiply_And_Assign_Variable_Gets_New_Value()
		{
			AssertScriptReturnValue(42, @"
var x = 21;
x *= 2;
return x;");
		}

		[Test]
		public void Divide_And_Assign_Variable_Gets_New_Value()
		{
			AssertScriptReturnValue(42, @"
var x = 84;
x /= 2;
return x;");
		}
	}
}
