using System;
using System.Collections.Generic;
using System.IO;
using Snowsoft.SnowflakeScript.Lexer;
using Snowsoft.SnowflakeScript.Parser;
using Snowsoft.SnowflakeScript.Executor;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptEngine
	{
		IScriptLexer lexer;

		Script script;

		public ScriptEngine()
			: this(new ScriptLexer())
		{
		}

		public ScriptEngine(IScriptLexer parser)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}

			this.lexer = parser;
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
			this.script = new Script(this.lexer.Lex(script));
		}

		public void Run()
		{
			if (this.script == null)
				throw new InvalidOperationException("No Script currently loaded.");

			this.script.CallFunc("Main");
		}
	}
}
