using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowsoft.SnowflakeScript.Parsing;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptExecutor
	{
		class ExecutionContext
		{
			public ScriptStack Stack;
			public bool ShouldReturn;
			public ScriptVariable ReturnValue;
		}

		public ScriptExecutor()
		{
		}
				
		private void ThrowUnableToExecuteException(string executionStage, SyntaxTreeNode node)
		{
			throw new ScriptExecutionException("Unable to execute node type " + node.GetType().Name + " at " + executionStage + ".");
		}

		public ScriptVariable Execute(ScriptNode script, ScriptStack stack)
		{
			if (script == null)
				throw new ArgumentNullException("script");

			if (stack == null)
				throw new ArgumentNullException("stack");

			var context = new ExecutionContext()
			{
				Stack = stack,
				ReturnValue = ScriptVariable.Null
			};
						
			foreach (StatementNode statement in script.Statements)
			{
				this.ExecuteStatement(context, statement);

				if (context.ShouldReturn)
					break;
			}

			return context.ReturnValue;
		}

		private ScriptVariable CallFunction(ExecutionContext context, ScriptFunction function, IList<ScriptVariable> args)
		{
			ScriptVariable functionReturnValue = ScriptVariable.Null;
				
			//if (args != null && args.Count != func.Args.Count)
			//	throw new ExecutionException("Invalid number of arguments specified when calling \"" + funcName + "\".");

			context.Stack.Push();
			
			//if (args != null)
			//{
			//	for (int i = 0; i < args.Count; i++)
			//	{
			//		Variable variable = Stack[func.Args[i]];
			//		variable.Gets(args[i]);
			//	}
			//}

			context.ShouldReturn = false;
			context.ReturnValue = null;

			foreach (StatementNode statement in function.StatementBlock.Statements)
			{
				this.ExecuteStatement(context, statement);

				if (context.ShouldReturn)
				{
					functionReturnValue = context.ReturnValue;

					context.ReturnValue = ScriptVariable.Null;
					context.ShouldReturn = false;

					break;
				}
			}

			context.Stack.Pop();
				
			return functionReturnValue;
		}

		private void ExecuteStatement(ExecutionContext context, StatementNode node)
		{
			if (node is VariableDeclarationNode)
			{
				this.ExecuteVariableDeclaration(context, (VariableDeclarationNode)node);
			}
			else if (node is EchoNode)
			{
				this.ExecuteEcho(context, (EchoNode)node);
			}
			else if (node is ReturnNode)
			{
				this.ExecuteReturn(context, (ReturnNode)node);
			}
			else if (node is ExpressionNode)
			{
				this.ExecuteExpression(context, (ExpressionNode)node);
			}
			else
			{
				throw new ScriptExecutionException("Unable to handle node in script tree.");
			}
		}

		private void ExecuteVariableDeclaration(ExecutionContext context, VariableDeclarationNode node)
		{
			ScriptVariable variable = null;

			if (node.ValueExpression != null)
			{
				variable = this.ExecuteExpression(context, node.ValueExpression);
			}
			else
			{
				variable = ScriptVariable.Null;
			}

			context.Stack[node.VariableName] = variable;
		}

		private void ExecuteEcho(ExecutionContext context, EchoNode node)
		{
			Console.Write(this.ExecuteExpression(context, node.Expression));
		}

		private void ExecuteReturn(ExecutionContext context, ReturnNode node)
		{
			// It's important to store the result before setting any of the context values because
			// the expression itself could change the context values.
			var result = this.ExecuteExpression(context, node.Expression);

			context.ShouldReturn = true;
			context.ReturnValue = result;
		}

		private ScriptVariable GetVariable(ExecutionContext context, string variableName)
		{
			ScriptVariable variable = context.Stack[variableName];

			if (variable == null)
				throw new ScriptExecutionException(string.Format("\"{0}\" is not defined.", variableName));

			return variable;
		}

		private ScriptVariable ExecuteExpression(ExecutionContext context, ExpressionNode node)
		{
			ScriptVariable result = null;

			if (node is FunctionNode)
			{
				FunctionNode function = (FunctionNode)node;
				result = new ScriptVariable(new ScriptFunction(function.StatementBlock));
			}
			else if (node is FunctionCallNode)
			{
				FunctionCallNode functionCall = (FunctionCallNode)node;

				List<ScriptVariable> args = new List<ScriptVariable>();
				foreach (ExpressionNode expression in functionCall.Arguments)
				{
					args.Add(this.ExecuteExpression(context, expression));
				}

				ScriptVariable variable = this.GetVariable(context, functionCall.FunctionName);
				
				if (variable.Type != ScriptVariableType.Function)
					throw new ScriptExecutionException(string.Format("\"{0}\" is not a function.", functionCall.FunctionName));

				result = this.CallFunction(context, (ScriptFunction)variable.Value, args);
			}
			else if (node is OperationNode)
			{
				OperationNode operation = (OperationNode)node;
				result = this.ExecuteExpression(context, operation.LHS);
				ScriptVariable rhs = this.ExecuteExpression(context, operation.RHS);

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
			else if (node is VariableReferenceNode)
			{
				var variableReferenceNode = (VariableReferenceNode)node;

				result = context.Stack[variableReferenceNode.VariableName];

				if (result == null)
					throw new ScriptException(string.Format("Variable \"{0}\" is not defined.", variableReferenceNode.VariableName));
			}
			else if (node is NullValueNode)
			{
				result = ScriptVariable.Null;
			}
			else if (node is BooleanValueNode)
			{
				result = new ScriptVariable(((BooleanValueNode)node).Value);
			}
			else if (node is StringValueNode)
			{
				result = new ScriptVariable(((StringValueNode)node).Value);
			}
			else if (node is CharValueNode)
			{
				result = new ScriptVariable(((CharValueNode)node).Value);
			}
			else if (node is IntegerValueNode)
			{
				result = new ScriptVariable(((IntegerValueNode)node).Value);
			}
			else if (node is FloatValueNode)
			{
				result = new ScriptVariable(((FloatValueNode)node).Value);
			}

			if (result == null)
				this.ThrowUnableToExecuteException("Expression", node);

			return result;
		}
	}
}
