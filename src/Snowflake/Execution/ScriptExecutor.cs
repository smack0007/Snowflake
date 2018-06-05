using System;
using Snowflake.Parsing;

namespace Snowflake.Execution
{
    public class ScriptExecutor
    {
        public object Execute(ScriptNode script, ScriptExecutionContext context)
        {
            bool shouldReturn = false;
            return ExecuteStatements(script.Statements, context, ref shouldReturn);
        }

        private object ExecuteStatements(SyntaxNodeCollection<StatementNode> statements, ScriptExecutionContext context, ref bool shouldReturn)
        {
            foreach (var statement in statements)
            {
                object result = ExecuteStatement(statement, context, ref shouldReturn);

                if (shouldReturn)
                    return result;
            }

            return null;
        }

        public object Execute(StatementBlockNode statementBlock, ScriptExecutionContext context)
        {
            bool shouldReturn = false;
            return this.ExecuteStatementBlock(statementBlock, context, ref shouldReturn);
        }

        private object ExecuteStatementBlock(StatementBlockNode statementBlock, ScriptExecutionContext context, ref bool shouldReturn)
        {
            context.PushStackFrame("<scope>");

            object result = this.ExecuteStatements(statementBlock.Statements, context, ref shouldReturn);

            context.PopStackFrame();

            return result;
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

                case IfNode x:
                    result = ExecuteIf(x, context, ref shouldReturn);
                    break;

                case ReturnNode x:
                    result = Evaluate(x.ValueExpression, context);
                    shouldReturn = true;
                    break;

                case VariableDeclarationNode x:
                    ExecuteVariableDeclaration(x, context);
                    break;

                case WhileNode x:
                    result = ExecuteWhile(x, context, ref shouldReturn);
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
            object value = null;

            if (node.ValueExpression != null)
                value = Evaluate(node.ValueExpression, context);

            context.DeclareVariable(node.VariableName, value);
        }

        private object ExecuteIf(IfNode node, ScriptExecutionContext context, ref bool shouldReturn)
        {
            object evaluateResult = this.Evaluate(node.EvaluateExpression, context);

            if (!(evaluateResult is bool shouldExecute))
                throw new ScriptExecutionException("Evaluate expression of if resulted in non boolean type value.", context.GetStackFrames());

            if (shouldExecute)
            {
                var result = this.ExecuteStatementBlock(node.BodyStatementBlock, context, ref shouldReturn);

                if (shouldReturn)
                    return result;
            }
            else if (node.ElseStatementBlock != null)
            {
                var result = this.ExecuteStatementBlock(node.ElseStatementBlock, context, ref shouldReturn);

                if (shouldReturn)
                    return result;
            }

            return null;
        }

        private object ExecuteWhile(WhileNode node, ScriptExecutionContext context, ref bool shouldReturn)
        {
            while (true)
            {
                object evaluateResult = this.Evaluate(node.EvaluateExpression, context);

                if (!(evaluateResult is bool shouldExecute))
                    throw new ScriptExecutionException("Evaluate expression of while loop resulted in non boolean type value.", context.GetStackFrames());
                
                if (shouldExecute)
                {
                    var result = this.ExecuteStatementBlock(node.BodyStatementBlock, context, ref shouldReturn);

                    if (shouldReturn)
                        return result;
                }
                else
                {
                    break;
                }
            }

            return null;
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

                case AssignmentOpeartionNode x: return this.EvaluateAssignmentOperation(x, context);
                case FunctionNode x: return this.EvaluateFunction(x, context);
                case FunctionCallNode x: return this.EvaluateFunctionCall(x, context);
                case OperationNode x: return this.EvaluateOperation(x, context);
                case UnaryOperationNode x: return this.EvaluateUnaryOperation(x, context);
                case VariableReferenceNode x: return context.GetVariable(x.VariableName);

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

                    case AssignmentOperationType.AddGets:
                        object currentValue = context.GetVariable(variableReference.VariableName);
                        context.SetVariable(variableReference.VariableName, this.Add(currentValue, value, context));
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
                try
                {
                    return d.DynamicInvoke(args);
                }
                catch (Exception ex)
                {
                    throw new ScriptExecutionException("Failed while executing function call to delegate.", context.GetStackFrames(), ex);
                }
            }

            throw new NotImplementedException($"Function call on type '{function.GetType()}' not implemented.");
        }

        private object EvaluateOperation(OperationNode operation, ScriptExecutionContext context)
        {
            var lhs = Evaluate(operation.LeftHand, context);
            var rhs = Evaluate(operation.RightHand, context);

            switch (operation.Type)
            {
                case OperationType.Add: return this.Add(lhs, rhs, context);
                case OperationType.ConditionalAnd: return this.ConditionalAnd(lhs, rhs, context);
                case OperationType.ConditionalOr: return this.ConditionalOr(lhs, rhs, context);
                case OperationType.Divide: return this.Divide(lhs, rhs, context);
                case OperationType.Equals: return lhs.Equals(rhs);
                case OperationType.Multiply: return this.Multiply(lhs, rhs, context);
                case OperationType.NotEquals: return !lhs.Equals(rhs);
                case OperationType.Subtract: return this.Subtract(lhs, rhs, context);

                default:
                    throw new NotImplementedException($"{operation.Type} not implemented in {nameof(EvaluateOperation)}.");
            }
        }

