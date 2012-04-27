using System;
using System.Collections.Generic;
using System.IO;
using Snowsoft.SnowflakeScript.Lexer;
using Snowsoft.SnowflakeScript.Parser;

namespace Snowsoft.SnowflakeScript.Executor
{	
	public class Script
	{
		class FuncInfo
		{
			public string Name;
			public int Location;
			public string[] Args;
			public int EntryLocation;
			public int ExitLocation;
		}

		IList<ScriptLexeme> lexemes;
		Dictionary<string, FuncInfo> funcs;
		ScriptVariableStack variableStack;
		
		/// <summary>
		/// Creates a new script based on the given lexemes.
		/// </summary>
		public Script(IList<ScriptLexeme> lexemes)
		{
			if (lexemes == null)
			{
				throw new ArgumentNullException("lexemes");
			}

			this.lexemes = lexemes;
			this.funcs = new Dictionary<string, FuncInfo>();
			this.variableStack = new ScriptVariableStack();
			
			for (int pos = 0; pos < this.lexemes.Count; pos++)
			{
				if (this.lexemes[pos].Type == ScriptLexemeType.Func)
				{
					FuncInfo funcInfo = this.ParseFunc(ref pos);
					this.funcs.Add(funcInfo.Name, funcInfo);
				}
			}
		}
				
		private void EnsureLexemeType(ScriptLexemeType expected, int pos)
		{
			if (this.lexemes[pos].Type != expected)
				throw new ScriptSyntaxException(expected + " was expected at Line " + this.lexemes[pos].Line + " Column " + this.lexemes[pos].Column + ".");
		}

		private void EnsureNotLexemeType(ScriptLexemeType unexpected, int pos)
		{
			if (this.lexemes[pos].Type == unexpected)
				throw new ScriptSyntaxException(unexpected + " was not expected at Line " + this.lexemes[pos].Line + " Column " + this.lexemes[pos].Column + ".");
		}

		/// <summary>
		/// Moves "pos" from OpenBrace to the matching CloseBrace.
		/// </summary>
		/// <param name="pos"></param>
		private void MoveToMatchingBrace(ref int pos)
		{
			this.EnsureLexemeType(ScriptLexemeType.OpenBrace, pos);

			int level = 1;
			while (level > 0 && this.lexemes[pos].Type != ScriptLexemeType.EOF)
			{
				pos++;
				if (this.lexemes[pos].Type == ScriptLexemeType.OpenBrace)
				{
					level++;
				}
				else if (this.lexemes[pos].Type == ScriptLexemeType.CloseBrace)
				{
					level--;
				}
			}

			this.EnsureLexemeType(ScriptLexemeType.CloseBrace, pos);
		}

		private FuncInfo ParseFunc(ref int pos)
		{
			FuncInfo funcInfo = new FuncInfo();

			this.EnsureLexemeType(ScriptLexemeType.Func, pos);
			funcInfo.Location = pos;

			pos++;
			this.EnsureLexemeType(ScriptLexemeType.Identifier, pos);

			funcInfo.Name = this.lexemes[pos].Val;

			pos++;
			this.EnsureLexemeType(ScriptLexemeType.OpenParen, pos);

			pos++;
			if (this.lexemes[pos].Type == ScriptLexemeType.Variable)
			{
				List<string> argList = new List<string>();
				
				pos++;
				this.EnsureLexemeType(ScriptLexemeType.Identifier, pos);
				argList.Add(this.lexemes[pos].Val);

				pos++;
				while (this.lexemes[pos].Type == ScriptLexemeType.Comma)
				{
					pos++;
					this.EnsureLexemeType(ScriptLexemeType.Variable, pos);

					pos++;
					this.EnsureLexemeType(ScriptLexemeType.Identifier, pos);
					argList.Add(this.lexemes[pos].Val);

					pos++;
				}

				funcInfo.Args = argList.ToArray();
			}
			else // No args
			{
				funcInfo.Args = new string[0];
			}

			this.EnsureLexemeType(ScriptLexemeType.CloseParen, pos);

			pos++;
			this.EnsureLexemeType(ScriptLexemeType.OpenBrace, pos);
			funcInfo.EntryLocation = pos;

			this.MoveToMatchingBrace(ref pos);

			this.EnsureLexemeType(ScriptLexemeType.CloseBrace, pos);
			funcInfo.ExitLocation = pos;

			return funcInfo;
		}
		
