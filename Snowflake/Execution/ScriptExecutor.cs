﻿using System;
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
		        
		private void ThrowUnableToExecuteException(string executionStage, SyntaxNode node)
		{
            string message = string.Format(
                "Unable to execute node type {0} as {1}.",
                node.GetType().Name,
                executionStage,
                node.Line,
                node.Column);

            throw new ScriptExecutionException(message, node.Line, node.Column);
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
				ReturnValue = ScriptUndefined.Value
			};

			foreach (StatementNode statement in script.Statements)
			{
				this.ExecuteStatement(context, statement);

				if (context.ShouldReturn)
					break;
			}
				
			return context.ReturnValue;
		}

        private ScriptObject GetVariableValue(ExecutionContext context, string variableName)
        {
            ScriptVariableReference variable = context.Stack[variableName];

            if (variable == null)
                return ScriptUndefined.Value;

            return variable.Value;
        }

		private ScriptObject CallFunction(ExecutionContext context, ScriptFunction function, List<ScriptObject> args)
		{
			ScriptObject functionReturnValue = ScriptUndefined.Value;

			if (args.Count < function.Args.Length)
			{
				for (int i = args.Count; i < function.Args.Length; i++)
				{
					ScriptObject value = ScriptNull.Value;
					
					if (function.Args[i].DefaultValueExpression != null)
						value = this.ExecuteExpression(context, function.Args[i].DefaultValueExpression);
					
					args.Add(value);
				}
			}

			if (args.Count != function.Args.Length)
				throw new ScriptExecutionException("Invalid number of arguments specified when calling function.");

			context.Stack.Push();

			foreach (var pair in function.VariableReferences)
			{
				context.Stack[pair.Key] = pair.Value;
			}

			for (int i = 0; i < args.Count; i++)
			{
				context.Stack[function.Args[i].Name] = new ScriptVariableReference(args[i]);
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

		private ScriptObject CallClrMethod(ExecutionContext context, ScriptClrMethod function, IList<ScriptObject> args)
		{			
			object[] parameters = args.Select(x => x.Unbox()).ToArray();

			object result = null;

			try
			{
				result = function.Function.DynamicInvoke(parameters);
			}
			catch (TargetParameterCountException ex)
			{
				throw new ScriptExecutionException("Argument count for CLR method is wrong.", ex);
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
			else if (node is AssignmentNode)
			{
				this.ExecuteAssignment(context, (AssignmentNode)node);
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
				this.ThrowUnableToExecuteException("Statement", node);
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

		private void ExecuteAssignment(ExecutionContext context, AssignmentNode node)
		{
			var variable = context.Stack[node.VariableName];
			var value = this.ExecuteExpression(context, node.ValueExpression);

			switch (node.Operation)
			{
				case AssignmentOperation.AddGets:
					value = variable.Value.Add(value);
					break;

				case AssignmentOperation.SubtractGets:
					value = variable.Value.Subtract(value);
					break;

				case AssignmentOperation.MultiplyGets:
					value = variable.Value.Multiply(value);
					break;

				case AssignmentOperation.DivideGets:
					value = variable.Value.Divide(value);
					break;
			}

			variable.Gets(value);
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

		private ScriptObject ExecuteExpression(ExecutionContext context, ExpressionNode node)
		{
			ScriptObject result = null;

			if (node is FunctionNode)
			{
                result = this.ExecuteFunction(context, (FunctionNode)node);
			}
			else if (node is FunctionCallNode)
			{
                result = this.ExecuteFunctionCall(context, (FunctionCallNode)node);
			}
			else if (node is OperationNode)
			{
				result = this.ExecuteOperation(context, (OperationNode)node);
			}
			else if (node is VariableReferenceNode)
			{
				var variableReferenceNode = (VariableReferenceNode)node;
				
                result = context.Stack[variableReferenceNode.VariableName];

                if (result == null)
                    result = new ScriptVariableReference(ScriptUndefined.Value);
			}
            else if (node is UndefinedValueNode)
            {
                result = ScriptUndefined.Value;
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

        private ScriptObject ExecuteFunction(ExecutionContext context, FunctionNode node)
        {
            Dictionary<string, ScriptVariableReference> references = new Dictionary<string, ScriptVariableReference>();

            var capturedVariables = node.BodyStatementBlock
                .FindChildren<VariableReferenceNode>()
                .Where(
                    x => x.FindParent<FunctionNode>() == node &&
                    node.Args.SingleOrDefault(y => y.VariableName == x.VariableName) == null &&
                    node.FindChildren<VariableDeclarationNode>().SingleOrDefault(y => y.VariableName == x.VariableName) == null);

            foreach (var referenceNode in capturedVariables)
            {
                var variable = context.Stack[referenceNode.VariableName];

                if (variable == null)
                    variable = new ScriptVariableReference(ScriptUndefined.Value);

                references.Add(referenceNode.VariableName, variable);
            }

            var args = node.Args
                .Select(x => new ScriptFunction.Argument()
                {
                    Name = x.VariableName,
                    DefaultValueExpression = x.ValueExpression
                })
                .ToArray();

            return new ScriptFunction(node.BodyStatementBlock, args, references);
        }

        private ScriptObject ExecuteFunctionCall(ExecutionContext context, FunctionCallNode node)
        {
            List<ScriptObject> args = new List<ScriptObject>();
            foreach (ExpressionNode expression in node.Args)
            {
                args.Add(this.ExecuteExpression(context, expression));
            }

            ScriptObject function = this.ExecuteExpression(context, node.FunctionExpression);

            if (function is ScriptVariableReference)
                function = ((ScriptVariableReference)function).Value;

            ScriptObject result = null;

            if (function is ScriptFunction)
            {
                result = this.CallFunction(context, (ScriptFunction)function, args);
            }
            else if (function is ScriptClrMethod)
            {
                result = this.CallClrMethod(context, (ScriptClrMethod)function, args);
            }
            else
            {
                throw new ScriptExecutionException("Expression is not a function.");
            }

            return result;
        }

		private ScriptObject ExecuteOperation(ExecutionContext context, OperationNode node)
		{
			OperationNode operation = (OperationNode)node;
			ScriptObject lhs = this.ExecuteExpression(context, operation.LHS);
			ScriptObject rhs = this.ExecuteExpression(context, operation.RHS);

			if (lhs is ScriptVariableReference)
				lhs = ((ScriptVariableReference)lhs).Value;

			if (rhs is ScriptVariableReference)
				rhs = ((ScriptVariableReference)rhs).Value;

			ScriptObject result = null;

			switch (operation.Type)
			{
				case OperationType.Equals:
					result = lhs.EqualTo(rhs);
					break;

				case OperationType.NotEquals:
					result = lhs.EqualTo(rhs).Inverse();
					break;

				case OperationType.Add:
					result = lhs.Add(rhs);
					break;

				case OperationType.Subtract:
					result = lhs.Subtract(rhs);
					break;

				case OperationType.Multiply:
					result = lhs.Multiply(rhs);
					break;

				case OperationType.Divide:
					result = lhs.Divide(rhs);
					break;

				case OperationType.ConditionalOr:
					result = lhs.LogicalOr(rhs);
					break;

				case OperationType.ConditionalAnd:
					result = lhs.LogicalAnd(rhs);
					break;
			}

			return result;
		}
	}
}
