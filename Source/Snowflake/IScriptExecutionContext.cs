using System;
using System.Collections.Generic;

namespace Snowflake
{
    public interface IScriptExecutionContext
    {
        dynamic GetGlobalVariable(string name);

        void SetGlobalVariable(string name, dynamic value, bool isConst = false);

        void RegisterType(string name, Type type);
    }
}
