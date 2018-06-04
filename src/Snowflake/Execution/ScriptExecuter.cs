using System;
using Snowflake.Parsing;

namespace Snowflake.Execution
{
    public class ScriptExecuter
    {
        public object Execute(ScriptNode script, ScriptExecutionContext context)
        {
            ExecuteStatements(script.Statements, context);
            return null;
        }

        public object Execute(StatementBlockNode statementBlock, ScriptExecutionContext context)
        {
            ExecuteStatements(statementBlock.Statements, context);
            return null;
        }

        private object ExecuteStatements(SyntaxNodeCollection<StatementNode> statements, ScriptExecutionContext context)
        {
            bool shouldReturn = false;

            foreach (var statement in statements)
            {
                object result = ExecuteStatement(statement, context, ref shouldReturn);

                if (shouldReturn)
                    return result;
            }

            return null;
        }

        private object ExecuteStatement(StatementNode statement, ScriptExecutionContext context, ref bool shouldReturn)
        {
            object result = null;

            switch (statement)
            {
                case AssignmentOpeartionNode x:
                    EvaluateAssignmentOperation(x, context);
                    break;

                case ConstDeclarationNode x:
                    ExecuteConstDeclaration(x, context);
                    break;

                case FunctionCallNode x:
                    EvaluateFunctionCall(x, context);
                    break;

                case ReturnNode x:
                    result = Evaluate(x.ValueExpression, context);
                    shouldReturn = true;
                    break;

                case VariableDeclarationNode x:
                    ExecuteVariableDeclaration(x, context);
                    break;

                default:
                    throw new NotImplementedException($"{statement.GetType()} not implemented in {nameof(ExecuteStatement)}.");
            }

            return result;
        }

        private void ExecuteConstDeclaration(ConstDeclarationNode node, ScriptExecutionContext context)
        {
            context.DeclareVariable(node.ConstName, Evaluate(node.ValueExpression, context), true);
        }

        private void ExecuteVariableDeclaration(VariableDeclarationNode node, ScriptExecutionContext context)
        {
            context.DeclareVariable(node.VariableName, Evaluate(node.ValueExpression, context));
        }

        private object Evaluate(ExpressionNode expression, ScriptExecutionContext context)
        {
            switch (expression)
            {
                case BooleanValueNode x: return x.Value;
                case CharacterValueNode x: return x.Value;
                case DoubleValueNode x: return x.Value;
                case FloatValueNode x: return x.Value;
                case IntegerValueNode x: return x.Value;
                case NullValueNode x: return null;
                case StringValueNode x: return x.Value;

                case AssignmentOpeartionNode x: return EvaluateAssignmentOperation(x, context);
                case FunctionNode x: return EvaluateFunction(x, context);
                case FunctionCallNode x: return EvaluateFunctionCall(x, context);
                case OperationNode x: return EvaluateOperation(x, context);
                case VariableReferenceNode x: return EvaluateVariableReference(x, context);

                default:
                    throw new NotImplementedException($"{expression.GetType()} not implemented in {nameof(Evaluate)}.");
            }
        }

        private object EvaluateAssignmentOperation(AssignmentOpeartionNode assignment, ScriptExecutionContext context)
        {
            var value = Evaluate(assignment.ValueExpression, context);

            if (assignment.TargetExpression is VariableReferenceNode variableReference)
            {
                switch (assignment.Type)
                {
                    case AssignmentOperationType.Gets:
                        context.SetVariable(variableReference.VariableName, value);
                        break;

                    default:
                        throw new NotImplementedException($"{assignment.Type} not implemented in {nameof(EvaluateAssignmentOperation)} for {nameof(VariableReferenceNode)}.");
                }
            }
            else
            {
                throw new NotImplementedException($"{nameof(assignment)}.{nameof(assignment.TargetExpression)} not implemented in {nameof(EvaluateAssignmentOperation)}.");
            }

            return value;
        }

        private object EvaluateFunction(FunctionNode function, ScriptExecutionContext context)
        {
            return new ScriptFunction(this, function.Args, function.BodyStatementBlock);
        }

        private object EvaluateFunctionCall(FunctionCallNode functionCall, ScriptExecutionContext context)
        {
            var function = Evaluate(functionCall.FunctionExpression, context);

            bool canBeCalled = function is ScriptFunction || function is Delegate;

            if (!canBeCalled)
                throw new ScriptExecutionException($"Unable to execute function call on type '{function.GetType()}'.");

            var args = new object[functionCall.Args.Count];
            for (int i = 0; i < args.Length; i++)
                args[i] = Evaluate(functionCall.Args[i], context);

            if (function is ScriptFunction sf)
            {
                string stackFrameName = "<anonymous>";

                if (functionCall.FunctionExpression is VariableReferenceNode vr)
                {
                    stackFrameName = vr.VariableName;
                }

                return sf.Invoke(context, stackFrameName, args);
            }
            else if (function is Delegate d)
            {
                return d.DynamicInvoke(args);
            }

            throw new NotImplementedException($"Function call on type '{function.GetType()}' not implemented.");
        }

        private object EvaluateOperation(OperationNode operation, ScriptExecutionContext context)
        {
            var lhs = Evaluate(operation.LeftHand, context);
            var rhs = Evaluate(operation.RightHand, context);

            switch (operation.Type)
            {
                case OperationType.Add: return EvaluateAddOperation(lhs, rhs, context);

                default:
                    throw new NotImplementedException($"{operation.Type} not implemented in {nameof(EvaluateOperation)}.");
            }
        }

        private object EvaluateAddOperation(object lhs, object rhs, ScriptExecutionContext context)
        {
            if (lhs is int lhsInt)
            {
                if (rhs is int rhsInt)
                {
                    return lhsInt + rhsInt;
                }
                else if (rhs is float || rhs is double)
                {
                    return lhsInt + (int)rhs;
                }
            }
            else if (lhs is string lhsString)
            {
                if (rhs is string rhsString)
                {
                    return lhsString + rhsString;
                }
                else
                {
                    return lhsString + rhs.ToString();
                }
            }

            throw new NotImplementedException($"Add not implemented for {lhs.GetType()} and {rhs.GetType()} in {nameof(EvaluateAddOperation)}.");
        }

        private object EvaluateVariableReference(VariableReferenceNode variableReference, ScriptExecutionContext context)
        {
            return context.GetVariable(variableReference.VariableName);
        }
    }
}
