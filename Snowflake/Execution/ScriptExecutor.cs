using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowsoft.SnowflakeScript.Parsing;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptExecutor : IScriptExecutor
	{
		ScriptNode script;
		bool shouldReturn;
		Variable returnValue;

		public VariableStack Stack
		{
			get;
			private set;
		}

		public ScriptExecutor()
		{
			this.Stack = new VariableStack();
		}

		public void SetScript(ScriptNode script)
		{
			this.script = script;
		}

		private void ThrowUnableToExecuteException(string executionStage, SyntaxTreeNode node)
		{
			throw new ExecutionException("Unable to execute node type " + node.GetType().Name + " at " + executionStage + ".");
		}

		public Variable CallFunction(string funcName, IList<Variable> args)
		{
			if (this.script == null)
			{
				throw new InvalidOperationException("No script loaded for execution.");
			}

			Variable functionReturnValue = Variable.Null;

			foreach (FunctionNode func in this.script.Functions)
			{
				if (func.Name == funcName)
				{
					if (args != null && args.Count != func.Args.Count)
						throw new ExecutionException("Invalid number of arguments specified when calling \"" + funcName + "\".");

					this.Stack.Push();

					if (args != null)
					{
						for (int i = 0; i < args.Count; i++)
						{
							Variable variable = Stack[func.Args[i]];
							variable.Gets(args[i]);
						}
					}

					this.shouldReturn = false;
					this.returnValue = null;

					foreach (StatementNode statement in func.StatementBlock.Statements)
					{
						this.ExecuteStatement(statement);

						if (this.shouldReturn)
						{
							functionReturnValue = this.returnValue;

							this.returnValue = null;
							this.shouldReturn = false;

							break;
						}
					}

					this.Stack.Pop();
				}
			}

			return functionReturnValue;
		}

		private void ExecuteStatement(StatementNode node)
		{
			if (node is EchoNode)
			{
				this.ExecuteEcho((EchoNode)node);
			}
			else if (node is ReturnNode)
			{
				this.ExecuteReturn((ReturnNode)node);
			}
			else if (node is ExpressionNode)
			{
				this.ExecuteExpression((ExpressionNode)node);
			}
			else
			{
				throw new ExecutionException("Unable to handle node in script tree.");
			}
		}

		private void ExecuteEcho(EchoNode node)
		{
			Console.Write(this.ExecuteExpression(node.Expression));
		}

		private void ExecuteReturn(ReturnNode node)
		{
			this.shouldReturn = true;
			this.returnValue = this.ExecuteExpression(node.Expression);
		}

		private Variable ExecuteExpression(ExpressionNode node)
		{
			Variable result = null;

			if (node is FunctionCallNode)
			{
				FunctionCallNode functionCall = (FunctionCallNode)node;

				List<Variable> args = new List<Variable>();
				foreach (ExpressionNode expression in functionCall.ArgExpressions)
				{
					args.Add(this.ExecuteExpression(expression));
				}

				result = this.CallFunction(functionCall.FunctionName, args);
			}
			else if (node is OperationNode)
			{
				OperationNode operation = (OperationNode)node;
				result = this.ExecuteExpression(operation.LHS);
				Variable rhs = this.ExecuteExpression(operation.RHS);

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
				result = this.Stack[((VariableNode)node).VariableName];
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
