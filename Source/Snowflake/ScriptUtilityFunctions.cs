using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake
{
    public static class ScriptUtilityFunctions
    {
        public static ScriptType Import(IScriptExecutionContext context, string dotNetTypeName)
        {
            Type dotNetType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(x => x.FullName == dotNetTypeName);

            if (dotNetType == null)
            {

            }

            string scriptTypeName = "__Imported." + dotNetType.FullName;
            context.RegisterType(scriptTypeName, dotNetType);
            return new ScriptType(scriptTypeName);
        }
    }
}