		public ScriptVariable CallFunc(string funcName)
		{
			return this.CallFunc(funcName, null);
		}

		public ScriptVariable CallFunc(string funcName, IList<ScriptVariable> args)
		{
			if (!this.funcs.ContainsKey(funcName))
				throw new ScriptFunctionCallException("No function found by the name " + funcName + ".");

			FuncInfo funcInfo = this.funcs[funcName];
			int pos = funcInfo.EntryLocation;

			this.variableStack.Push();

			if (args != null)
			{
				if (args.Count != funcInfo.Args.Length)
					throw new ScriptFunctionCallException("Invalid number of args specified for " + funcName + ".");

				for (int i = 0; i < args.Count; i++)
				{
					ScriptVariable arg = this.variableStack[funcInfo.Args[i]];
					arg.Gets(args[i]);
				}
			}

			pos++;
			while (pos != funcInfo.ExitLocation)
			{
				Statement(ref pos);
			}

			this.variableStack.Pop();

			return null; // TODO: Implement this.
		}

		private ScriptVariable Statement(ref int pos)
		{
			ScriptVariable variable = null;

			if (lexemes[pos].Type == ScriptLexemeType.Echo)
			{
				Echo(ref pos);
			}
			else if (lexemes[pos].Type == ScriptLexemeType.If)
			{
				DoIf(ref pos);
			}
			else // Must be an expression
			{
				variable = Expression(ref pos);
			}

			this.EnsureLexemeType(ScriptLexemeType.EndStatement, pos);
			pos++;

			return variable;
		}

		private void Echo(ref int pos)
		{
			this.EnsureLexemeType(ScriptLexemeType.Echo, pos);

			pos++;
			ScriptVariable variable = Expression(ref pos);

			if (variable != null)
				Output(variable.ToString());
		}

		private void DoIf(ref int pos)
		{
			DoIf(ref pos, false);
		}

