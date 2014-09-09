using System;
using System.Collections.Generic;
using System.IO;
using Snowsoft.SnowflakeScript.Lexing;
using Snowsoft.SnowflakeScript.Parsing;
using Snowsoft.SnowflakeScript.CodeGeneration;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptEngine
	{
		ScriptLexer lexer;
		ScriptParser parser;
		CodeGenerator codeGenerator;
		CodeCompiler codeCompiler;

		Dictionary<string, dynamic> globals;

		public ScriptEngine()
		{
			this.lexer = new ScriptLexer();
			this.parser = new ScriptParser();
			this.codeGenerator = new CodeGenerator();
			this.codeCompiler = new CodeCompiler();

			this.globals = new Dictionary<string, dynamic>();
		}

		internal dynamic GetGlobalVariableIntern(string name)
		{
			if (this.globals.ContainsKey(name))
				return this.globals[name];

			return ScriptUndefined.Value;
		}

		public void SetGlobalVariable<T>(string name, T value)
		{			
			this.globals[name] = value;
		}

		#region SetGlobalFunction

		public void SetGlobalFunction(string name, Action value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1>(string name, Action<T1> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2>(string name, Action<T1, T2> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3>(string name, Action<T1, T2, T3> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4>(string name, Action<T1, T2, T3, T4> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5>(string name, Action<T1, T2, T3, T4, T5> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6>(string name, Action<T1, T2, T3, T4, T5, T6> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7>(string name, Action<T1, T2, T3, T4, T5, T6, T7> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<TResult>(string name, Func<TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, TResult>(string name, Func<T1, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, TResult>(string name, Func<T1, T2, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, TResult>(string name, Func<T1, T2, T3, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, TResult>(string name, Func<T1, T2, T3, T4, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, TResult>(string name, Func<T1, T2, T3, T4, T5, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> value)
		{
			this.SetGlobalVariable(name, value);
		}

		#endregion

		public string GenerateCode(string script)
		{
			var lexemes = this.lexer.Lex(script);
			var syntaxTree = this.parser.Parse("Compiled", lexemes);
			return this.codeGenerator.Generate(syntaxTree);
		}

        public object Execute(string script)
        {
			return this.Execute("Compiled", script);
        }

		public object ExecuteFile(string fileName)
		{
			string script = File.ReadAllText(fileName);
			return this.Execute(Path.GetFileNameWithoutExtension(fileName), script);
		}

		private object Execute(string id, string script)
        {
			if (string.IsNullOrEmpty(id))
			{
				id = "Compiled";
			}

            var lexemes = this.lexer.Lex(script);
            var syntaxTree = this.parser.Parse(id, lexemes);
			var code = this.codeGenerator.Generate(syntaxTree);
			var compiled = this.codeCompiler.Compile(id, code);

			compiled.Id = id;
			compiled.Engine = this;
			return compiled.Execute();
        }
	}
}
