using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptVariable
	{
		public static readonly ScriptVariable Null = new ScriptVariable();

		public object Value
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

		public ScriptVariable(ScriptFunction function)
		{
			this.Value = function;
			this.Type = ScriptVariableType.Function;
		}

		public ScriptVariable(bool val)
		{
			this.Value = val;
			this.Type = ScriptVariableType.Boolean;
		}

		public ScriptVariable(char val)
		{
			this.Value = val;
			this.Type = ScriptVariableType.Char;
		}

		public ScriptVariable(string val)
		{
			this.Value = val;
			this.Type = ScriptVariableType.String;
		}

		public ScriptVariable(int val)
		{
			this.Value = val;
			this.Type = ScriptVariableType.Integer;
		}

		public ScriptVariable(double val)
		{
			this.Value = val;
			this.Type = ScriptVariableType.Float;
		}

		public void Gets(ScriptVariable variable)
		{
			this.Value = variable.Value;
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
				else if (variable.Type == ScriptVariableType.String ||
						 variable.Type == ScriptVariableType.Char)
				{
					return new ScriptVariable(this.ToString() + variable.ToString());
				}
			}
			else if(this.Type == ScriptVariableType.Float)
			{
				return new ScriptVariable(this.ToFloat() + variable.ToFloat());
			}

			throw new ScriptExecutionException("Add not available for type " + this.Type + " to " + variable.Type + ".");
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

			throw new ScriptExecutionException("Subtract not available for type " + this.Type.ToString());
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

			throw new ScriptExecutionException("EqualTo not available for type " + this.Type.ToString());
		}

		//public Variable AtIndex(Variable index)
		//{
		//	if(this.Type != VariableType.Array)
		//		throw new VariableException("Variable is not an array");

		//	if(index != null) // Key was specified
		//	{
		//		string key = index.ToString();

		//		if (((Dictionary<string, Variable>)this.Value).ContainsKey(key))
		//		{
		//			return ((Dictionary<string, Variable>)this.Value)[key];
		//		}
		//		else
		//		{
		//			Variable variable = new Variable();
		//			((Dictionary<string, Variable>)this.Value).Add(key, variable);

		//			return variable;
		//		}
		//	}
		//	else // Generate a new variable and return it
		//	{
		//		Variable variable = new Variable();

		//		((Dictionary<string, Variable>)this.Value).Add(((Dictionary<string, Variable>)this.Value).Count.ToString(), variable);

		//		return variable;
		//	}
		//}

		public bool ToBoolean()
		{
			if (this.Type == ScriptVariableType.Null)
			{
				return false;
			}
			else if (this.Type == ScriptVariableType.Boolean)
			{
				return (bool)this.Value;
			}
			else if (this.Type == ScriptVariableType.String)
			{
				string s = this.ToString().ToUpper();

				if (s == "TRUE" || s == "T")
				{
					return true;
				}
				else if (s == "FALSE" || s == "F")
				{
					return false;
				}
			}
			else if (this.Type == ScriptVariableType.Integer)
			{
				int i = this.ToInteger();

				if (i == 0)
					return false;

				return true;
			}
			else if (this.Type == ScriptVariableType.Float)
			{
				double f = this.ToFloat();

				if (f == 0.0)
					return false;

				return true;
			}

			throw new ScriptExecutionException("Boolean conversion not available for type " + this.Type.ToString());
		}

		public override string ToString()
		{
			if (this.Type == ScriptVariableType.Null)
			{
				return "Null";
			}
			else if (this.Type == ScriptVariableType.Boolean)
			{
				return ((bool)this.Value).ToString();
			}
			else if (this.Type == ScriptVariableType.Char)
			{
				return ((char)this.Value).ToString();
			}
			else if (this.Type == ScriptVariableType.String)
			{
				return (string)this.Value;
			}
			else if (this.Type == ScriptVariableType.Integer)
			{
				return ((int)this.Value).ToString();
			}
			else if (this.Type == ScriptVariableType.Float)
			{
				return ((double)this.Value).ToString();
			}
			//else if (this.Type == VariableType.Array)
			//{
			//	Dictionary<string, Variable> dictionary = ((Dictionary<string, Variable>)this.Value);

			//	string s = "";
			//	foreach (string key in dictionary.Keys)
			//	{
			//		if (s != "")
			//			s += ", ";

			//		s += key + " => " + dictionary[key];
			//	}

			//	return '{' + s + '}';
			//}

			throw new ScriptExecutionException("String conversion not available for type " + this.Type.ToString());
		}

		public int ToInteger()
		{
			if (this.Type == ScriptVariableType.Null)
			{
				return 0;
			}
			else if (this.Type == ScriptVariableType.Integer || this.Type == ScriptVariableType.Float)
			{
				return (int)this.Value;
			}

			throw new ScriptExecutionException("Integer conversion not available for type " + this.Type.ToString());
		}

		public double ToFloat()
		{
			if (this.Type == ScriptVariableType.Null)
			{
				return 0;
			}
			else if (this.Type == ScriptVariableType.Integer)
			{
				return (double)this.Value;
			}
			else if (this.Type == ScriptVariableType.Float)
			{
				return (double)this.Value;
			}

			throw new ScriptExecutionException("Float conversion not available for type " + this.Type.ToString());
		}
	}
}
