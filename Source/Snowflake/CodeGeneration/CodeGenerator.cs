using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Snowflake.Parsing;

namespace Snowflake.CodeGeneration
{
	public class CodeGenerator
	{
		public const string GeneratedCodeNamespace = "Snowflake.Generated";

		class DataContext
		{
			public StringBuilder Code
			{
				get;
				private set;
			}

			public int Padding
			{
				get;
				set;
			}

			public bool DisableNewLines
			{
				get;
				set;
			}

			public int FunctionDepth
			{
				get;
				set;
			}

			public VariableMap VariableMap
			{
				get;
				private set;
			}

			public DataContext()
			{
				this.Code = new StringBuilder();
				this.VariableMap = new VariableMap();
			}
		}

		public CodeGenerator()
		{
		}

		private static void StartLine(DataContext data)
		{
			StartLine(data, "");
		}

		private static void StartLine(DataContext data, string format, params object[] args)
		{
			if (!data.DisableNewLines)
			{
				for (int i = 0; i < data.Padding; i++)
					data.Code.Append("\t");
			}

			data.Code.Append(string.Format(format, args));
		}

		private static void Append(DataContext data, string format, params object[] args)
		{
			data.Code.Append(string.Format(format, args));
		}

		private static void EndLine(DataContext data)
		{
			EndLine(data, "");
		}

		private static void EndLine(DataContext data, string format, params object[] args)
		{
			if (!data.DisableNewLines)
			{
				data.Code.AppendLine(string.Format(format, args));
			}
			else
			{
				data.Code.Append(string.Format(format, args));
			}
		}

		private static void WriteLine(DataContext data)
		{
			WriteLine(data, "");
		}

		private static void WriteLine(DataContext data, string format, params object[] args)
		{
			StartLine(data, format, args);

			if (!data.DisableNewLines)
			{
				data.Code.AppendLine();
			}
		}

		private static void DeleteCharacters(DataContext data, int count)
		{
			data.Code.Remove(data.Code.Length - count, count);
		}

		public string Generate(ScriptNode syntaxTree)
		{
			DataContext data = new DataContext();

			WriteLine(data, "using System;");
			WriteLine(data, "using System.Collections.Generic;");
			WriteLine(data);

			WriteLine(data, "namespace {0}", GeneratedCodeNamespace);
			WriteLine(data, "{{");

			data.Padding++;
			WriteLine(data, "public class {0}Script : Script", syntaxTree.Id);
			WriteLine(data, "{{");

			data.Padding++;
			WriteLine(data, "protected override object Execute()");
			WriteLine(data, "{{");
									
			data.Padding++;
			WriteLine(data, "var stack = new Stack<ScriptStackFrame>();");

			WriteLine(data, "try");
			WriteLine(data, "{{");

			data.Padding++;
			foreach (var statement in syntaxTree.Statements)
			{
				GenerateStatement(statement, data);
			}

			if (!(syntaxTree.Statements.Last() is ReturnNode))
				WriteLine(data, "return null;");

			data.Padding--;
			WriteLine(data, "}}");
			WriteLine(data, "catch (Exception ex)");
			WriteLine(data, "{{");

			data.Padding++;
			WriteLine(data, "throw new ScriptExecutionException(ex.Message, ex, this.Id, stack);");

			data.Padding--;
			WriteLine(data, "}}");

			data.Padding--;
			WriteLine(data, "}}");

			data.Padding--;
			WriteLine(data, "}}");

			data.Padding--;
			WriteLine(data, "}}");

			return data.Code.ToString();
		}

		private static void ThrowUnableToGenerateException(string generationStage, SyntaxNode node)
		{
			string message = string.Format(
				"Unable to generate node type {0} as {1}.",
				node.GetType().Name,
				generationStage);

			throw new CodeGenerationException(message, node.Line, node.Column);
		}

		private static void GenerateStatementBlock(StatementBlockNode statementBlock, DataContext data, bool pushStackFrame = true)
		{
			if (pushStackFrame)
				data.VariableMap.PushFrame();

			foreach (var statement in statementBlock)
			{
				GenerateStatement(statement, data);
			}

			if (pushStackFrame)
				data.VariableMap.PopFrame();
		}

