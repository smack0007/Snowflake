using System;

namespace Snowsoft.SnowflakeScript.Execution
{
    public sealed class ScriptUndefined : ScriptObject
    {
        public static readonly ScriptUndefined Value = new ScriptUndefined();

        public override string TypeName
        {
            get { return "undef"; }
        }

        private ScriptUndefined()
        {
        }

        public override object Unbox()
        {
            return null;
        }

        public override ScriptBoolean EqualTo(ScriptObject other)
        {
            return other is ScriptUndefined ? ScriptBoolean.True : ScriptBoolean.False;
        }
    }
}
