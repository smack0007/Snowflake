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
            context.PushStackFrame(stackFrameName, this.capturedVariables);

            if (args.Length > this.Args.Count)
                throw new ScriptExecutionException("Too many arguments provided.", context.GetStackFrames());

			if (args.Length != this.Args.Count)
            {
                var passedArgs = args;
				args = new object[this.Args.Count];
                
                int i = 0;
                
                for (; i < passedArgs.Length; i++)
                    args[i] = passedArgs[i];

                for (; i < this.Args.Count; i++)
                {
                    if (this.Args[i].ValueExpression == null)
                        throw new ScriptExecutionException($"No default value provided for argument '{this.Args[i].VariableName}'.");

                    args[i] = this.executor.Evaluate(this.Args[i].ValueExpression, context);
                }
            }

			for (var i = 0; i < this.Args.Count; i++)
				context.DeclareVariable(this.Args[i].VariableName, args[i]);
            
            object result = this.executor.Execute(this.body, context);

			context.PopStackFrame();

			return result;
		}
	}
}
