using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Snowflake.Execution;

namespace Snowflake.Execution
{
    public class ScriptType : DynamicObject
    {
        private MethodInfo[] staticMethods;

        public Type Type
        {
            get;
            private set;
        }

		public bool IsGenericTypeDefinition
		{
			get { return this.Type.IsGenericTypeDefinition; }
		}

        public bool IsGenericType
        {
            get { return this.Type.IsGenericType; }
        }

        public int GenericArgumentCount
        {
            get
            {
                if (this.Type.IsGenericType)
                    return this.Type.GetGenericArguments().Length;

                return 0;
            }
        }

        public ScriptType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.Type = type;
        }
        
        public static ScriptType FromValue(dynamic value, params ScriptType[] genericArgs)
        {
            ScriptType scriptType = null;
            Type type = null;

            if (value is ScriptTypeSet)
            {
                ScriptTypeSet scriptTypeSet = (ScriptTypeSet)value;
                                
                if (genericArgs != null && genericArgs.Length > 0)
                {
                    scriptTypeSet.TryGetType(genericArgs.Length, out scriptType);
                    type = scriptType.Type.MakeGenericType(genericArgs.Select(x => x.Type).ToArray());
                }
                else
                {
                    scriptTypeSet.TryGetType(0, out scriptType);
                }
            }
            else if (value is ScriptType)
            {
                type = ((ScriptType)value).Type;
            }
            
            if (type == null)
                throw new ScriptExecutionException("Unable to create ScriptType from value.");

            return new ScriptType(type);
        }

        private static bool MatchMethod(MethodInfo method, string name, object[] args, bool exactTypes)
        {
            if (method.Name != name)
                return false;

            ParameterInfo[] parameters = method.GetParameters();

            if (parameters.Length != args.Length)
                return false;

            for (int i = 0; i < parameters.Length; i++)
            {
                if (exactTypes)
                {
                    if (parameters[i].ParameterType != args[i].GetType())
                        return false;
                }
                else
                {
                    if (!parameters[i].ParameterType.IsAssignableFrom(args[i].GetType()))
                        return false;
                }
            }

            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = null;
                        
            if (this.staticMethods == null)
            {
                this.staticMethods = this.Type.GetMethods(BindingFlags.Static | BindingFlags.Public);
            }

            IEnumerable<MethodInfo> matchedMethods = this.staticMethods.Where(x => MatchMethod(x, binder.Name, args, true));
            int count = matchedMethods.Count();

            if (count != 1)
            {
                matchedMethods = this.staticMethods.Where(x => MatchMethod(x, binder.Name, args, false));
                count = matchedMethods.Count();
            }

            if (count == 1)
            {
                result = matchedMethods.First().Invoke(null, args);
                return true;
            }
            else
            {
                matchedMethods = this.staticMethods.Where(x => x.Name == binder.Name);

                if (matchedMethods.Count() > 1)
                {
                    throw new ScriptExecutionException(string.Format("More than 1 method named \"{0}\" exists for type \"{1}\". Unable to determine which method to invoke.", binder.Name, this.Type));
                }
                else
                {
                    throw new ScriptExecutionException(string.Format("No method named \"{0}\" exists for type \"{1}\".", binder.Name, this.Type));
                }
            }
        }
    }
}
