using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptTypeBoxer
	{
		public ScriptObject Box(object value)
		{
			if (value == null)
				return ScriptNull.Value;

			ScriptObject scriptObject = null;

			if (value is bool)
			{
				scriptObject = new ScriptBoolean((bool)value);
			}
			else if (value is char)
			{
				scriptObject = new ScriptCharacter((char)value);
			}
			else if (value is Delegate)
			{
				scriptObject = new ScriptClrMethod((Delegate)value);
			}
			else if (value is float)
			{
				scriptObject = new ScriptFloat((float)value);
			}
			else if (value is int)
			{
				scriptObject = new ScriptInteger((int)value);
			}
			else if (value is string)
			{
				scriptObject = new ScriptString((string)value);
			}
			else
			{
				throw new ScriptException(string.Format("Unable to set global variable \"{0}\". Type {1} is not supported.", value.GetType()));
			}

			return scriptObject;
		}
	}
}
