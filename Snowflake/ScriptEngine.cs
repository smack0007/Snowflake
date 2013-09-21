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
		ScriptTypeBoxer boxer;
		ScriptStack stack;

		public ScriptEngine()
		{
			this.lexer = new ScriptLexer();
			this.parser = new ScriptParser();
			this.executor = new ScriptExecutor();
			this.boxer = new ScriptTypeBoxer();
			this.stack = new ScriptStack();
		}

		public void SetGlobalVariable(string name, object value)
		{			
			this.stack.Globals[name] = new ScriptVariableReference(this.boxer.Box(value));
		}

		#region SetGlobalFunction

		public void SetGlobalFunction(string name, Action value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1>(string name, Action<T1> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2>(string name, Action<T1, T2> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3>(string name, Action<T1, T2, T3> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4>(string name, Action<T1, T2, T3, T4> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5>(string name, Action<T1, T2, T3, T4, T5> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6>(string name, Action<T1, T2, T3, T4, T5, T6> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7>(string name, Action<T1, T2, T3, T4, T5, T6, T7> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<TResult>(string name, Func<TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, TResult>(string name, Func<T1, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, TResult>(string name, Func<T1, T2, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, TResult>(string name, Func<T1, T2, T3, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, TResult>(string name, Func<T1, T2, T3, T4, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, TResult>(string name, Func<T1, T2, T3, T4, T5, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		public void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> value)
		{
			this.SetGlobalVariable(name, (object)value);
		}

		#endregion

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
