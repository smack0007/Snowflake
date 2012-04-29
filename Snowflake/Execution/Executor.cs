using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowsoft.SnowflakeScript.Parsing;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class Executor : IExecutor
	{
		public Executor()
		{
		}

		public Variable CallFunc(ScriptNode script, string funcName, IList<Variable> args, VariableStack stack)
		{
			foreach (FuncNode func in script.Funcs)
			{
				if (func.Name == funcName)
				{
					if (args != null && args.Count != func.Args.Count)
						throw new ExecutionException("Invalid number of arguments specified when calling \"" + funcName + "\".");

					stack.Push();

					if (args != null)
					{
						for (int i = 0; i < args.Count; i++)
						{
							Variable variable = stack[func.Args[i]];
							variable.Gets(args[i]);
						}
					}

					foreach (StatementNode statement in func.Statements)
					{
						this.ExecuteStatement(statement, stack);
					}

					stack.Pop();
				}
			}

			return null;
		}

		public void ExecuteStatement(StatementNode statement, VariableStack stack)
		{
			if (statement is EchoNode)
			{
				this.ExecuteEcho((EchoNode)statement, stack);
			}
		}

		public void ExecuteEcho(EchoNode echo, VariableStack stack)
		{
			Console.Write(this.ExecuteExpression(echo.Expression, stack));
		}

		public Variable ExecuteExpression(ExpressionNode expression, VariableStack stack)
		{
			if (expression is StringValueNode)
			{
				return new Variable(((StringValueNode)expression).Value);
			}

			return null;
		}
	}
}
