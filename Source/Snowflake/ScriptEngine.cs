using System;
using System.Collections.Generic;
using System.IO;
using Snowflake.Lexing;
using Snowflake.Parsing;
using Snowflake.CodeGeneration;

namespace Snowflake
{
	public class ScriptEngine : IScriptExecutionContext
	{
		ScriptLexer lexer;
		ScriptParser parser;
		CodeGenerator codeGenerator;
        CodeCompiler codeCompiler;

        int scriptCount;
        ScriptExecutionContext executionContext;
                
		public ScriptEngine()
		{
			this.lexer = new ScriptLexer();
			this.parser = new ScriptParser();
			this.codeGenerator = new CodeGenerator();
            this.codeCompiler = new CodeCompiler();

            this.executionContext = new ScriptExecutionContext();
		}

        public dynamic GetGlobalVariable(string name)
        {
            return this.executionContext.GetGlobalVariable(name);
        }

        public void SetGlobalVariable(string name, dynamic value)
        {
            this.executionContext.SetGlobalVariable(name, value);
        }

		public IList<Lexeme> GetLexemes(string script)
		{
			return this.lexer.Lex(script);
		}

		public string GenerateCode(string script, string className)
		{
			var lexemes = this.lexer.Lex(script);
			var syntaxTree = this.parser.Parse(lexemes);
			return this.codeGenerator.Generate(syntaxTree, className);
		}

        private string GetNextScriptClassName()
        {
            this.scriptCount++;
            return "Script" + this.scriptCount;
        }

        public Script Compile(string script)
        {
            return this.Compile(script, this.GetNextScriptClassName());
        }

        public Script Compile(string script, string className)
        {
            var lexemes = this.lexer.Lex(script);
            var syntaxTree = this.parser.Parse(lexemes);
            var code = this.codeGenerator.Generate(syntaxTree, className);
            return this.codeCompiler.Compile(code, className);
        }

        public dynamic Execute(string script)
        {
            var lexemes = this.lexer.Lex(script);
            var syntaxTree = this.parser.Parse(lexemes);

            string className = this.GetNextScriptClassName();
            var code = this.codeGenerator.Generate(syntaxTree, className);
            var compiled = this.codeCompiler.Compile(code, className);
            return compiled.Execute(this.executionContext);
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
