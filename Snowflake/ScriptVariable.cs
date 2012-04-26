using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptVariable
	{
		public object Val
		{
			get;
			private set;
		}

		public ScriptVariableType Type
		{
			get;
			private set;
		}			

		public ScriptVariable()
		{
			this.Type = ScriptVariableType.Null;
		}

		public ScriptVariable(bool val)
		{
			this.Val = val;
			this.Type = ScriptVariableType.Boolean;
		}

		public ScriptVariable(char val)
		{
			this.Val = val;
			this.Type = ScriptVariableType.Char;
		}

		public ScriptVariable(string val)
		{
			this.Val = val;
			this.Type = ScriptVariableType.String;
		}

		public ScriptVariable(int val)
		{
			this.Val = val;
			this.Type = ScriptVariableType.Integer;
		}

		public ScriptVariable(double val)
		{
			this.Val = val;
			this.Type = ScriptVariableType.Float;
		}

		public ScriptVariable(Dictionary<string, ScriptVariable> val)
		{
			this.Val = val;
			this.Type = ScriptVariableType.Array;
		}

		public void Gets(ScriptVariable variable)
		{
			this.Val = variable.Val;
			this.Type = variable.Type;
		}

		public ScriptVariable Add(ScriptVariable variable)
		{
			if (this.Type == ScriptVariableType.Char)
			{
				return new ScriptVariable(this.ToString() + variable.ToString());
			}
			else if (this.Type == ScriptVariableType.String)
			{
				return new ScriptVariable(this.ToString() + variable.ToString());
			}
			else if (this.Type == ScriptVariableType.Integer)
			{
				if(variable.Type == ScriptVariableType.Integer) // Integer to Integer Addition
				{
					return new ScriptVariable(this.ToInteger() + variable.ToInteger());
				}
				else if(variable.Type == ScriptVariableType.Float) // Upgrade this to Float
				{
					return new ScriptVariable(this.ToFloat() + variable.ToFloat());
				}
			}
			else if(this.Type == ScriptVariableType.Float)
			{
				return new ScriptVariable(this.ToFloat() + variable.ToFloat());
			}

			throw new ScriptVariableException("Add not available for type " + this.Type.ToString());
		}

		public ScriptVariable Subtract(ScriptVariable variable)
		{
			if(this.Type == ScriptVariableType.Integer)
			{
				if(variable.Type == ScriptVariableType.Integer) // Integer to Integer Subtraction
				{
					return new ScriptVariable(this.ToInteger() - variable.ToInteger());
				}
				else if(variable.Type == ScriptVariableType.Float) // Upgrade this to Float
				{
					return new ScriptVariable(this.ToFloat() - variable.ToFloat());
				}
			}
			else if(this.Type == ScriptVariableType.Float)
			{
				return new ScriptVariable(this.ToFloat() - variable.ToFloat());
			}

			throw new ScriptVariableException("Subtract not available for type " + this.Type.ToString());
		}

		public ScriptVariable EqualTo(ScriptVariable variable)
		{
			if(this.Type == ScriptVariableType.Integer)
			{
				return new ScriptVariable(this.ToInteger() == variable.ToInteger());
			}
			else if(this.Type == ScriptVariableType.Float)
			{
				return new ScriptVariable(this.ToFloat() == variable.ToFloat());
			}
			else if(this.Type == ScriptVariableType.String)
			{
				return new ScriptVariable(this.ToString() == variable.ToString());
			}

			throw new ScriptVariableException("EqualTo not available for type " + this.Type.ToString());
		}

		public ScriptVariable AtIndex(ScriptVariable index)
		{
			if(this.Type != ScriptVariableType.Array)
				throw new ScriptVariableException("Variable is not an array");

			if(index != null) // Key was specified
			{
				string key = index.ToString();

				if (((Dictionary<string, ScriptVariable>)this.Val).ContainsKey(key))
				{
					return ((Dictionary<string, ScriptVariable>)this.Val)[key];
				}
				else
				{
					ScriptVariable variable = new ScriptVariable();
					((Dictionary<string, ScriptVariable>)this.Val).Add(key, variable);

					return variable;
				}
			}
			else // Generate a new variable and return it
			{
				ScriptVariable variable = new ScriptVariable();

				((Dictionary<string, ScriptVariable>)this.Val).Add(((Dictionary<string, ScriptVariable>)this.Val).Count.ToString(), variable);

				return variable;
			}
		}

		public bool ToBoolean()
		{
			if(this.Type == ScriptVariableType.Null)
				return false;
			else if(this.Type == ScriptVariableType.Boolean)
				return (bool)this.Val;
			else if(this.Type == ScriptVariableType.String)
			{
				string s = this.ToString().ToUpper();

				if(s == "TRUE" || s == "T")
				{
					return true;
				}
				else if(s == "FALSE" || s == "F")
				{
					return false;
				}
			}
			else if(this.Type == ScriptVariableType.Integer)
			{
				int i = this.ToInteger();

				if(i == 0)
					return false;

				return true;
			}
			else if(this.Type == ScriptVariableType.Float)
			{
				double f = this.ToFloat();

				if(f == 0.0)
					return false;

				return true;
			}

			throw new ScriptVariableException("Boolean conversion not available for type " + this.Type.ToString());
		}

		public override string ToString()
		{
			if (this.Type == ScriptVariableType.Null)
			{
				return "Null";
			}
			else if (this.Type == ScriptVariableType.Boolean)
			{
				return ((bool)this.Val).ToString();
			}
			else if (this.Type == ScriptVariableType.Char)
			{
				return ((char)this.Val).ToString();
			}
			else if (this.Type == ScriptVariableType.String)
			{
				return (string)this.Val;
			}
			else if (this.Type == ScriptVariableType.Integer)
			{
				return ((int)this.Val).ToString();
			}
			else if (this.Type == ScriptVariableType.Float)
			{
				return ((double)this.Val).ToString();
			}
			else if (this.Type == ScriptVariableType.Array)
			{
				Dictionary<string, ScriptVariable> dictionary = ((Dictionary<string, ScriptVariable>)this.Val);

				string s = "";
				foreach (string key in dictionary.Keys)
				{
					if (s != "")
						s += ", ";

					s += key + " => " + dictionary[key];
				}

				return '{' + s + '}';
			}

			throw new ScriptVariableException("String conversion not available for type " + this.Type.ToString());
		}

		public int ToInteger()
		{
			if (this.Type == ScriptVariableType.Null)
			{
				return 0;
			}
			else if (this.Type == ScriptVariableType.Integer || this.Type == ScriptVariableType.Float)
			{
				return (int)this.Val;
			}

			throw new ScriptVariableException("Integer conversion not available for type " + this.Type.ToString());
		}

		public double ToFloat()
		{
			if (this.Type == ScriptVariableType.Null)
			{
				return 0;
			}
			else if (this.Type == ScriptVariableType.Integer)
			{
				return (double)this.Val;
			}
			else if (this.Type == ScriptVariableType.Float)
			{
				return (double)this.Val;
			}

			throw new ScriptVariableException("Float conversion not available for type " + this.Type.ToString());
		}
	}
}
