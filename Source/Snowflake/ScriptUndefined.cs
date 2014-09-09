using System;

namespace Snowflake
{
	public sealed class ScriptUndefined
	{
		public static readonly ScriptUndefined Value = new ScriptUndefined();
				
		private ScriptUndefined()
		{
		}

		public override string ToString()
		{
			return "undef";
		}
	}
}
