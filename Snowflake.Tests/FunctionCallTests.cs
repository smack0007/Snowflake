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
var doubleIt = func(x) {
	return x + x;
};

return doubleIt(21);");
		}

		[Test]
		public void Function_Call_With_Two_Args()
		{
			AssertScriptReturnValue(42, @"
var add = func(x, y) {
	return x + y;
};

return add(21, 21);");
		}

		[Test]
		public void Too_Many_Arguments_To_Script_Function_Is_Error()
		{
			AssertScriptIsExecutionException(@"
var doubleIt = func(x) {
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
var doubleIt = func(x = 21) {
	return x + x;
};

return doubleIt();");
		}

        [Test]
        public void Anonymous_Function_Can_Be_Directly_Invoked()
        {
            AssertScriptReturnValue(42, @"
return func(x) {
	return x + x;   
}(21);");
        }

        [Test]
        public void Anonymous_Function_Returned_From_Anonymous_Function_Can_Be_Directly_Invoked()
        {
            AssertScriptReturnValue(5, @"
return func(x) {
	return func(y) {
        return x + y;   
    };
}(3)(2);");
        }

        [Test]
        public void Anonymous_Function_Wrapped_In_Parens_Can_Be_Directly_Invoked()
        {
            AssertScriptReturnValue(42, @"
return (func(x) {
	return x + x;   
})(21);");
        }
	}
}
