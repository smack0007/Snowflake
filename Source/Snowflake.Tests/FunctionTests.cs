using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.CodeGeneration;

namespace Snowflake.Tests
{
	[TestFixture]
	public class FunctionTests : LanguageTestFixture
	{
		[Test]
		public void Declared_Function_Cannot_Be_Overwritten_By_Varaible()
		{
			AssertScriptIsException<CodeGenerationException>(@"
func foo() {
	return 42;
}

var foo = 12;");
		}

		[Test]
		public void Function_Declaration_Cannnot_Overwrite_Variable()
		{
			AssertScriptIsException<CodeGenerationException>(@"
var foo = 12;

func foo() {
	return 42;
}
");
		}

		[Test]
		public void Named_Function_Can_Be_Assigned_To_Varaible()
		{
			AssertScriptReturnValue(42, @"
func foo() {
	return 42;
}

var bar = foo;

return bar();
");
		}

		[Test]
		public void Named_Function_Can_Still_Be_Called_After_Changing_Variable_Value()
		{
			AssertScriptReturnValue(42, @"
func foo() {
	return 42;
}

var bar = foo;

var result = bar();

bar = 12;

return foo();
");
		}
	}
}
