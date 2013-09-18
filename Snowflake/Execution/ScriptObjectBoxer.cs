using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptObjectBoxer
	{
		public ScriptObject Box(object value)
		{
			if (value == null)
				return ScriptNull.Value;

			Type type = value.GetType();
			ScriptObject scriptObject = null;

			if (type == typeof(int))
			{
				scriptObject = new ScriptInteger((int)value);
			}
			else if (type == typeof(float))
			{
				scriptObject = new ScriptFloat((float)value);
			}
			else if (value is Delegate)
			{
				scriptObject = new ScriptClrFunction((Delegate)value);
			}
			else
			{
				throw new ScriptException(string.Format("Unable to set global variable \"{0}\". Type {1} is not supported.", type));
			}

			return scriptObject;
		}
	}
}
