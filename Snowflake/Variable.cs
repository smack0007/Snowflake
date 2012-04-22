using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript
{
	public class VariableException : ApplicationException
	{
		public VariableException(string message)
			: base(message)
		{
		}
	}

	public class Variable
	{
		object val;

		public object Val
		{
			get { return val; }
			set { val = value; }
		}

		VariableType type;

		VariableType Type
		{
			get { return type; }
			set { type = value; }
		}			

		public Variable()
		{
			this.type = VariableType.Null;
		}

		public Variable(bool val)
		{
			this.val = val;
			this.type = VariableType.Boolean;
		}

		public Variable(string val)
		{
			this.val = val;
			this.type = VariableType.String;
		}

		public Variable(int val)
		{
			this.val = val;
			this.type = VariableType.Integer;
		}

		public Variable(double val)
		{
			this.val = val;
			this.type = VariableType.Float;
		}

		public Variable(Dictionary<string, Variable> val)
		{
			this.val = val;
			this.type = VariableType.Array;
		}

		public void Gets(Variable variable)
		{
			this.val = variable.val;
			this.type = variable.type;
		}

		public Variable Add(Variable variable)
		{
			if(type == VariableType.String)
			{
				return new Variable(this.ToString() + variable.ToString());
			}
			if(type == VariableType.Integer)
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
			else if(type == VariableType.Float)
			{
				return new Variable(this.ToFloat() + variable.ToFloat());
			}

			throw new VariableException("Add not available for type " + type.ToString());
		}

		public Variable Subtract(Variable variable)
		{
			if(type == VariableType.Integer)
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
			else if(type == VariableType.Float)
			{
				return new Variable(this.ToFloat() - variable.ToFloat());
			}

			throw new VariableException("Subtract not available for type " + type.ToString());
		}

		public Variable EqualTo(Variable variable)
		{
			if(type == VariableType.Integer)
			{
				return new Variable(this.ToInteger() == variable.ToInteger());
			}
			else if(type == VariableType.Float)
			{
				return new Variable(this.ToFloat() == variable.ToFloat());
			}
			else if(type == VariableType.String)
			{
				return new Variable(this.ToString() == variable.ToString());
			}

			throw new VariableException("EqualTo not available for type " + type.ToString());
		}

		public Variable AtIndex(Variable index)
		{
			if(this.Type != VariableType.Array)
				throw new VariableException("Variable is not an array");

			if(index != null) // Key was specified
			{
				string key = index.ToString();

				if(((Dictionary<string, Variable>)val).ContainsKey(key))
					return ((Dictionary<string, Variable>)val)[key];
				else
				{
					Variable variable = new Variable();
					((Dictionary<string, Variable>)val).Add(key, variable);

					return variable;
				}
			}
			else // Generate a new variable and return it
			{
				Variable variable = new Variable();

				((Dictionary<string, Variable>)val).Add(((Dictionary<string, Variable>)val).Count.ToString(), variable);

				return variable;
			}
		}

		public bool ToBoolean()
		{
			if(type == VariableType.Null)
				return false;
			else if(type == VariableType.Boolean)
				return (bool)val;
			else if(type == VariableType.String)
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
			else if(type == VariableType.Integer)
			{
				int i = this.ToInteger();

				if(i == 0)
					return false;

				return true;
			}
			else if(type == VariableType.Float)
			{
				double f = this.ToFloat();

				if(f == 0.0)
					return false;

				return true;
			}

			throw new VariableException("Boolean conversion not available for type " + type.ToString());
		}

		public override string ToString()
		{
			if(type == VariableType.Null)
				return "Null";
			else if(type == VariableType.Boolean)
				return ((bool)val).ToString();
			else if(type == VariableType.String)
				return (string)val;
			else if(type == VariableType.Integer)
				return ((int)val).ToString();
			else if(type == VariableType.Float)
				return ((double)val).ToString();
			else if(type == VariableType.Array)
			{
				Dictionary<string, Variable> dictionary = ((Dictionary<string, Variable>)val);

				string s = "";
				foreach(string key in dictionary.Keys)
				{
					if(s != "")
						s += ", ";

					s += key + " => " + dictionary[key];
				}

				return '{' + s + '}';
			}

			throw new VariableException("String conversion not available for type " + type.ToString());
		}

		public int ToInteger()
		{
			if(type == VariableType.Null)
				return 0;
			else if(type == VariableType.Integer || type == VariableType.Float)
				return (int)val;

			throw new VariableException("Integer conversion not available for type " + type.ToString());
		}

		public double ToFloat()
		{
			if(type == VariableType.Null)
				return 0;
			else if(type == VariableType.Integer)
				return (double)val;
			else if(type == VariableType.Float)
				return (double)val;

			throw new VariableException("Float conversion not available for type " + type.ToString());
		}
	}
}
