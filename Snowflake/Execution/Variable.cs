using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class Variable
	{
		public static readonly Variable Null = new Variable();

		public object Value
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
			this.Value = val;
			this.Type = VariableType.Boolean;
		}

		public Variable(char val)
		{
			this.Value = val;
			this.Type = VariableType.Char;
		}

		public Variable(string val)
		{
			this.Value = val;
			this.Type = VariableType.String;
		}

		public Variable(int val)
		{
			this.Value = val;
			this.Type = VariableType.Integer;
		}

		public Variable(double val)
		{
			this.Value = val;
			this.Type = VariableType.Float;
		}

		public Variable(Dictionary<string, Variable> val)
		{
			this.Value = val;
			this.Type = VariableType.Array;
		}

		public void Gets(Variable variable)
		{
			this.Value = variable.Value;
			this.Type = variable.Type;
		}

		public Variable Add(Variable variable)
		{
			if (this.Type == VariableType.Char)
			{
				return new Variable(this.ToString() + variable.ToString());
			}
			else if (this.Type == VariableType.String)
			{
				return new Variable(this.ToString() + variable.ToString());
			}
			else if (this.Type == VariableType.Integer)
			{
				if(variable.Type == VariableType.Integer) // Integer to Integer Addition
				{
					return new Variable(this.ToInteger() + variable.ToInteger());
				}
				else if(variable.Type == VariableType.Float) // Upgrade this to Float
				{
					return new Variable(this.ToFloat() + variable.ToFloat());
				}
				else if (variable.Type == VariableType.String ||
						 variable.Type == VariableType.Char)
				{
					return new Variable(this.ToString() + variable.ToString());
				}
			}
			else if(this.Type == VariableType.Float)
			{
				return new Variable(this.ToFloat() + variable.ToFloat());
			}

			throw new VariableException("Add not available for type " + this.Type + " to " + variable.Type + ".");
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

			throw new VariableException("Subtract not available for type " + this.Type.ToString());
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

			throw new VariableException("EqualTo not available for type " + this.Type.ToString());
		}

		public Variable AtIndex(Variable index)
		{
			if(this.Type != VariableType.Array)
				throw new VariableException("Variable is not an array");

			if(index != null) // Key was specified
			{
				string key = index.ToString();

				if (((Dictionary<string, Variable>)this.Value).ContainsKey(key))
				{
					return ((Dictionary<string, Variable>)this.Value)[key];
				}
				else
				{
					Variable variable = new Variable();
					((Dictionary<string, Variable>)this.Value).Add(key, variable);

					return variable;
				}
			}
			else // Generate a new variable and return it
			{
				Variable variable = new Variable();

				((Dictionary<string, Variable>)this.Value).Add(((Dictionary<string, Variable>)this.Value).Count.ToString(), variable);

				return variable;
			}
		}

		public bool ToBoolean()
		{
			if(this.Type == VariableType.Null)
				return false;
			else if(this.Type == VariableType.Boolean)
				return (bool)this.Value;
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

			throw new VariableException("Boolean conversion not available for type " + this.Type.ToString());
		}

		public override string ToString()
		{
			if (this.Type == VariableType.Null)
			{
				return "Null";
			}
			else if (this.Type == VariableType.Boolean)
			{
				return ((bool)this.Value).ToString();
			}
			else if (this.Type == VariableType.Char)
			{
				return ((char)this.Value).ToString();
			}
			else if (this.Type == VariableType.String)
			{
				return (string)this.Value;
			}
			else if (this.Type == VariableType.Integer)
			{
				return ((int)this.Value).ToString();
			}
			else if (this.Type == VariableType.Float)
			{
				return ((double)this.Value).ToString();
			}
			else if (this.Type == VariableType.Array)
			{
				Dictionary<string, Variable> dictionary = ((Dictionary<string, Variable>)this.Value);

				string s = "";
				foreach (string key in dictionary.Keys)
				{
					if (s != "")
						s += ", ";

					s += key + " => " + dictionary[key];
				}

				return '{' + s + '}';
			}

			throw new VariableException("String conversion not available for type " + this.Type.ToString());
		}

		public int ToInteger()
		{
			if (this.Type == VariableType.Null)
			{
				return 0;
			}
			else if (this.Type == VariableType.Integer || this.Type == VariableType.Float)
			{
				return (int)this.Value;
			}

			throw new VariableException("Integer conversion not available for type " + this.Type.ToString());
		}

		public double ToFloat()
		{
			if (this.Type == VariableType.Null)
			{
				return 0;
			}
			else if (this.Type == VariableType.Integer)
			{
				return (double)this.Value;
			}
			else if (this.Type == VariableType.Float)
			{
				return (double)this.Value;
			}

			throw new VariableException("Float conversion not available for type " + this.Type.ToString());
		}
	}
}
