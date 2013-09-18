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
		ScriptObjectBoxer boxer;

		ScriptStack stack;

		public ScriptEngine()
		{
			this.lexer = new ScriptLexer();
			this.parser = new ScriptParser();
			this.executor = new ScriptExecutor();
			this.boxer = new ScriptObjectBoxer();

			this.stack = new ScriptStack();
		}

		public void SetGlobalVariable(string name, object value)
		{			
			// TODO: Ensure stack frame 0 is always used.
			this.stack[name] = new ScriptVariableReference(this.boxer.Box(value));
		}
		
		public object Execute(string script)
		{
			var scriptNode = this.parser.Parse(this.lexer.Lex(script));
			var scriptObject = this.executor.Execute(scriptNode, this.stack, this.boxer);
			return scriptObject.Unbox();
		}

		public object ExecuteFile(string fileName)
		{
			string script = File.ReadAllText(fileName);
			return this.Execute(script);
		}
	}
}
