﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
	[TestFixture]
	public class StringOperationsTests : LanguageTestFixture
	{
		[Test]
		public void Add_String_And_String()
		{
			AssertScriptReturnValue("Hello World!", "return \"Hello\" + \" World!\";");
		}

		[Test]
		public void Add_String_And_Character()
		{
			AssertScriptReturnValue("HelloC", "return \"Hello\" + 'C';");
		}

		[Test]
		public void Add_String_And_Bool()
		{
			string trueString = true.ToString();
			AssertScriptReturnValue("Hello" + trueString, "return \"Hello\" + true;");
		}

		[Test]
		public void Add_String_And_Int()
		{
			AssertScriptReturnValue("Hello42", "return \"Hello\" + 42;");
		}

		[Test]
		public void Add_String_And_Float()
		{
			string floatString = (1.1f).ToString();
			AssertScriptReturnValue("Hello" + floatString, "return \"Hello\" + 1.1;");
		}
	}
}