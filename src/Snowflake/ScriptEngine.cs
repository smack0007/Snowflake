using System;
using System.Collections.Generic;
using System.IO;
using Snowflake.Lexing;
using Snowflake.Parsing;

namespace Snowflake
{
	public class ScriptEngine : IScriptExecutionContext
	{
		ScriptLexer lexer;
		ScriptParser parser;
		
        int scriptCount;
        ScriptExecutionContext executionContext;
                
		public ScriptEngine()
		{
			this.lexer = new ScriptLexer();
			this.parser = new ScriptParser();

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
            
            this.executionContext.PushStackFrame("<script>");
            dynamic result = null;
            //var result = compiled.Execute(this.executionContext);
            this.executionContext.PopStackFrame();

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
