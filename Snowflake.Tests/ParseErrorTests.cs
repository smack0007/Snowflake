using System;
using NUnit.Framework;
using Snowsoft.SnowflakeScript;

namespace Snowflake.Tests
{
	[TestFixture]
	public class ParseErrorTests
	{
		private Script CreateScript(string script)
		{
			return Script.FromString(script);
		}

		[Test, ExpectedException(typeof(ScriptException))]
		public void Char_WithNoEscapeCodeMoreThanOneChar()
		{
			this.CreateScript("'aa'");
		}

		[Test, ExpectedException(typeof(ScriptException))]
		public void Char_WithEscapeCodeMoreThanOneChar()
		{
			this.CreateScript("'\\aa'");
		}

		[Test, ExpectedException(typeof(ScriptException))]
		public void UnexpectedEndOfFile_UnclosedChar()
		{
			this.CreateScript("\'a");
		}

		[Test, ExpectedException(typeof(ScriptException))]
		public void UnexpectedEndOfFile_UnclosedString()
		{
			this.CreateScript("\"aa");
		}

		[Test, ExpectedException(typeof(ScriptException))]
		public void UnexpectedEndOfFile_FloatEndsWithDecimal()
		{
			this.CreateScript("123.");
		}
	}
}
