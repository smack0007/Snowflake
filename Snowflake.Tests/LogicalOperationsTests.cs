using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
	[TestFixture]
	public class LogicalOperationsTests : LanguageTestFixture
	{
		[Test]
		public void True_Equal_To_True()
		{
			AssertScriptReturnValue(true, "return true == true;");
		}

		[Test]
		public void True_Not_Equal_To_False()
		{
			AssertScriptReturnValue(true, "return true != false;");
		}

		[Test]
		public void True_And_True_Is_True()
		{
			AssertScriptReturnValue(true, "return true && true;");
		}

		[Test]
		public void True_And_False_Is_False()
		{
			AssertScriptReturnValue(false, "return true && false;");
		}

		[Test]
		public void False_And_True_Is_False()
		{
			AssertScriptReturnValue(false, "return false && true;");
		}

		[Test]
		public void False_And_False_Is_False()
		{
			AssertScriptReturnValue(false, "return false && false;");
		}

		[Test]
		public void True_Or_True_Is_True()
		{
			AssertScriptReturnValue(true, "return true || true;");
		}

		[Test]
		public void True_Or_False_Is_True()
		{
			AssertScriptReturnValue(true, "return true || false;");
		}

		[Test]
		public void False_Or_True_Is_True()
		{
			AssertScriptReturnValue(true, "return false || true;");
		}

		[Test]
		public void False_Or_False_Is_False()
		{
			AssertScriptReturnValue(false, "return false || false;");
		}

		[Test]
		public void Variable_True_And_Variable_True_Is_True()
		{
			AssertScriptReturnValue(true, "var x = true; var y = true; return x && y;");
		}
	}
}
