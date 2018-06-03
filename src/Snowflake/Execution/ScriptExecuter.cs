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

        private static void ExecuteStatements(SyntaxNodeCollection<StatementNode> statements, ScriptExecutionContext context)
        {
            foreach (var statement in statements)
            {
                ExecuteStatement(statement, context);
            }
        }

        private static void ExecuteStatement(StatementNode statement, ScriptExecutionContext context)
        {
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

                case VariableDeclarationNode x:
                    ExecuteVariableDeclaration(x, context);
                    break;

                default:
                    throw new NotImplementedException($"{statement.GetType()} not implemented in {nameof(ExecuteStatement)}.");
            }
        }

        private static void ExecuteConstDeclaration(ConstDeclarationNode node, ScriptExecutionContext context)
        {
            context.DeclareVariable(node.ConstName, Evaluate(node.ValueExpression, context), true);
        }

        private static void ExecuteVariableDeclaration(VariableDeclarationNode node, ScriptExecutionContext context)
        {
            context.DeclareVariable(node.VariableName, Evaluate(node.ValueExpression, context));
        }

        private static object Evaluate(ExpressionNode expression, ScriptExecutionContext context)
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
                case FunctionCallNode x: return EvaluateFunctionCall(x, context);
                case OperationNode x: return EvaluateOperation(x, context);
                case VariableReferenceNode x: return EvaluateVariableReference(x, context);

                default:
                    throw new NotImplementedException($"{expression.GetType()} not implemented in {nameof(Evaluate)}.");
            }
        }

        private static object EvaluateAssignmentOperation(AssignmentOpeartionNode assignment, ScriptExecutionContext context)
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

        private static object EvaluateFunctionCall(FunctionCallNode functionCall, ScriptExecutionContext context)
        {
            var function = Evaluate(functionCall.FunctionExpression, context);

            bool canBeCalled = function is Delegate;

            if (!canBeCalled)
                throw new ScriptExecutionException($"Unable to execute function call on type '{function.GetType()}'.");

            var args = new object[functionCall.Args.Count];
            for (int i = 0; i < args.Length; i++)
                args[i] = Evaluate(functionCall.Args[i], context);

            if (function is Delegate d)
            {
                return d.DynamicInvoke(args);
            }

            throw new NotImplementedException($"Function call on type '{function.GetType()}' not implemented.");
        }

        private static object EvaluateOperation(OperationNode operation, ScriptExecutionContext context)
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

        private static object EvaluateAddOperation(object lhs, object rhs, ScriptExecutionContext context)
        {
            if (lhs is int lhsInt)
            {
                if (rhs is int rhsInt)
                {
                    return lhsInt + rhsInt;
                }
            }

            throw new NotImplementedException($"Add not implemented for {lhs.GetType()} and {rhs.GetType()} in {nameof(EvaluateAddOperation)}.");
        }

        private static object EvaluateVariableReference(VariableReferenceNode variableReference, ScriptExecutionContext context)
        {
            return context.GetVariable(variableReference.VariableName);
        }
    }
}
