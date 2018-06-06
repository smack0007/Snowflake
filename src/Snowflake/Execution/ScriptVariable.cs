using Snowflake.Execution;

namespace Snowflake.Execution
{
    public sealed class ScriptVariable
    {
        private object value;

        public object Value
        {
            get { return this.value; }
            
            set
            {
                if (this.IsConst)
                    throw new ScriptExecutionException("Const variable value cannot be changed.");

                this.value = value;
            }
        }

        public bool IsGlobal { get; }

        public bool IsConst { get; }

        internal ScriptVariable(object value, bool isGlobal, bool isConst)
        {
            this.value = value;
            this.IsGlobal = isGlobal;
            this.IsConst = isConst;
        }
    }
}
