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
		ScriptLexer lexer;
		ScriptParser parser;
		ScriptExecutor executor;
				
		ScriptStack stack;

		public ScriptEngine()
		{
			this.lexer = new ScriptLexer();
			this.parser = new ScriptParser();
			this.executor = new ScriptExecutor();

			this.stack = new ScriptStack();
		}
		
		public object Execute(string script)
		{
			var scriptNode = this.parser.Parse(this.lexer.Lex(script));
			var scriptObject = this.executor.Execute(scriptNode, stack);
			return scriptObject.GetValue();
		}

		public object ExecuteFile(string fileName)
		{
			string script = File.ReadAllText(fileName);
			return this.Execute(script);
		}
	}
}
