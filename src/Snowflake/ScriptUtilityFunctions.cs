using System;
using System.Linq;
using Snowflake.Execution;

namespace Snowflake
{
    public static class ScriptUtilityFunctions
    {
        public static ScriptType Import(IScriptExecutionContext context, string typeName)
        {
            Type dotNetType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(x => x.FullName == typeName);

            if (dotNetType == null)            
                throw new ScriptExecutionException(string.Format("Unable to import type \"{0}\".", typeName));

            return new ScriptType(dotNetType);
        }

        public static void Export(IScriptExecutionContext context, string name, dynamic value)
        {
            context.SetGlobalVariable(name, value, true);
        }
    }
}
