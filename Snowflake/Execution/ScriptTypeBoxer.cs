using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public sealed class ScriptTypeBoxer
	{
		public ScriptObject Box(object value)
		{		
			ScriptObject scriptObject = null;

			if (value == null)
			{
				scriptObject = ScriptNull.Value;
			}
			else if (value is bool)
			{
				if ((bool)value)
				{
					scriptObject = ScriptBoolean.True;
				}
				else
				{
					scriptObject = ScriptBoolean.False;
				}
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
