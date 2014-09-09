using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake;
using Xunit;

namespace Snowflake.Tests
{
	public class FunctionCallTests : LanguageTestFixture
	{
		[Fact]
		public void Function_Call_With_One_Arg()
		{
			AssertScriptReturnValue(42, @"
var doubleIt = func(x) {
	return x + x;
};

return doubleIt(21);");
		}

		[Fact]
		public void Function_Call_With_Two_Args()
		{
			AssertScriptReturnValue(42, @"
var add = func(x, y) {
	return x + y;
};

return add(21, 21);");
		}

		[Fact]
		public void Too_Many_Arguments_To_Script_Function_Is_Error()
		{
			AssertScriptIsException<ScriptExecutionException>(@"
var doubleIt = func(x) {
	return x + x;
};

return doubleIt(21, 21);");
		}

		[Fact]
		public void Default_Value_For_Function_Argument_Taken_When_Not_Provided()
		{
			AssertScriptReturnValue(42, @"
var doubleIt = func(x = 21) {
	return x + x;
};

return doubleIt();");
		}

		[Fact]
        public void Anonymous_Function_Can_Be_Directly_Invoked()
        {
            AssertScriptReturnValue(42, @"
return func(x) {
	return x + x;   
}(21);");
        }

		[Fact]
        public void Anonymous_Function_Returned_From_Anonymous_Function_Can_Be_Directly_Invoked()
        {
            AssertScriptReturnValue(5, @"
return func(x) {
	return func(y) {
        return x + y;   
    };
}(3)(2);");
        }

		[Fact]
        public void Anonymous_Function_Wrapped_In_Parens_Can_Be_Directly_Invoked()
        {
            AssertScriptReturnValue(42, @"
return (func(x) {
	return x + x;   
})(21);");
        }
	}
}
