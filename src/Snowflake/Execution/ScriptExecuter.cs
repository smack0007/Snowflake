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
                case ConstDeclarationNode x:
                    ExecuteConstDeclaration(x, context);
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
                case IntegerValueNode x:
                    return EvaluateIntegerValue(x, context);

                case StringValueNode x:
                    return EvaluateStringValue(x, context);

                default:
                    throw new NotImplementedException($"{expression.GetType()} not implemented in {nameof(Evaluate)}.");
            }
        }

        private static object EvaluateIntegerValue(IntegerValueNode expression, ScriptExecutionContext context)
        {
            return expression.Value;
        }

        private static object EvaluateStringValue(StringValueNode expression, ScriptExecutionContext context)
        {
            return expression.Value;
        }
    }
}