		/// <summary>
		/// Executes or skips an if statement.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="skip">Should the if statement be skipped?</param>
		private void DoIf(ref int pos, bool skip)
		{
			this.EnsureLexemeType(ScriptLexemeType.If, pos);

			pos++;
			this.EnsureLexemeType(ScriptLexemeType.OpenParen, pos);

			pos++;
			ScriptVariable variable = Expression(ref pos);

			this.EnsureLexemeType(ScriptLexemeType.CloseParen, pos);

			pos++;
			this.EnsureLexemeType(ScriptLexemeType.OpenBrace, pos);

			pos++;
			if (variable.ToBoolean() && !skip)
			{
				variableStack.Push();

				while (lexemes[pos].Type != ScriptLexemeType.CloseBrace)
				{
					Statement(ref pos);

					this.EnsureLexemeType(ScriptLexemeType.EndStatement, pos);

					pos++;
				}

				variableStack.Pop();

				pos++;

				if (lexemes[pos].Type == ScriptLexemeType.Else)
				{
					pos++;

					if (lexemes[pos].Type == ScriptLexemeType.If)
					{
						DoIf(ref pos, true);
					}
					else if (lexemes[pos].Type == ScriptLexemeType.OpenBrace)
					{
						pos++;

						while (lexemes[pos].Type != ScriptLexemeType.CloseBrace)
							pos++;

						pos++;
					}
					else
					{
						throw new ScriptSyntaxException("'if' or '{' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);
					}
				}
			}
			else
			{
				while (lexemes[pos].Type != ScriptLexemeType.CloseBrace)
					pos++;

				pos++;

				if (lexemes[pos].Type == ScriptLexemeType.Else)
				{
					pos++;

					if (lexemes[pos].Type == ScriptLexemeType.If)
					{
						DoIf(ref pos, skip);
					}
					else if (lexemes[pos].Type == ScriptLexemeType.OpenBrace)
					{
						pos++;

						if (!skip)
						{
							variableStack.Push();

							while (lexemes[pos].Type != ScriptLexemeType.CloseBrace)
							{
								Statement(ref pos);

								this.EnsureLexemeType(ScriptLexemeType.EndStatement, pos);

								pos++;
							}

							variableStack.Pop();
						}
						else
						{
							while (lexemes[pos].Type != ScriptLexemeType.CloseBrace)
								pos++;
						}

						pos++;
					}
					else
					{
						throw new ScriptSyntaxException("'if' or '{' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);
					}
				}
			}
		}

		/// <summary>
		/// Skips over an if statement.
		/// </summary>
		/// <param name="pos"></param>
		private void SkipIf(ref int pos)
		{
		}

		private ScriptVariable Expression(ref int pos)
		{
			ScriptVariable variable = null;

			if (lexemes[pos].Type == ScriptLexemeType.Identifier)
			{
				FuncCall(ref pos);
			}
			else if (lexemes[pos].Type == ScriptLexemeType.Variable)
			{
				variable = Variable(ref pos);

				if(lexemes[pos].Type == ScriptLexemeType.OpenBracket)
				{
					pos++;

					if(lexemes[pos].Type != ScriptLexemeType.CloseBracket)
						variable = variable.AtIndex(Statement(ref pos)); // Key specified
					else
						variable = variable.AtIndex(null); // Request next key

					if (lexemes[pos].Type != ScriptLexemeType.CloseBracket)
						this.EnsureLexemeType(ScriptLexemeType.CloseBracket, pos);

					pos++;
				}

				// Check for operations

				if (lexemes[pos].Type == ScriptLexemeType.Gets)
				{
					pos++;
					variable.Gets(Expression(ref pos));
				}
				else if (lexemes[pos].Type == ScriptLexemeType.Plus)
				{
					pos++;
					variable = variable.Add(Expression(ref pos));
				}
				else if (lexemes[pos].Type == ScriptLexemeType.PlusGets)
				{
					pos++;
					variable.Gets(variable.Add(Expression(ref pos)));
				}
				else if (lexemes[pos].Type == ScriptLexemeType.Minus)
				{
					pos++;
					variable = variable.Subtract(Expression(ref pos));
				}
				else if (lexemes[pos].Type == ScriptLexemeType.MinusGets)
				{
					pos++;
					variable.Gets(variable.Subtract(Expression(ref pos)));
				}
				else if (lexemes[pos].Type == ScriptLexemeType.EqualTo)
				{
					pos++;
					variable = variable.EqualTo(Expression(ref pos));
				}
			}
			else if (lexemes[pos].Type == ScriptLexemeType.Null)
			{
				variable = Null(ref pos);
			}
			else if (lexemes[pos].Type == ScriptLexemeType.True || lexemes[pos].Type == ScriptLexemeType.False)
			{
				variable = Boolean(ref pos);
			}
			else if (lexemes[pos].Type == ScriptLexemeType.Char)
			{
				variable = Char(ref pos);
			}
			else if (lexemes[pos].Type == ScriptLexemeType.String)
			{
				variable = String(ref pos);
			}
			else if (lexemes[pos].Type == ScriptLexemeType.Integer)
			{
				variable = Integer(ref pos);
			}
			else if (lexemes[pos].Type == ScriptLexemeType.Float)
			{
				variable = Float(ref pos);
			}
			else if (lexemes[pos].Type == ScriptLexemeType.Array)
			{
				variable = Array(ref pos);
			}

			return variable;
		}

		private ScriptVariable FuncCall(ref int pos)
		{
			this.EnsureLexemeType(ScriptLexemeType.Identifier, pos);
			string funcName = this.lexemes[pos].Val;

			pos++;
			this.EnsureLexemeType(ScriptLexemeType.OpenParen, pos);

			pos++;
			List<ScriptVariable> args = new List<ScriptVariable>();

			while (this.lexemes[pos].Type != ScriptLexemeType.CloseParen &&
				   this.lexemes[pos].Type != ScriptLexemeType.EOF)
			{
				args.Add(this.Expression(ref pos));

				if (this.lexemes[pos].Type == ScriptLexemeType.Comma)
				{
					pos++;

					this.EnsureNotLexemeType(ScriptLexemeType.CloseParen, pos);
				}
			}
									
			this.EnsureLexemeType(ScriptLexemeType.CloseParen, pos);
			
			pos++; // Move to just after the FuncCall.

			return this.CallFunc(funcName, args);
		}

		/// <summary>
		/// Returns the SciptVariable associated with the identifier at pos.
		/// </summary>
		/// <param name="pos">The lexeme to read.</param>
		/// <returns>ScriptVariable</returns>
		private ScriptVariable Variable(ref int pos)
		{
			this.EnsureLexemeType(ScriptLexemeType.Variable, pos);

			pos++;
			this.EnsureLexemeType(ScriptLexemeType.Identifier, pos);

			ScriptVariable variable = this.variableStack[lexemes[pos].Val];
			
			pos++;
			return variable;
		}

		private ScriptVariable Null(ref int pos)
		{
			pos++;
			return new ScriptVariable();
		}

		private ScriptVariable Boolean(ref int pos)
		{
			pos++;
			if (this.lexemes[pos - 1].Type == ScriptLexemeType.True)
				return new ScriptVariable(true);
			else
				return new ScriptVariable(false);
		}

		private ScriptVariable Char(ref int pos)
		{
			return new ScriptVariable(this.lexemes[pos++].Val);
		}

		private ScriptVariable String(ref int pos)
		{
			return new ScriptVariable(this.lexemes[pos++].Val);
		}

		private ScriptVariable Integer(ref int pos)
		{
			return new ScriptVariable(Int32.Parse(this.lexemes[pos++].Val));
		}

		private ScriptVariable Float(ref int pos)
		{
			return new ScriptVariable(Double.Parse(this.lexemes[pos++].Val));
		}

		private ScriptVariable Array(ref int pos)
		{
			pos++;
			this.EnsureLexemeType(ScriptLexemeType.OpenParen, pos);

			Dictionary<string, ScriptVariable> values = new Dictionary<string, ScriptVariable>();
			pos++;

			int i = 0;
			while (this.lexemes[pos].Type != ScriptLexemeType.CloseParen &&
				   this.lexemes[pos].Type != ScriptLexemeType.EOF)
			{
				if (this.lexemes[pos + 1].Type != ScriptLexemeType.MapsTo) // No key specified
				{
					values.Add((i++).ToString(), Expression(ref pos));
				}
				else
				{
					ScriptVariable key = Expression(ref pos);

					pos++; // =>

					values.Add(key.ToString(), Expression(ref pos));
				}

				if (this.lexemes[pos].Type == ScriptLexemeType.Comma)
					pos++;
			}

			this.EnsureLexemeType(ScriptLexemeType.CloseParen, pos);

			pos++;
			return new ScriptVariable(values);
		}

		/// <summary>
		/// Output function. By default uses Console.Write().
		/// </summary>
		/// <param name="s"></param>
		public virtual void Output(string s)
		{
			Console.Write(s);
		}

		/// <summary>
		/// Output line function.
		/// </summary>
		/// <param name="s"></param>
		public void OutputLine(string s)
		{
			Output(s + '\n');
		}
	}
}
