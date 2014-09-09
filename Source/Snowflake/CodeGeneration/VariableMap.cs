using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowsoft.SnowflakeScript.CodeGeneration
{
	public class VariableMap
	{
		List<Dictionary<string, string>> frames;
		int variableCount;
		
		private Dictionary<string, string> CurrentFrame
		{
			get { return this.frames[this.frames.Count - 1]; }
		}
		
		public VariableMap()
		{
			this.frames = new List<Dictionary<string, string>>();
			this.frames.Add(new Dictionary<string, string>());
		}

		public void PushFrame()
		{
			this.frames.Add(new Dictionary<string, string>());
		}

		public void PopFrame()
		{
			if (this.frames.Count <= 1)
				throw new InvalidOperationException("Cannot pop root frame.");

			this.frames.RemoveAt(this.frames.Count - 1);
		}

		public bool CurrentFrameContainsKey(string key)
		{
			return this.CurrentFrame.ContainsKey(key);
		}

		private string GetNextVariableName()
		{
			this.variableCount++;
			return 'v' + this.variableCount.ToString();
		}

		public string DeclarVariable(string realName, int line, int column)
		{
			if (this.CurrentFrame.ContainsKey(realName))
				throw new CodeGenerationException(string.Format("Variable {0} declared more than once.", realName), line, column);

			string anonymousName = this.GetNextVariableName();
			this.CurrentFrame.Add(realName, anonymousName);
			return anonymousName;
		}

		public string GetVariableName(string realName)
		{
			for (int i = this.frames.Count - 1; i >= 0; i--)
			{
				var frame = this.frames[i];
				if (frame.ContainsKey(realName))
					return frame[realName];
			}

			return "this.GetGlobalVariable(\"" + realName + "\")";
		}
	}
}
