using System;
using NUnit.Framework;
using Snowsoft.SnowflakeScript;

namespace Snowflake.Tests
{
	/// <summary>
	/// Contains tests which should cause ScriptParserException(s).
	/// </summary>
	[TestFixture]
	public class ScriptParserErrorTests
	{
		private void ParseScript(string script)
		{
			ScriptParser parser = new ScriptParser();
			parser.Parse(script);
		}

		[Test, ExpectedException(typeof(ScriptParserException))]
		public void Char_WithNoEscapeCodeMoreThanOneChar()
		{
			this.ParseScript("'aa'");
		}

		[Test, ExpectedException(typeof(ScriptParserException))]
		public void Char_WithEscapeCodeMoreThanOneChar()
		{
			this.ParseScript("'\\aa'");
		}

		[Test, ExpectedException(typeof(ScriptParserException))]
		public void UnexpectedEndOfFile_UnclosedChar()
		{
			this.ParseScript("\'a");
		}

		[Test, ExpectedException(typeof(ScriptParserException))]
		public void UnexpectedEndOfFile_UnclosedString()
		{
			this.ParseScript("\"aa");
		}

		[Test, ExpectedException(typeof(ScriptParserException))]
		public void UnexpectedEndOfFile_FloatEndsWithDecimal()
		{
			this.ParseScript("123.");
		}
	}
}
