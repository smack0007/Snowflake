using System;
using NUnit.Framework;
using Snowsoft.SnowflakeScript;
using Snowsoft.SnowflakeScript.Lexer;

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
			ScriptLexer parser = new ScriptLexer();
			parser.Lex(script);
		}

		[Test, ExpectedException(typeof(ScriptLexerException))]
		public void Char_WithNoEscapeCodeMoreThanOneChar()
		{
			this.ParseScript("'aa'");
		}

		[Test, ExpectedException(typeof(ScriptLexerException))]
		public void Char_WithEscapeCodeMoreThanOneChar()
		{
			this.ParseScript("'\\aa'");
		}

		[Test, ExpectedException(typeof(ScriptLexerException))]
		public void UnexpectedEndOfFile_UnclosedChar()
		{
			this.ParseScript("\'a");
		}

		[Test, ExpectedException(typeof(ScriptLexerException))]
		public void UnexpectedEndOfFile_UnclosedString()
		{
			this.ParseScript("\"aa");
		}

		[Test, ExpectedException(typeof(ScriptLexerException))]
		public void UnexpectedEndOfFile_FloatEndsWithDecimal()
		{
			this.ParseScript("123.");
		}
	}
}
