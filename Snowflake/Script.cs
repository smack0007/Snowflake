using System;
using System.Collections.Generic;
using System.IO;

namespace Snowsoft.SnowflakeScript
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

		List<Lexeme> lexemes;
		Dictionary<string, FuncInfo> funcs;
		VariableStack variableStack;
		bool debug;

		/// <summary>
		/// If debug is true, debug message will be output.
		/// </summary>
		public bool Debug
		{
			get { return this.debug; }
			set { this.debug = true; }
		}

		/// <summary>
		/// Private constructor. Use static methods to construct instances.
		/// </summary>
		private Script(List<Lexeme> lexemes)
		{
			this.lexemes = lexemes;
			this.funcs = new Dictionary<string, FuncInfo>();
			this.variableStack = new VariableStack();
			this.debug = false;

			for (int pos = 0; pos < this.lexemes.Count; pos++)
			{
				if (this.lexemes[pos].Type == LexemeType.Func)
				{
					FuncInfo funcInfo = this.ParseFunc(ref pos);
					this.funcs.Add(funcInfo.Name, funcInfo);
				}
			}
		}

		/// <summary>
		/// Loads a script from a file.
		/// </summary>
		/// <param name="filename">The filename to read from.</param>
		/// <returns>Script</returns>
		public static Script FromFile(string filename)
		{
			StreamReader sr = File.OpenText(filename);
			string text = sr.ReadToEnd();
			
			return new Script(Lexeme.Parse(text));
		}	

		public void DisplayLexemes()
		{
			for (int i = 0; i < lexemes.Count; i++)
				OutputLine(i + " => " + lexemes[i]);
		}
				
		private void EnsureLexemeType(LexemeType expected, int pos)
		{
			if (this.lexemes[pos].Type != expected)
				throw new ScriptException(ScriptError.SyntaxError, expected + " was expected at Line " + this.lexemes[pos].Line + " Column " + this.lexemes[pos].Column + ".");
		}

		private void EnsureNotLexemeType(LexemeType unexpected, int pos)
		{
			if (this.lexemes[pos].Type == unexpected)
				throw new ScriptException(ScriptError.SyntaxError, unexpected + " was not expected at Line " + this.lexemes[pos].Line + " Column " + this.lexemes[pos].Column + ".");
		}

		/// <summary>
		/// Moves "pos" to from OpenBrace to the matching CloseBrace.
		/// </summary>
		/// <param name="pos"></param>
		private void MoveToMatchingBrace(ref int pos)
		{
			this.EnsureLexemeType(LexemeType.OpenBrace, pos);

			int level = 1;
			while (level > 0 && this.lexemes[pos].Type != LexemeType.EOF)
			{
				pos++;
				if (this.lexemes[pos].Type == LexemeType.OpenBrace)
				{
					level++;
				}
				else if (this.lexemes[pos].Type == LexemeType.CloseBrace)
				{
					level--;
				}
			}

			this.EnsureLexemeType(LexemeType.CloseBrace, pos);
		}

		private FuncInfo ParseFunc(ref int pos)
		{
			FuncInfo funcInfo = new FuncInfo();

			this.EnsureLexemeType(LexemeType.Func, pos);
			funcInfo.Location = pos;

			pos++;
			this.EnsureLexemeType(LexemeType.Identifier, pos);

			funcInfo.Name = this.lexemes[pos].Val;

			pos++;
			this.EnsureLexemeType(LexemeType.OpenParen, pos);

			pos++;
			if (this.lexemes[pos].Type == LexemeType.Variable)
			{
				List<string> argList = new List<string>();
				
				pos++;
				this.EnsureLexemeType(LexemeType.Identifier, pos);
				argList.Add(this.lexemes[pos].Val);

				pos++;
				while (this.lexemes[pos].Type == LexemeType.Comma)
				{
					pos++;
					this.EnsureLexemeType(LexemeType.Variable, pos);

					pos++;
					this.EnsureLexemeType(LexemeType.Identifier, pos);
					argList.Add(this.lexemes[pos].Val);

					pos++;
				}

				funcInfo.Args = argList.ToArray();
			}
			else // No args
			{
				funcInfo.Args = new string[0];
			}

			this.EnsureLexemeType(LexemeType.CloseParen, pos);

			pos++;
			this.EnsureLexemeType(LexemeType.OpenBrace, pos);
			funcInfo.EntryLocation = pos;

			this.MoveToMatchingBrace(ref pos);

			this.EnsureLexemeType(LexemeType.CloseBrace, pos);
			funcInfo.ExitLocation = pos;

			return funcInfo;
		}

		/// <summary>
		/// Execute the script.
		/// </summary>
		public Variable Execute()
		{
			return this.Execute("Main", null);
		}

		public Variable Execute(string funcName)
		{
			return this.Execute(funcName, null);
		}

		public Variable Execute(string funcName, IList<Variable> args)
		{
			if (!this.funcs.ContainsKey(funcName))
				throw new ScriptException(ScriptError.InvalidFunctionCall, "No function found by the name " + funcName + ".");

			FuncInfo funcInfo = this.funcs[funcName];
			int pos = funcInfo.EntryLocation;

			this.variableStack.Push();

			if (args != null)
			{
				if (args.Count != funcInfo.Args.Length)
					throw new ScriptException(ScriptError.InvalidFunctionCall, "Invalid number of args specified for " + funcName + ".");

				for (int i = 0; i < args.Count; i++)
				{
					Variable arg = this.variableStack[funcInfo.Args[i]];
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

		private Variable Statement(ref int pos)
		{
			if (this.debug)
				OutputLine("Statement at " + pos);

			Variable variable = null;

			if (lexemes[pos].Type == LexemeType.Echo)
			{
				Echo(ref pos);
			}
			else if (lexemes[pos].Type == LexemeType.If)
			{
				DoIf(ref pos);
			}
			else // Must be an expression
			{
				variable = Expression(ref pos);
			}

			this.EnsureLexemeType(LexemeType.EndStatement, pos);
			pos++;

			return variable;
		}

		private void Echo(ref int pos)
		{
			if (this.debug)
				OutputLine("Echo at " + pos);

			this.EnsureLexemeType(LexemeType.Echo, pos);

			pos++;
			Variable variable = Expression(ref pos);

			if (variable != null)
				OutputLine(variable.ToString());
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
			pos++;
			if (lexemes[pos].Type != LexemeType.OpenParen)
				throw new ScriptException(ScriptError.SyntaxError, "'(' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

			pos++;
			Variable variable = Expression(ref pos);

			if (lexemes[pos].Type != LexemeType.CloseParen)
				throw new ScriptException(ScriptError.SyntaxError, "')' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

			pos++;
			if (lexemes[pos].Type != LexemeType.OpenBrace)
				throw new ScriptException(ScriptError.SyntaxError, "'{' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

			pos++;
			if (variable.ToBoolean() && !skip)
			{
				variableStack.Push();

				while (lexemes[pos].Type != LexemeType.CloseBrace)
				{
					Statement(ref pos);

					if (lexemes[pos].Type != LexemeType.EndStatement)
						throw new ScriptException(ScriptError.SyntaxError, "; was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

					pos++;
				}

				variableStack.Pop();

				pos++;

				if (lexemes[pos].Type == LexemeType.Else)
				{
					pos++;

					if (lexemes[pos].Type == LexemeType.If)
					{
						DoIf(ref pos, true);
					}
					else if (lexemes[pos].Type == LexemeType.OpenBrace)
					{
						pos++;

						while (lexemes[pos].Type != LexemeType.CloseBrace)
							pos++;

						pos++;
					}
					else
					{
						throw new ScriptException(ScriptError.SyntaxError, "'if' or '{' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);
					}
				}
			}
			else
			{
				while (lexemes[pos].Type != LexemeType.CloseBrace)
					pos++;

				pos++;

				if (lexemes[pos].Type == LexemeType.Else)
				{
					pos++;

					if (lexemes[pos].Type == LexemeType.If)
					{
						DoIf(ref pos, skip);
					}
					else if (lexemes[pos].Type == LexemeType.OpenBrace)
					{
						pos++;

						if (!skip)
						{
							variableStack.Push();

							while (lexemes[pos].Type != LexemeType.CloseBrace)
							{
								Statement(ref pos);

								if (lexemes[pos].Type != LexemeType.EndStatement) // Check for ;
									throw new ScriptException(ScriptError.SyntaxError, "; was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

								pos++;
							}

							variableStack.Pop();
						}
						else
						{
							while (lexemes[pos].Type != LexemeType.CloseBrace)
								pos++;
						}

						pos++;
					}
					else
					{
						throw new ScriptException(ScriptError.SyntaxError, "'if' or '{' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);
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

		private Variable Expression(ref int pos)
		{
			if (this.debug)
				OutputLine("Expression at " + pos);

			Variable variable = null;

			if (lexemes[pos].Type == LexemeType.Identifier)
			{
				FuncCall(ref pos);
			}
			else if (lexemes[pos].Type == LexemeType.Variable)
			{
				variable = Variable(ref pos);

				if(lexemes[pos].Type == LexemeType.OpenBracket)
				{
					pos++;

					if(lexemes[pos].Type != LexemeType.CloseBracket)
						variable = variable.AtIndex(Statement(ref pos)); // Key specified
					else
						variable = variable.AtIndex(null); // Request next key

					if (lexemes[pos].Type != LexemeType.CloseBracket)
						this.EnsureLexemeType(LexemeType.CloseBracket, pos);

					pos++;
				}

				// Check for operations

				if (lexemes[pos].Type == LexemeType.Gets)
				{
					pos++;
					variable.Gets(Expression(ref pos));
				}
				else if (lexemes[pos].Type == LexemeType.Plus)
				{
					pos++;
					variable = variable.Add(Expression(ref pos));
				}
				else if (lexemes[pos].Type == LexemeType.PlusGets)
				{
					pos++;
					variable.Gets(variable.Add(Expression(ref pos)));
				}
				else if (lexemes[pos].Type == LexemeType.Minus)
				{
					pos++;
					variable = variable.Subtract(Expression(ref pos));
				}
				else if (lexemes[pos].Type == LexemeType.MinusGets)
				{
					pos++;
					variable.Gets(variable.Subtract(Expression(ref pos)));
				}
				else if (lexemes[pos].Type == LexemeType.EqualTo)
				{
					pos++;
					variable = variable.EqualTo(Expression(ref pos));
				}
			}
			else if (lexemes[pos].Type == LexemeType.Null)
			{
				variable = Null(ref pos);
			}
			else if (lexemes[pos].Type == LexemeType.True || lexemes[pos].Type == LexemeType.False)
			{
				variable = Boolean(ref pos);
			}
			else if (lexemes[pos].Type == LexemeType.String)
			{
				variable = String(ref pos);
			}
			else if (lexemes[pos].Type == LexemeType.MagicString)
			{
				variable = MagicString(ref pos);
			}
			else if (lexemes[pos].Type == LexemeType.Integer)
			{
				variable = Integer(ref pos);
			}
			else if (lexemes[pos].Type == LexemeType.Float)
			{
				variable = Float(ref pos);
			}
			else if (lexemes[pos].Type == LexemeType.Array)
			{
				variable = Array(ref pos);
			}

			return variable;
		}

		private Variable FuncCall(ref int pos)
		{
			this.EnsureLexemeType(LexemeType.Identifier, pos);
			string funcName = this.lexemes[pos].Val;

			pos++;
			this.EnsureLexemeType(LexemeType.OpenParen, pos);

			pos++;
			List<Variable> args = new List<Variable>();

			while (this.lexemes[pos].Type != LexemeType.CloseParen &&
				   this.lexemes[pos].Type != LexemeType.EOF)
			{
				args.Add(this.Expression(ref pos));

				if (this.lexemes[pos].Type == LexemeType.Comma)
				{
					pos++;

					this.EnsureNotLexemeType(LexemeType.CloseParen, pos);
				}
			}
									
			this.EnsureLexemeType(LexemeType.CloseParen, pos);
			
			pos++; // Move to just after the FuncCall.

			return this.Execute(funcName, args);
		}

		/// <summary>
		/// Returns the SciptVariable associated with the identifier at pos.
		/// </summary>
		/// <param name="pos">The lexeme to read.</param>
		/// <returns>ScriptVariable</returns>
		private Variable Variable(ref int pos)
		{
			if (this.debug)
				OutputLine("Variable at " + pos);

			this.EnsureLexemeType(LexemeType.Variable, pos);

			pos++;

			Variable variable = null;

			if (lexemes[pos].Type == LexemeType.Identifier)
			{
				variable = variableStack[lexemes[pos].Val];
			}

			pos++;
			return variable;
		}

		private Variable Null(ref int pos)
		{
			if (this.debug)
				OutputLine("Null at " + pos);

			pos++;
			return new Variable();
		}

		private Variable Boolean(ref int pos)
		{
			if (this.debug)
				OutputLine("Boolean at " + pos);

			pos++;
			if (lexemes[pos - 1].Type == LexemeType.True)
				return new Variable(true);
			else
				return new Variable(false);
		}

		private Variable String(ref int pos)
		{
			if (this.debug)
				OutputLine("String at " + pos);

			return new Variable(lexemes[pos++].Val);
		}

		private Variable MagicString(ref int pos)
		{
			if (this.debug)
				OutputLine("MagicString at " + pos);

			string output = lexemes[pos++].Val;

			return new Variable(output);
		}

		private Variable Integer(ref int pos)
		{
			if (this.debug)
				OutputLine("Integer at " + pos);
						
			return new Variable(Int32.Parse(lexemes[pos++].Val));
		}

		private Variable Float(ref int pos)
		{
			if (this.debug)
				OutputLine("Float at " + pos);

			return new Variable(Double.Parse(lexemes[pos++].Val));
		}

		private Variable Array(ref int pos)
		{
			if (this.debug)
				OutputLine("Array at " + pos);

			pos++;
			if(lexemes[pos].Type != LexemeType.OpenParen)
				throw new ScriptException(ScriptError.SyntaxError, "'(' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

			Dictionary<string, Variable> values = new Dictionary<string, Variable>();
			pos++;

			int i = 0;
			while (lexemes[pos].Type != LexemeType.CloseParen && lexemes[pos].Type != LexemeType.EOF)
			{
				if (lexemes[pos + 1].Type != LexemeType.MapsTo) // No key specified
				{
					values.Add((i++).ToString(), Expression(ref pos));
				}
				else
				{
					Variable key = Expression(ref pos);

					pos++; // =>

					values.Add(key.ToString(), Expression(ref pos));
				}

				if(lexemes[pos].Type == LexemeType.Comma)
					pos++;
			}

			if (lexemes[pos].Type != LexemeType.CloseParen)
				throw new ScriptException(ScriptError.SyntaxError, "')' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

			pos++;
			return new Variable(values);
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
