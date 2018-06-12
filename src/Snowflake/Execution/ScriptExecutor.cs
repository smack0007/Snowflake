﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public object Evaluate(ExpressionNode expression, ScriptExecutionContext context)
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

                case ArrayNode x: return this.EvaluateArray(x, context);
                case AssignmentOpeartionNode x: return this.EvaluateAssignmentOperation(x, context);
                case ElementAccessNode x: return this.EvaluateElementAccess(x, context);
                case FunctionNode x: return this.EvaluateFunction(x, context);
                case FunctionCallNode x: return this.EvaluateFunctionCall(x, context);
                case ListNode x: return this.EvaluateList(x, context);
                case OperationNode x: return this.EvaluateOperation(x, context);
                case UnaryOperationNode x: return this.EvaluateUnaryOperation(x, context);
                case VariableReferenceNode x: return context.GetVariableValue(x.VariableName);

                default:
                    throw new NotImplementedException($"{expression.GetType()} not implemented in {nameof(Evaluate)}.");
            }
        }

        private object[] EvaluateArray(ArrayNode array, ScriptExecutionContext context)
        {
            var value = new object[array.ValueExpressions.Count];

            for (int i = 0; i < array.ValueExpressions.Count; i++)
                value[i] = this.Evaluate(array.ValueExpressions[i], context);

            return value;
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
                        object currentValue = context.GetVariableValue(variableReference.VariableName);
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

        private object EvaluateElementAccess(ElementAccessNode elementAccess, ScriptExecutionContext context)
        {
            var source = this.Evaluate(elementAccess.SourceExpression, context);
            var element = this.Evaluate(elementAccess.ElementExpression, context);

            object value = null;

            if (source is Array array)
            {
                if (!(element is int index))
                    throw new ScriptExecutionException("Arrays can only be accessed via integers.", context.GetStackFrames());

                value = array.GetValue(index);
            }
            else if (source is ScriptList list)
            {
                if (!(element is int index))
                    throw new ScriptExecutionException("Lists can only be accessed via integers.", context.GetStackFrames());

                value = list[index];
            }
            else
            {
                throw new NotImplementedException($"Sources of type {source.GetType()} not implemented in {nameof(EvaluateElementAccess)}.");
            }

            return value;
        }

        private ScriptFunction EvaluateFunction(FunctionNode function, ScriptExecutionContext context)
        {
            var argumentNames = function.Args.Select(x => x.VariableName);
            
            var capturedVariables = function.BodyStatementBlock
                .FindChildren<VariableReferenceNode>()
                .Where(x =>
                    !argumentNames.Contains(x.VariableName) &&
                    context.TryGetVariable(x.VariableName, out var variable) &&
                    !variable.IsGlobal
                )
                .ToDictionary(x => x.VariableName, x => context.GetVariable(x.VariableName));

            return new ScriptFunction(this, function.Args, function.BodyStatementBlock, capturedVariables);
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

        private ScriptList EvaluateList(ListNode list, ScriptExecutionContext context)
        {
            ScriptList value = new ScriptList(list.ValueExpressions.Count);

            for (int i = 0; i < list.ValueExpressions.Count; i++)
                value.Add(this.Evaluate(list.ValueExpressions[i], context));

            return value;
        }

        private object EvaluateOperation(OperationNode operation, ScriptExecutionContext context)
        {
            var lhs = Evaluate(operation.LeftHand, context);
            
            // LogicalAnd and LogicalOr need shortcircuting support.
            switch (operation.Type)
            {
                case OperationType.LogicalAnd:
                    {
                        if (lhs is bool lhsBool)
                        {
                            if (!lhsBool)
                                return false;
                        }
                        else
                        {
                            throw new ScriptExecutionException($"Only booleans can be logically anded.", context.GetStackFrames());
                        }
                    }
                    break;


                case OperationType.LogicalOr:
                    {
                        if (lhs is bool lhsBool)
                        {
                            if (lhsBool)
                                return true;
                        }
                        else
                        {
                            throw new ScriptExecutionException($"Only booleans can be logically ored.", context.GetStackFrames());
                        }
                    }
                    break;
            }
            
            var rhs = Evaluate(operation.RightHand, context);

            switch (operation.Type)
            {
                case OperationType.Add: return this.Add(lhs, rhs, context);
                case OperationType.Divide: return this.Divide(lhs, rhs, context);
                case OperationType.Equals: return lhs.Equals(rhs);
                case OperationType.LogicalAnd: return this.LogicalAnd(lhs, rhs, context);
                case OperationType.LogicalOr: return this.LogicalOr(lhs, rhs, context);
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

        private object LogicalAnd(object lhs, object rhs, ScriptExecutionContext context)
        {
            if (lhs is bool lhsBool && rhs is bool rhsBool)
            {
                return lhsBool && rhsBool;
            }
            
            throw new ScriptExecutionException($"Only booleans can be logically anded.", context.GetStackFrames());
        }

        private object LogicalOr(object lhs, object rhs, ScriptExecutionContext context)
        {
            if (lhs is bool lhsBool && rhs is bool rhsBool)
            {
                return lhsBool || rhsBool;
            }
            
            throw new ScriptExecutionException($"Only booleans can be logically ored.", context.GetStackFrames());
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
