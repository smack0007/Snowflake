using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake
{
    public class ScriptVariable
    {
        dynamic value;

        public dynamic Value
        {
            get { return this.value; }
            
            set
            {
                if (this.IsConst)
                    throw new ScriptExecutionException("Const variable value cannot be changed.");

                this.value = value;
            }
        }

        public bool IsConst { get; private set; }

        public ScriptVariable(dynamic value, bool isConst)
        {
            this.value = value;
            this.IsConst = isConst;
        }
    }
}
