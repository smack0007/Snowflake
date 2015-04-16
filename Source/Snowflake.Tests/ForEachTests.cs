using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.CodeGeneration;
using Snowflake.Parsing;
using Xunit;

namespace Snowflake.Tests
{
	public class ForEachTests : LanguageTestFixture
	{
		[Fact]
		public void Array_Can_Be_Iterated()
		{
			AssertScriptReturnValue(6, @"
var array = [| 1, 2, 3 |];
var sum = 0;
foreach (var x in array) {
	sum += x;
}
return sum;
");
		}

		[Fact]
		public void List_Can_Be_Iterated()
		{
			AssertScriptReturnValue(6, @"
var list = [ 1, 2, 3 ];
var sum = 0;
foreach (var x in list) {
	sum += x;
}
return sum;
");
		}

		[Fact]
		public void Iterate_Empty_List_Is_Never_Executed()
		{
			AssertScriptReturnValue(0, @"
var list = [ ];
var sum = 0;
foreach (var x in list) {
	sum += 1;
}
return sum;
");
		}

		[Fact]
		public void Native_List_Can_Be_Iterated()
		{
			AssertScriptReturnValue(
				(engine) =>
				{
					engine.SetGlobalVariable("list", new List<int>() { 1, 2, 3 });
				},
                6,
@"
var sum = 0;
foreach (var x in list) {
	sum += x;
}
return sum;
");
		}

		[Fact]
		public void List_Can_Be_Delared_As_Source_Expression()
		{
			AssertScriptReturnValue(6, @"
var sum = 0;
foreach (var x in [ 1, 2, 3 ]) {
	sum += x;
}
return sum;
");
		}

		[Fact]
		public void Array_Can_Be_Delared_As_Source_Expression()
		{
			AssertScriptReturnValue(6, @"
var sum = 0;
foreach (var x in [| 1, 2, 3 |]) {
	sum += x;
}
return sum;
");
		}

		[Fact]
		public void VariableDeclaration_With_Value_Is_Syntax_Error()
		{
			AssertScriptIsException<SyntaxException>(@"
var list = [ 1, 2, 3 ];
var sum = 0;
foreach (var x = 5 in list) {
	sum += x;
}
return sum;
");
		}

		[Fact]
		public void VariableDeclaration_With_Same_Name_In_Frame_Is_CodeGeneration_Error()
		{
			AssertScriptIsException<CodeGenerationException>(@"
var list = [ 1, 2, 3 ];
var x = 0;
foreach (var x in list) {
	x += x;
}
return x;
");
		}

		[Fact]
		public void VariableDeclaration_With_Same_Name_In_Parent_Frame_Is_Not_CodeGeneration_Error()
		{
			AssertScriptReturnValue(6, @"
var list = [ 1, 2, 3 ];
var x = 42;

func sum(input) {
	var result = 0;
	foreach (var x in input) {
		result += x;
	}	
	return result;
}

return sum(list);
");
		}
	}
}
