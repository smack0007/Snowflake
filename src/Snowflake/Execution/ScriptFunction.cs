using System.Collections.Generic;
using Snowflake.Parsing;

namespace Snowflake.Execution
{
    public sealed class ScriptFunction
	{
		private readonly ScriptExecutor executor;

		private readonly StatementBlockNode body;

        private readonly Dictionary<string, ScriptVariable> capturedVariables;

		public SyntaxNodeCollection<VariableDeclarationNode> Args { get; private set; }

		internal ScriptFunction(
            ScriptExecutor executor,
            SyntaxNodeCollection<VariableDeclarationNode> args,
            StatementBlockNode body,
            Dictionary<string, ScriptVariable> capturedVariables)
		{
			this.executor = executor;
			this.Args = args;
			this.body = body;
            this.capturedVariables = capturedVariables;
		}

		internal object Invoke(ScriptExecutionContext context, string stackFrameName, object[] args)
		{
			if (args.Length != this.Args.Count)
				throw new ScriptExecutionException("The number of args must match.");

			context.PushStackFrame(stackFrameName, this.capturedVariables);

			for (var i = 0; i < this.Args.Count; i++)
				context.DeclareVariable(this.Args[i].VariableName, args[i]);
            
            object result = this.executor.Execute(this.body, context);

			context.PopStackFrame();

			return result;
		}
	}
}
