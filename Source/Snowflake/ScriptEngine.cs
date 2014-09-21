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

		public string GenerateCode(string script)
		{
			var lexemes = this.lexer.Lex(script);
			var syntaxTree = this.parser.Parse("Compiled", lexemes);
			return this.codeGenerator.Generate(syntaxTree);
		}

        public dynamic Execute(string script)
        {
			return this.Execute("Compiled", script);
        }

        public dynamic ExecuteFile(string fileName)
		{
			string script = File.ReadAllText(fileName);
			return this.Execute(Path.GetFileNameWithoutExtension(fileName), script);
		}

        private dynamic Execute(string id, string script)
        {
			if (string.IsNullOrEmpty(id))
			{
				id = "Compiled";
			}

            var lexemes = this.lexer.Lex(script);
            var syntaxTree = this.parser.Parse(id, lexemes);
			var code = this.codeGenerator.Generate(syntaxTree);

            Script compiled = this.codeCompiler.Compile(id, code);
            compiled.Id = id;
            return compiled.Execute(this.executionContext);
        }
	}
}
