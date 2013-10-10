using NUnit.Framework;
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
			AssertScriptReturnValue("Hellotrue", "return \"Hello\" + true;");
		}

		[Test]
		public void Add_String_And_Int()
		{
			AssertScriptReturnValue("Hello42", "return \"Hello\" + 42;");
		}

		[Test]
		public void Add_String_And_Float()
		{
			AssertScriptReturnValue("Hello1.1", "return \"Hello\" + 1.1;");
		}
	}
}
