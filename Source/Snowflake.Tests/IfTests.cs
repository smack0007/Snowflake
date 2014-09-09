using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
	[TestFixture]
	public class IfTests : LanguageTestFixture
	{
		[Test]
		public void If_Expression_Which_Evaluates_To_True_Is_Executed()
		{
			AssertScriptReturnValue(42, @"
if (true) {
	return 42;
}

return 0;
");
		}

		[Test]
		public void If_Expression_Which_Evaluates_To_False_Is_Not_Executed()
		{
			AssertScriptReturnValue(0, @"
if (false) {
	return 42;
}

return 0;
");
		}

		[Test]
		public void If_Else_Block_Is_Executed_When_Expression_Evaluates_To_False()
		{
			AssertScriptReturnValue(21, @"
var result = 0;

if (false) {
	result = 42;
} else {
	result = 21;
}

return result;
");
		}

		[Test]
		public void If_Else_Block_Is_Not_Executed_When_Expression_Evaluates_To_False()
		{
			AssertScriptReturnValue(42, @"
var result = 0;

if (true) {
	result = 42;
} else {
	result = 21;
}

return result;
");
		}

		[Test]
		public void If_Expression_With_Variable_Which_Evaluates_To_True_Is_Executed()
		{
			AssertScriptReturnValue(42, @"
var test = true;

if (test) {
	return 42;
}

return 0;
");
		}

		[Test]
		public void If_Expression_With_Variable_Which_Evaluates_To_False_Is_Not_Executed()
		{
			AssertScriptReturnValue(0, @"
var test = false;

if (test) {
	return 42;
}

return 0;
");
		}
	}
}
