using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
	[TestFixture]
	public class FunctionCallTests : LanguageTestFixture
	{
		[Test]
		public void Function_Call_With_One_Arg()
		{
			AssertScriptReturnValue(42, @"
var doubleIt = func(var x) {
	return x + x;
};

return doubleIt(21);");
		}

		[Test]
		public void Function_Call_With_Two_Args()
		{
			AssertScriptReturnValue(42, @"
var add = func(var x, var y) {
	return x + y;
};

return add(21, 21);");
		}

		[Test]
		public void Too_Many_Arguments_To_Script_Function_Is_Error()
		{
			AssertScriptIsExecutionException(@"
var doubleIt = func(var x) {
	return x + x;
};

return doubleIt(21, 21);");
		}

		[Test]
		public void Function_With_No_Return_Statement_Returns_Undefined()
		{
			AssertScriptReturnValue(true, @"
var doIt = func() { };

return doIt() == undef;");
		}

		[Test]
		public void Default_Value_For_Function_Argument_Taken_When_Not_Provided()
		{
			AssertScriptReturnValue(42, @"
var doubleIt = func(var x = 21) {
	return x + x;
};

return doubleIt();");
		}
	}
}
