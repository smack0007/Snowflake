using System;
using System.Collections.Generic;
using System.IO;
using Snowflake.Execution;
using Snowflake.Lexing;
using Snowflake.Parsing;

namespace Snowflake
{
	public class ScriptEngine : IScriptExecutionContext
	{
		ScriptLexer lexer;
		ScriptParser parser;
        ScriptExecuter executer;
		
        ScriptExecutionContext executionContext;

        public dynamic this[string name]
        {
            get => this.executionContext[name];
            set => this.executionContext[name] = value;
        }

        public ScriptEngine()
		{
			this.lexer = new ScriptLexer();
			this.parser = new ScriptParser();
            this.executer = new ScriptExecuter();

            this.executionContext = new ScriptExecutionContext();
		}

        public dynamic GetGlobalVariable(string name)
        {
            return this.executionContext.GetGlobalVariable(name);
        }

        public void SetGlobalVariable(string name, dynamic value, bool isConst = false)
        {
            this.executionContext.SetGlobalVariable(name, value, isConst);
        }

        public void RegisterType(string name, Type type)
        {
            this.executionContext.RegisterType(name, type);
        }
                
		public IList<Lexeme> GetLexemes(string script)
		{
			return this.lexer.Lex(script);
		}

        public dynamic Execute(string script)
        {
            var lexemes = this.lexer.Lex(script);
            var syntaxTree = this.parser.Parse(lexemes);
            
            var result = this.executer.Execute(syntaxTree, this.executionContext);

            return result;
        }

        public dynamic ExecuteFile(string fileName)
		{
			string script = File.ReadAllText(fileName);
			return this.Execute(script);
		}

        public dynamic Execute(Script script)
        {
            if (script == null)
                throw new ArgumentNullException("script");

            return script.Execute(this.executionContext);
        }
    }
}