		private static void GenerateStatement(StatementNode node, DataContext data)
		{
			if (node is VariableDeclarationNode)
			{
				GenerateVariableDeclaration((VariableDeclarationNode)node, data);
			}
			else if (node is FunctionNode && !((FunctionNode)node).IsAnonymous)
			{
				GenerateFunctionDeclaration((FunctionNode)node, data);
			}
			else if (node is IfNode)
			{
				GenerateIf((IfNode)node, data);
			}
			else if (node is WhileNode)
			{
				GenerateWhile((WhileNode)node, data);
			}
			else if (node is ForNode)
			{
				GenerateFor((ForNode)node, data);
			}
			else if (node is ForEachNode)
			{
				GenerateForEach((ForEachNode)node, data);
			}
			else if (node is ReturnNode)
			{
				GenerateReturn((ReturnNode)node, data);
			}
			else if (node is ExpressionNode)
			{
				StartLine(data);
				GenerateExpression((ExpressionNode)node, data);
				EndLine(data, ";");
			}
			else
			{
				ThrowUnableToGenerateException("Statement", node);
			}
		}

		private static void GenerateVariableDeclaration(VariableDeclarationNode node, DataContext data)
		{
			string anonymousVariableName = data.VariableMap.DeclarVariable(node.VariableName, node.Line, node.Column);
			StartLine(data, "dynamic {0}", anonymousVariableName);

			if (node.ValueExpression != null)
			{
				Append(data, " = ");
				GenerateExpression(node.ValueExpression, data);
			}

			EndLine(data, ";");
		}

		private static void GenerateFunctionDeclaration(FunctionNode node, DataContext data)
		{
			string anonymousVariableName = data.VariableMap.DeclarVariable(node.FunctionName, node.Line, node.Column);
			StartLine(data, "dynamic {0} = ", anonymousVariableName);

			GenerateFunction(node, data);

			EndLine(data, ";");
		}

		private static void GenerateIf(IfNode node, DataContext data)
		{
			StartLine(data, "if (");

			GenerateExpression(node.EvaluateExpression, data);

			EndLine(data, ") {{");

			data.Padding++;
			GenerateStatementBlock(node.BodyStatementBlock, data);
						
			if (node.ElseStatementBlock != null)
			{
				data.Padding--;
				WriteLine(data, "}} else {{");

				data.Padding++;
				GenerateStatementBlock(node.ElseStatementBlock, data);
			}

			data.Padding--;
			WriteLine(data, "}}");
		}

		private static void GenerateWhile(WhileNode node, DataContext data)
		{
			StartLine(data, "while (");

			GenerateExpression(node.EvaluateExpression, data);

			EndLine(data, ") {{");

			data.Padding++;
			GenerateStatementBlock(node.BodyStatementBlock, data);

			data.Padding--;
			WriteLine(data, "}}");
		}

		private static void GenerateFor(ForNode node, DataContext data)
		{
			StartLine(data, "for (");

			data.DisableNewLines = true;

			if (node.InitializeSyntax is VariableDeclarationNode)
			{
				GenerateStatement((VariableDeclarationNode)node.InitializeSyntax, data);
			}
			else if (node.InitializeSyntax is ExpressionNode)
			{
				GenerateExpression((ExpressionNode)node.InitializeSyntax, data);
			}
			else
			{
				ThrowUnableToGenerateException("For initializer", node.InitializeSyntax);
			}

			Append(data, " ");

			GenerateExpression(node.EvaluateExpression, data);
			Append(data, "; ");

			GenerateExpression(node.IncrementExpression, data);

			data.DisableNewLines = false;

			EndLine(data, ") {{");

			data.Padding++;
			GenerateStatementBlock(node.BodyStatementBlock, data);

			data.Padding--;
			WriteLine(data, "}}");
		}

		private static void GenerateForEach(ForEachNode node, DataContext data)
		{
			StartLine(data, "foreach (");

			data.DisableNewLines = true;
			GenerateVariableDeclaration(node.VariableDeclaration, data);
			DeleteCharacters(data, 1);

			Append(data, " in ");

			GenerateExpression(node.SourceExpression, data);
						
			data.DisableNewLines = false;

			EndLine(data, ") {{");

			data.Padding++;
			GenerateStatementBlock(node.BodyStatementBlock, data);

			data.Padding--;
			WriteLine(data, "}}");
		}

