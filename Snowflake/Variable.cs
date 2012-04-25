using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript
{
	public class Variable
	{
		public object Val
		{
			get;
			private set;
		}

		public VariableType Type
		{
			get;
			private set;
		}			

		public Variable()
		{
			this.Type = VariableType.Null;
		}

		public Variable(bool val)
		{
			this.Val = val;
			this.Type = VariableType.Boolean;
		}

		public Variable(string val)
		{
			this.Val = val;
			this.Type = VariableType.String;
		}

		public Variable(int val)
		{
			this.Val = val;
			this.Type = VariableType.Integer;
		}

		public Variable(double val)
		{
			this.Val = val;
			this.Type = VariableType.Float;
		}

		public Variable(Dictionary<string, Variable> val)
		{
			this.Val = val;
			this.Type = VariableType.Array;
		}

		public void Gets(Variable variable)
		{
			this.Val = variable.Val;
			this.Type = variable.Type;
		}

		public Variable Add(Variable variable)
		{
			if(this.Type == VariableType.String)
			{
				return new Variable(this.ToString() + variable.ToString());
			}
			if(this.Type == VariableType.Integer)
			{
				if(variable.Type == VariableType.Integer) // Integer to Integer Addition
				{
					return new Variable(this.ToInteger() + variable.ToInteger());
				}
				else if(variable.Type == VariableType.Float) // Upgrade this to Float
				{
					return new Variable(this.ToFloat() + variable.ToFloat());
				}
			}
			else if(this.Type == VariableType.Float)
			{
				return new Variable(this.ToFloat() + variable.ToFloat());
			}

			throw new ScriptException(ScriptError.OperationNotAvailable, "Add not available for type " + this.Type.ToString());
		}

		public Variable Subtract(Variable variable)
		{
			if(this.Type == VariableType.Integer)
			{
				if(variable.Type == VariableType.Integer) // Integer to Integer Subtraction
				{
					return new Variable(this.ToInteger() - variable.ToInteger());
				}
				else if(variable.Type == VariableType.Float) // Upgrade this to Float
				{
					return new Variable(this.ToFloat() - variable.ToFloat());
				}
			}
			else if(this.Type == VariableType.Float)
			{
				return new Variable(this.ToFloat() - variable.ToFloat());
			}

			throw new ScriptException(ScriptError.OperationNotAvailable, "Subtract not available for type " + this.Type.ToString());
		}

		public Variable EqualTo(Variable variable)
		{
			if(this.Type == VariableType.Integer)
			{
				return new Variable(this.ToInteger() == variable.ToInteger());
			}
			else if(this.Type == VariableType.Float)
			{
				return new Variable(this.ToFloat() == variable.ToFloat());
			}
			else if(this.Type == VariableType.String)
			{
				return new Variable(this.ToString() == variable.ToString());
			}

			throw new ScriptException(ScriptError.OperationNotAvailable, "EqualTo not available for type " + this.Type.ToString());
		}

		public Variable AtIndex(Variable index)
		{
			if(this.Type != VariableType.Array)
				throw new ScriptException(ScriptError.OperationNotAvailable, "Variable is not an array");

			if(index != null) // Key was specified
			{
				string key = index.ToString();

				if (((Dictionary<string, Variable>)this.Val).ContainsKey(key))
				{
					return ((Dictionary<string, Variable>)this.Val)[key];
				}
				else
				{
					Variable variable = new Variable();
					((Dictionary<string, Variable>)this.Val).Add(key, variable);

					return variable;
				}
			}
			else // Generate a new variable and return it
			{
				Variable variable = new Variable();

				((Dictionary<string, Variable>)this.Val).Add(((Dictionary<string, Variable>)this.Val).Count.ToString(), variable);

				return variable;
			}
		}

		public bool ToBoolean()
		{
			if(this.Type == VariableType.Null)
				return false;
			else if(this.Type == VariableType.Boolean)
				return (bool)this.Val;
			else if(this.Type == VariableType.String)
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
			else if(this.Type == VariableType.Integer)
			{
				int i = this.ToInteger();

				if(i == 0)
					return false;

				return true;
			}
			else if(this.Type == VariableType.Float)
			{
				double f = this.ToFloat();

				if(f == 0.0)
					return false;

				return true;
			}

			throw new ScriptException(ScriptError.OperationNotAvailable, "Boolean conversion not available for type " + this.Type.ToString());
		}

		public override string ToString()
		{
			if (this.Type == VariableType.Null)
			{
				return "Null";
			}
			else if (this.Type == VariableType.Boolean)
			{
				return ((bool)this.Val).ToString();
			}
			else if (this.Type == VariableType.String)
			{
				return (string)this.Val;
			}
			else if (this.Type == VariableType.Integer)
			{
				return ((int)this.Val).ToString();
			}
			else if (this.Type == VariableType.Float)
			{
				return ((double)this.Val).ToString();
			}
			else if (this.Type == VariableType.Array)
			{
				Dictionary<string, Variable> dictionary = ((Dictionary<string, Variable>)this.Val);

				string s = "";
				foreach (string key in dictionary.Keys)
				{
					if (s != "")
						s += ", ";

					s += key + " => " + dictionary[key];
				}

				return '{' + s + '}';
			}

			throw new ScriptException(ScriptError.OperationNotAvailable, "String conversion not available for type " + this.Type.ToString());
		}

		public int ToInteger()
		{
			if (this.Type == VariableType.Null)
			{
				return 0;
			}
			else if (this.Type == VariableType.Integer || this.Type == VariableType.Float)
			{
				return (int)this.Val;
			}

			throw new ScriptException(ScriptError.OperationNotAvailable, "Integer conversion not available for type " + this.Type.ToString());
		}

		public double ToFloat()
		{
			if (this.Type == VariableType.Null)
			{
				return 0;
			}
			else if (this.Type == VariableType.Integer)
			{
				return (double)this.Val;
			}
			else if (this.Type == VariableType.Float)
			{
				return (double)this.Val;
			}

			throw new ScriptException(ScriptError.OperationNotAvailable, "Float conversion not available for type " + this.Type.ToString());
		}
	}
}
