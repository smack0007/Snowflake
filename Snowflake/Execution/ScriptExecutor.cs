using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowsoft.SnowflakeScript.Parsing;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptExecutor : IScriptExecutor
	{
		public ScriptExecutor()
		{
		}

		private void ThrowUnableToExecuteException(string executionType, SyntaxTreeNode node)
		{
			throw new ExecutionException("Unable to execute node type " + node.GetType().Name + " at " + executionType + ".");
		}

		public Variable CallFunc(ScriptNode script, string funcName, IList<Variable> args, VariableStack stack)
		{
			foreach (FunctionNode func in script.Functions)
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

					foreach (StatementNode statement in func.StatementBlock.Statements)
					{
						this.ExecuteStatement(statement, stack);
					}

					stack.Pop();
				}
			}

			return null;
		}

		private void ExecuteStatement(StatementNode statement, VariableStack stack)
		{
			if (statement is EchoNode)
			{
				this.ExecuteEcho((EchoNode)statement, stack);
			}
			else if (statement is ExpressionNode)
			{
				this.ExecuteExpression((ExpressionNode)statement, stack);
			}
			else
			{
				throw new ExecutionException("Unable to handle node in script tree.");
			}
		}

		private void ExecuteEcho(EchoNode echo, VariableStack stack)
		{
			Console.Write(this.ExecuteExpression(echo.Expression, stack));
		}

		private Variable ExecuteExpression(ExpressionNode expression, VariableStack stack)
		{
			Variable result = null;

			if (expression is OperationNode)
			{
				OperationNode operation = (OperationNode)expression;
				result = this.ExecuteExpression(operation.LHS, stack);
				Variable rhs = this.ExecuteExpression(operation.RHS, stack);

				switch (operation.Type)
				{
					case OperationType.Gets:
						result.Gets(rhs);
						break;

					case OperationType.Add:
						result = result.Add(rhs);
						break;
				}
			}
			else if (expression is VariableNode)
			{
				result = stack[((VariableNode)expression).VariableName];
			}
			else if (expression is NullValueNode)
			{
				result = Variable.Null;
			}
			else if (expression is StringValueNode)
			{
				result = new Variable(((StringValueNode)expression).Value);
			}
			else if (expression is CharValueNode)
			{
				result = new Variable(((CharValueNode)expression).Value);
			}
			else if (expression is IntegerValueNode)
			{
				result = new Variable(((IntegerValueNode)expression).Value);
			}
			else if (expression is FloatValueNode)
			{
				result = new Variable(((FloatValueNode)expression).Value);
			}

			if (result == null)
				this.ThrowUnableToExecuteException("Expression", expression);

			return result;
		}
	}
}
