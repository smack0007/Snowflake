using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowsoft.SnowflakeScript.Parsing;
using System.Reflection;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptExecutor
	{
		class ExecutionContext
		{
			public ScriptStack Stack;
			public ScriptTypeBoxer Boxer;
			public bool ShouldReturn;
			public ScriptObject ReturnValue;
		}

		public ScriptExecutor()
		{
		}
				
		private void ThrowUnableToExecuteException(string executionStage, SyntaxTreeNode node)
		{
			throw new ScriptExecutionException("Unable to execute node type " + node.GetType().Name + " at " + executionStage + ".");
		}

		public ScriptObject Execute(ScriptNode script, ScriptStack stack, ScriptTypeBoxer boxer)
		{
			if (script == null)
				throw new ArgumentNullException("script");

			if (stack == null)
				throw new ArgumentNullException("stack");

			if (boxer == null)
				throw new ArgumentNullException("boxer");

			var context = new ExecutionContext()
			{
				Stack = stack,
				Boxer = boxer,
				ReturnValue = ScriptNull.Value
			};

			foreach (StatementNode statement in script.Statements)
			{
				this.ExecuteStatement(context, statement);

				if (context.ShouldReturn)
					break;
			}
				
			return context.ReturnValue;
		}

		private ScriptObject CallFunction(ExecutionContext context, string functionName, ScriptFunction function, IList<ScriptObject> args)
		{
			ScriptObject functionReturnValue = ScriptNull.Value;
				
			if (args != null && args.Count != function.Args.Length)
				throw new ScriptExecutionException(string.Format("Invalid number of arguments specified when calling \"{0}\".", functionName));

			context.Stack.Push();
						
			for (int i = 0; i < args.Count; i++)
			{
				context.Stack[function.Args[i]] = new ScriptVariableReference(args[i]);
			}

			context.ShouldReturn = false;
			context.ReturnValue = null;

			foreach (StatementNode statement in function.StatementBlock.Statements)
			{
				this.ExecuteStatement(context, statement);

				if (context.ShouldReturn)
				{
					functionReturnValue = context.ReturnValue;

					context.ReturnValue = ScriptNull.Value;
					context.ShouldReturn = false;

					break;
				}
			}

			context.Stack.Pop();
				
			return functionReturnValue;
		}

		private ScriptObject CallClrMethod(ExecutionContext context, string functionName, ScriptClrMethod function, IList<ScriptObject> args)
		{			
			object[] parameters = args.Select(x => x.Unbox()).ToArray();

			object result = null;

			try
			{
				result = function.Function.DynamicInvoke(parameters);
			}
			catch (TargetParameterCountException ex)
			{
				throw new ScriptExecutionException(string.Format("Argument count for function \"{0}\" is wrong.", functionName), ex);
			}
			catch (ArgumentException ex)
			{
				throw new ScriptExecutionException(string.Format("Argument type for \"{0}\" is wrong.", ex.ParamName), ex);
			}

			return context.Boxer.Box(result);
		}
				
		private void ExecuteStatement(ExecutionContext context, StatementNode node)
		{
			if (node is VariableDeclarationNode)
			{
				this.ExecuteVariableDeclaration(context, (VariableDeclarationNode)node);
			}
			else if (node is IfNode)
			{
				this.ExecuteIf(context, (IfNode)node);
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
			if (context.Stack.IsDeclaredInFrame(node.VariableName))
				throw new ScriptExecutionException(string.Format("Variable \"{0}\" is already declared.", node.VariableName));

			ScriptObject value = null;

			if (node.ValueExpression != null)
			{
				value = this.ExecuteExpression(context, node.ValueExpression);
			}
			else
			{
				value = ScriptNull.Value;
			}

			context.Stack[node.VariableName] = new ScriptVariableReference(value);
		}

		private void ExecuteIf(ExecutionContext context, IfNode node)
		{
			var result = this.ExecuteExpression(context, node.EvaluateExpression).Unbox();

			if (!(result is bool))
			{
				throw new ScriptExecutionException("Evaluate expression of if must result in a boolean value.");
			}
						
			if ((bool)result)
			{
				foreach (var statement in node.BodyStatementBlock.Statements)
				{
					this.ExecuteStatement(context, statement);

					if (context.ShouldReturn)
						return;
				}
			}
			else if (node.ElseStatementBlock != null)
			{
				foreach (var statement in node.ElseStatementBlock.Statements)
				{
					this.ExecuteStatement(context, statement);

					if (context.ShouldReturn)
						return;
				}	
			}
		}

		private void ExecuteReturn(ExecutionContext context, ReturnNode node)
		{
			// It's important to store the result before setting any of the context values because
			// the expression itself could change the context values.
			var result = this.ExecuteExpression(context, node.Expression);

			context.ShouldReturn = true;
			context.ReturnValue = result;
		}

		private ScriptVariableReference GetVariableRefernce(ExecutionContext context, string variableName)
		{
			ScriptVariableReference variable = context.Stack[variableName];

			if (variable == null)
				throw new ScriptExecutionException(string.Format("\"{0}\" is not defined.", variableName));

			return variable;
		}

		private ScriptObject ExecuteExpression(ExecutionContext context, ExpressionNode node)
		{
			ScriptObject result = null;

			if (node is FunctionNode)
			{
				FunctionNode functionNode = (FunctionNode)node;
				result = new ScriptFunction(functionNode.BodyStatementBlock, functionNode.Args.ToArray());
			}
			else if (node is FunctionCallNode)
			{
				FunctionCallNode functionCall = (FunctionCallNode)node;

				List<ScriptObject> args = new List<ScriptObject>();
				foreach (ExpressionNode expression in functionCall.Args)
				{
					args.Add(this.ExecuteExpression(context, expression));
				}

				var variable = this.GetVariableRefernce(context, functionCall.FunctionName);

				if (variable.Value is ScriptFunction)
				{
					result = this.CallFunction(context, functionCall.FunctionName, (ScriptFunction)variable.Value, args);
				}
				else if (variable.Value is ScriptClrMethod)
				{
					result = this.CallClrMethod(context, functionCall.FunctionName, (ScriptClrMethod)variable.Value, args);
				}
				else
				{
					throw new ScriptExecutionException(string.Format("\"{0}\" is not a function.", functionCall.FunctionName));
				}
			}
			else if (node is OperationNode)
			{
				OperationNode operation = (OperationNode)node;
				ScriptObject lhs = this.ExecuteExpression(context, operation.LHS);
				ScriptObject rhs = this.ExecuteExpression(context, operation.RHS);

				if (operation.Type == OperationType.Gets)
				{
					lhs.Gets(rhs);
					result = lhs;
				}
				else
				{
					if (lhs is ScriptVariableReference)
						lhs = ((ScriptVariableReference)lhs).Value;

					if (rhs is ScriptVariableReference)
						rhs = ((ScriptVariableReference)rhs).Value;

					switch (operation.Type)
					{
						case OperationType.Add:
							result = lhs.Add(rhs);
							break;

						case OperationType.Subtract:
							result = lhs.Subtract(rhs);
							break;

						case OperationType.LogicalAnd:
							result = lhs.LogicalAnd(rhs);
							break;

						case OperationType.LogicalOr:
							result = lhs.LogicalOr(rhs);
							break;
					}
				}
			}
			else if (node is VariableReferenceNode)
			{
				var variableReferenceNode = (VariableReferenceNode)node;

				result = context.Stack[variableReferenceNode.VariableName];

				if (result == null)
					throw new ScriptExecutionException(string.Format("Variable \"{0}\" is not defined.", variableReferenceNode.VariableName));
			}
			else if (node is NullValueNode)
			{
				result = ScriptNull.Value;
			}
			else if (node is BooleanValueNode)
			{
				if (((BooleanValueNode)node).Value)
				{
					result = ScriptBoolean.True;
				}
				else
				{
					result = ScriptBoolean.False;
				}
			}
			else if (node is StringValueNode)
			{
				result = new ScriptString(((StringValueNode)node).Value);
			}
			else if (node is CharacterValueNode)
			{
				result = new ScriptCharacter(((CharacterValueNode)node).Value);
			}
			else if (node is IntegerValueNode)
			{
				result = new ScriptInteger(((IntegerValueNode)node).Value);
			}
			else if (node is FloatValueNode)
			{
				result = new ScriptFloat(((FloatValueNode)node).Value);
			}

			if (result == null)
				this.ThrowUnableToExecuteException("Expression", node);

			return result;
		}
	}
}
