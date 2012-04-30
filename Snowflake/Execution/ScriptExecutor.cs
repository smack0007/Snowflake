using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowsoft.SnowflakeScript.Parsing;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptExecutor : IScriptExecutor
	{
		ScriptNode scriptNode;
		bool shouldReturn;
		Variable returnValue;

		public ScriptExecutor()
		{
		}

		private void ThrowUnableToExecuteException(string executionStage, SyntaxTreeNode node)
		{
			throw new ExecutionException("Unable to execute node type " + node.GetType().Name + " at " + executionStage + ".");
		}

		public Variable CallFunction(ScriptNode node, string funcName, IList<Variable> args, VariableStack stack)
		{
			this.scriptNode = node; // TODO: Change this.

			Variable functionReturnValue = Variable.Null;

			foreach (FunctionNode func in node.Functions)
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

					this.shouldReturn = false;
					this.returnValue = null;

					foreach (StatementNode statement in func.StatementBlock.Statements)
					{
						this.ExecuteStatement(statement, stack);

						if (this.shouldReturn)
						{
							functionReturnValue = this.returnValue;

							this.returnValue = null;
							this.shouldReturn = false;

							break;
						}
					}

					stack.Pop();
				}
			}

			return functionReturnValue;
		}

		private void ExecuteStatement(StatementNode node, VariableStack stack)
		{
			if (node is EchoNode)
			{
				this.ExecuteEcho((EchoNode)node, stack);
			}
			else if (node is ReturnNode)
			{
				this.ExecuteReturn((ReturnNode)node, stack);
			}
			else if (node is ExpressionNode)
			{
				this.ExecuteExpression((ExpressionNode)node, stack);
			}
			else
			{
				throw new ExecutionException("Unable to handle node in script tree.");
			}
		}

		private void ExecuteEcho(EchoNode node, VariableStack stack)
		{
			Console.Write(this.ExecuteExpression(node.Expression, stack));
		}

		private void ExecuteReturn(ReturnNode node, VariableStack stack)
		{
			this.shouldReturn = true;
			this.returnValue = this.ExecuteExpression(node.Expression, stack);
		}

		private Variable ExecuteExpression(ExpressionNode node, VariableStack stack)
		{
			Variable result = null;

			if (node is FunctionCallNode)
			{
				FunctionCallNode functionCall = (FunctionCallNode)node;

				List<Variable> args = new List<Variable>();
				foreach (ExpressionNode expression in functionCall.ArgExpressions)
				{
					args.Add(this.ExecuteExpression(expression, stack));
				}

				result = this.CallFunction(this.scriptNode, functionCall.FunctionName, args, stack);
			}
			else if (node is OperationNode)
			{
				OperationNode operation = (OperationNode)node;
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
			else if (node is VariableNode)
			{
				result = stack[((VariableNode)node).VariableName];
			}
			else if (node is NullValueNode)
			{
				result = Variable.Null;
			}
			else if (node is StringValueNode)
			{
				result = new Variable(((StringValueNode)node).Value);
			}
			else if (node is CharValueNode)
			{
				result = new Variable(((CharValueNode)node).Value);
			}
			else if (node is IntegerValueNode)
			{
				result = new Variable(((IntegerValueNode)node).Value);
			}
			else if (node is FloatValueNode)
			{
				result = new Variable(((FloatValueNode)node).Value);
			}

			if (result == null)
				this.ThrowUnableToExecuteException("Expression", node);

			return result;
		}
	}
}