		private static void GenerateReturn(ReturnNode node, DataContext data)
		{
			StartLine(data, "return ");

			GenerateExpression(node.Expression, data);

			EndLine(data, ";");
		}

		private static void GenerateExpression(ExpressionNode node, DataContext data)
		{		
			if (node is AssignmentOpeartionNode)
			{
				GenerateAssignmentOperation((AssignmentOpeartionNode)node, data);
			}
			else if (node is FunctionNode)
			{
				GenerateFunction((FunctionNode)node, data);
			}
			else if (node is FunctionCallNode)
			{
				GenerateFunctionCall((FunctionCallNode)node, data);
			}
			else if (node is MemberAccessNode)
			{
				GenerateMemberAccess((MemberAccessNode)node, data);
			}
			else if (node is ElementAccessNode)
			{
				GenerateElementAccess((ElementAccessNode)node, data);
			}
			else if (node is OperationNode)
			{
				GenerateOperation((OperationNode)node, data);
			}
			else if (node is UnaryOperationNode)
			{
				GenerateUnaryOperation((UnaryOperationNode)node, data);
			}
			else if (node is ListNode)
			{
				GenerateList((ListNode)node, data);
			}
			else if (node is ArrayNode)
			{
				GenerateArray((ArrayNode)node, data);
			}
			else if (node is VariableReferenceNode)
			{
				var variableReferenceNode = (VariableReferenceNode)node;
				Append(data, data.VariableMap.GetVariableName(variableReferenceNode.VariableName));
			}
			else if (node is NullValueNode)
			{
				Append(data, "null");
			}
			else if (node is UndefinedValueNode)
			{
				Append(data, "ScriptUndefined.Value");
			}
			else if (node is BooleanValueNode)
			{
				if (((BooleanValueNode)node).Value)
				{
					Append(data, "true");
				}
				else
				{
					Append(data, "false");
				}
			}
			else if (node is StringValueNode)
			{
				Append(data, "\"{0}\"", ((StringValueNode)node).Value);
			}
			else if (node is CharacterValueNode)
			{
				Append(data, "'{0}'", ((CharacterValueNode)node).Value);
			}
			else if (node is IntegerValueNode)
			{
				Append(data, ((IntegerValueNode)node).Value.ToString());
			}
			else if (node is FloatValueNode)
			{
				Append(data, ((FloatValueNode)node).Value.ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				ThrowUnableToGenerateException("Expression", node);
			}
		}

		private static void GenerateAssignmentOperation(AssignmentOpeartionNode node, DataContext data)
		{
			GenerateExpression(node.TargetExpression, data);

			switch (node.Type)
			{
				case AssignmentOperationType.Gets:
					Append(data, " = ");
					break;

				case AssignmentOperationType.AddGets:
					Append(data, " += ");
					break;

				case AssignmentOperationType.SubtractGets:
					Append(data, " -= ");
					break;

				case AssignmentOperationType.MultiplyGets:
					Append(data, " *= ");
					break;

				case AssignmentOperationType.DivideGets:
					Append(data, " /= ");
					break;
			}
			
			GenerateExpression(node.ValueExpression, data);
		}

		private static void GenerateFunction(FunctionNode node, DataContext data)
		{
			Append(data, "new ScriptFunction(new Func<");

			Append(data, string.Join(", ", Enumerable.Repeat("dynamic", node.Args.Count + 1)));
					
			Append(data, ">((");

			data.VariableMap.PushFrame();

			Append(data, string.Join(", ", node.Args.Select(x => data.VariableMap.DeclarVariable(x.VariableName, x.Line, x.Column))));
						
			Append(data, ") => {{ ");
			
			EndLine(data);

			data.Padding++;

			if (!node.IsAnonymous)
			{
				WriteLine(data, "stack.Push(new ScriptStackFrame(\"{0}\"));", node.FunctionName);
				WriteLine(data, "try {{");

				data.Padding++;
			}

			GenerateStatementBlock(node.BodyStatementBlock, data, pushStackFrame: false);
			
			if (!(node.BodyStatementBlock.Last() is ReturnNode))
				WriteLine(data, "return null;");

			if (!node.IsAnonymous)
			{
				data.Padding--;
				WriteLine(data, "}} finally {{");

				data.Padding++;
				WriteLine(data, "stack.Pop();");

				data.Padding--;
				WriteLine(data, "}}");
			}

			data.Padding--;

			StartLine(data, " }})");

			if (node.Args.Count != 0)
			{
				foreach (var arg in node.Args)
				{
					Append(data, ", ");

					if (arg.ValueExpression != null)
					{
						GenerateExpression(arg.ValueExpression, data);
					}
					else
					{
						Append(data, "null");
					}
				}
			}

			Append(data, ")");

			data.VariableMap.PopFrame();
		}

		private static void GenerateFunctionCall(FunctionCallNode node, DataContext data)
		{
			Append(data, "Invoke(");

			GenerateExpression(node.FunctionExpression, data);

			for (int i = 0; i < node.Args.Count; i++)
			{
				Append(data, ", ");
				GenerateExpression(node.Args[i], data);
			}

			Append(data, ")");
		}

		private static void GenerateMemberAccess(MemberAccessNode node, DataContext data)
		{
			GenerateExpression(node.SourceExpression, data);
			Append(data, ".{0}", node.MemberName);
		}

		private static void GenerateElementAccess(ElementAccessNode node, DataContext data)
		{
			GenerateExpression(node.SourceExpression, data);
			Append(data, "[");
			GenerateExpression(node.ElementExpression, data);
			Append(data, "]");
		}

		private static void GenerateList(ListNode node, DataContext data)
		{
			Append(data, "new List<dynamic> {{ ");

			for (int i = 0; i < node.ValueExpressions.Count; i++)
			{
				if (i != 0)
					Append(data, ", ");

				GenerateExpression(node.ValueExpressions[i], data);
			}

			Append(data, " }}");
		}

		private static void GenerateArray(ArrayNode node, DataContext data)
		{
			Append(data, "new dynamic[{0}] {{ ", node.ValueExpressions.Count);

			for (int i = 0; i < node.ValueExpressions.Count; i++)
			{
				if (i != 0)
					Append(data, ", ");

				GenerateExpression(node.ValueExpressions[i], data);
			}

			Append(data, " }}");
		}

		private static void GenerateOperation(OperationNode node, DataContext data)
		{
			Append(data, "(");

			OperationNode operation = (OperationNode)node;

			GenerateExpression(node.LeftHand, data);
					
			switch (operation.Type)
			{
				case OperationType.Equals:
					Append(data, " == ");
					break;

				case OperationType.NotEquals:
					Append(data, " != ");
					break;

				case OperationType.GreaterThan:
					Append(data, " > ");
					break;

				case OperationType.GreaterThanOrEqualTo:
					Append(data, " >= ");
					break;

				case OperationType.LessThan:
					Append(data, " < ");
					break;

				case OperationType.LessThanOrEqualTo:
					Append(data, " <= ");
					break;

				case OperationType.Add:
					Append(data, " + ");
					break;

				case OperationType.Subtract:
					Append(data, " - ");
					break;

				case OperationType.Multiply:
					Append(data, " * ");
					break;

				case OperationType.Divide:
					Append(data, " / ");
					break;

				case OperationType.ConditionalOr:
					Append(data, " || ");
					break;

				case OperationType.ConditionalAnd:
					Append(data, " && ");
					break;

				default:
					ThrowUnableToGenerateException("Operation", node);
					break;
			}

			GenerateExpression(node.RightHand, data);

			Append(data, ")");
		}

		private static void GenerateUnaryOperation(UnaryOperationNode node, DataContext data)
		{
			Append(data, "(");
			
			switch (node.Type)
			{
				case UnaryOperationType.Negate:
					Append(data, "-");
					break;

				case UnaryOperationType.LogicalNegate:
					Append(data, "!");
					break;
			}
			
			GenerateExpression(((UnaryOperationNode)node).ValueExpression, data);
			Append(data, ")");
		}
	}
}