        private object Add(object lhs, object rhs, ScriptExecutionContext context)
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

            throw new NotImplementedException($"{nameof(Add)} not implemented for {lhs.GetType()} and {rhs.GetType()}.");
        }

        private object ConditionalAnd(object lhs, object rhs, ScriptExecutionContext context)
        {
            if (lhs is bool lhsBool && rhs is bool rhsBool)
            {
                return lhsBool && rhsBool;
            }
            
            throw new ScriptExecutionException($"Only booleans can be conditionally anded.", context.GetStackFrames());
        }

        private object ConditionalOr(object lhs, object rhs, ScriptExecutionContext context)
        {
            if (lhs is bool lhsBool && rhs is bool rhsBool)
            {
                return lhsBool || rhsBool;
            }
            
            throw new ScriptExecutionException($"Only booleans can be conditionally ored.", context.GetStackFrames());
        }

        private object Divide(object lhs, object rhs, ScriptExecutionContext context)
        {
            if (lhs is int lhsInt)
            {
                if (rhs is int rhsInt)
                {
                    return lhsInt / rhsInt;
                }
                else if (rhs is float || rhs is double)
                {
                    return lhsInt / (int)rhs;
                }
            }
            else if (lhs is string lhsString)
            {
                throw new ScriptExecutionException("Strings cannot be divided.", context.GetStackFrames());
            }

            throw new NotImplementedException($"{nameof(Divide)} not implemented for {lhs.GetType()} and {rhs.GetType()}.");
        }

        private object Multiply(object lhs, object rhs, ScriptExecutionContext context)
        {
            if (lhs is int lhsInt)
            {
                if (rhs is int rhsInt)
                {
                    return lhsInt * rhsInt;
                }
                else if (rhs is float || rhs is double)
                {
                    return lhsInt * (int)rhs;
                }
            }
            else if (lhs is string lhsString)
            {
                throw new ScriptExecutionException("Strings cannot be multiplied.", context.GetStackFrames());
            }

            throw new NotImplementedException($"{nameof(Multiply)} not implemented for {lhs.GetType()} and {rhs.GetType()}.");
        }

        private object Subtract(object lhs, object rhs, ScriptExecutionContext context)
        {
            if (lhs is int lhsInt)
            {
                if (rhs is int rhsInt)
                {
                    return lhsInt - rhsInt;
                }
                else if (rhs is float || rhs is double)
                {
                    return lhsInt - (int)rhs;
                }
            }
            else if (lhs is string lhsString)
            {
                throw new ScriptExecutionException("Strings cannot be subtracted.", context.GetStackFrames());
            }

            throw new NotImplementedException($"{nameof(Subtract)} not implemented for {lhs.GetType()} and {rhs.GetType()}.");
        }

        private object EvaluateUnaryOperation(UnaryOperationNode operation, ScriptExecutionContext context)
        {
            var value = Evaluate(operation.ValueExpression, context);

            switch (operation.Type)
            {
                case UnaryOperationType.Decrement:
                {
                    var result = this.Decrement(value, context);

                    if (operation.ValueExpression is VariableReferenceNode vr)
                        context.SetVariable(vr.VariableName, result);

                    return result;
                }
                
                case UnaryOperationType.Increment:
                {
                    var result = this.Increment(value, context);

                    if (operation.ValueExpression is VariableReferenceNode vr)
                        context.SetVariable(vr.VariableName, result);

                    return result;
                }
            
                case UnaryOperationType.LogicalNegate: return this.LogicalNegate(value, context);
                case UnaryOperationType.Negate: return this.Negate(value, context);

                default:
                    throw new NotImplementedException($"{operation.Type} not implemented in {nameof(EvaluateUnaryOperation)}.");
            }
        }
        
        private object Decrement(object value, ScriptExecutionContext context)
        {
            if (value is int valueInt)
            {
                return --valueInt;
            }
            
            throw new ScriptExecutionException("Only integers can be decremented.", context.GetStackFrames());
        }

        private object Increment(object value, ScriptExecutionContext context)
        {
            if (value is int valueInt)
            {
                return ++valueInt;
            }
            
            throw new ScriptExecutionException("Only integers can be incremented.", context.GetStackFrames());
        }

        private object LogicalNegate(object value, ScriptExecutionContext context)
        {
            if (value is bool valueBool)
            {
                return !valueBool;
            }
            
            throw new ScriptExecutionException("Only booleans can be logically negated.", context.GetStackFrames());
        }

        private object Negate(object value, ScriptExecutionContext context)
        {
            if (value is bool valueBool)
            {
                throw new ScriptExecutionException("Booleans cannot be negated.", context.GetStackFrames());
            }
            else if (value is int valueInt)
            {
                return -valueInt;
            }
            else if (value is int valueDouble)
            {
                return -valueDouble;
            }
            else if (value is int valueFloat)
            {
                return -valueFloat;
            }
            else if (value is string valueString)
            {
                throw new ScriptExecutionException("Strings cannot be negated.", context.GetStackFrames());
            }

            throw new NotImplementedException($"{nameof(Negate)} not implemented for {value.GetType()}.");
        }
    }
}
