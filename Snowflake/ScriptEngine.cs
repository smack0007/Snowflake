using System;
using System.Collections.Generic;
using System.IO;
using Snowsoft.SnowflakeScript.Lexing;
using Snowsoft.SnowflakeScript.Parsing;
using Snowsoft.SnowflakeScript.Execution;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptEngine
	{
		ILexer lexer;
		IParser parser;
		IExecutor executor;

		ScriptNode script;
		VariableStack stack;

		public ScriptEngine()
			: this(new Lexer(), new Parser(), new Executor())
		{
		}

		public ScriptEngine(ILexer lexer, IParser parser, IExecutor executor)
		{
			if (lexer == null)
			{
				throw new ArgumentNullException("lexer");
			}

			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}

			if (executor == null)
			{
				throw new ArgumentNullException("executor");
			}

			this.lexer = lexer;
			this.parser = parser;
			this.executor = executor;

			this.stack = new VariableStack();
		}

		/// <summary>
		/// Loads a script from a file.
		/// </summary>
		/// <param name="filename">The filename to read from.</param>
		/// <returns>Script</returns>
		public void LoadFromFile(string fileName)
		{
			string text = File.ReadAllText(fileName);
			this.LoadFromString(text);
		}

		/// <summary>
		/// Loads a script from a string.
		/// </summary>
		/// <param name="script">The script text.</param>
		/// <returns>Script</returns>
		public void LoadFromString(string script)
		{
			this.script = this.parser.Parse(this.lexer.Lex(script));
		}

		public void Run()
		{
			if (this.script == null)
				throw new InvalidOperationException("No Script currently loaded.");

			this.executor.CallFunc(this.script, "Main", null, this.stack);
		}
	}
}
