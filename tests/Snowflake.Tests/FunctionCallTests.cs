using Snowflake.Execution;
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
		public void Error_When_Argument_Not_Provided_And_No_Default_Value_Specified()
		{
			AssertScriptIsException<ScriptExecutionException>(@"
var add = func(x = 1, y) {
	return x + y;
};

return add(2);");
		}

        [Fact]
		public void Default_Value_Can_Be_Variable()
		{
			AssertScriptReturnValue(42, @"
var doubleIt = func(x = y) {
	return x + x;
};

var y = 21;

return doubleIt();");
		}

		[Fact]
        public void Function_Can_Be_Directly_Invoked()
        {
            AssertScriptReturnValue(42, @"
return func(x) {
	return x + x;   
}(21);");
        }

        [Fact]
		public void Function_Returned_From_Function_Can_Be_Directly_Invoked()
		{
			AssertScriptReturnValue(42, @"
const foo = func() {
	return func() {
		return 42;
	};
};

return foo()();");
		}

		[Fact]
        public void Function_Returned_From_Function_Can_Capture_Variables()
        {
            AssertScriptReturnValue(5, @"
return func(x) {
	return func(y) {
        return x + y;   
    };
}(3)(2);");
        }

        [Fact]
        public void Function_Returned_From_Function_Returned_From_Function_Can_Capture_Variables()
        {
            AssertScriptReturnValue(6, @"
return func(x) {
	return func(y) {
        return func(z) {
            return x + y + z;
        };
    };
}(3)(2)(1);");
        }

        [Fact]
        public void Function_Wrapped_In_Parens_Can_Be_Directly_Invoked()
        {
            AssertScriptReturnValue(42, @"
return (func(x) {
	return x + x;   
})(21);");
        }

        [Fact]
        public void Global_Function_Can_Call_Other_Global_Function()
        {
            AssertScriptReturnValue(42, @"
const foo = func(x) {
    return x * 4 + bar(x);
};

const bar = func(x) {
    return x * 2;
};

return foo(7);");
        }
	}
}
