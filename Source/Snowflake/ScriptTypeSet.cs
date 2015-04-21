using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake
{
    public class ScriptTypeSet : DynamicObject
    {
        private readonly Dictionary<int, Type> types;
        private MethodInfo[] staticMethods;

        public Type this[int index]
        {
            get { return this.types[index]; }
            set { this.types[index] = value; }
        }

        public ScriptTypeSet()
        {
            this.types = new Dictionary<int, Type>();
        }

        public ScriptTypeSet(Type type)
            : this()
        {
            if (type.IsGenericType)
            {
                Type[] genericArgs = type.GetGenericArguments();
                this.types[genericArgs.Length] = type;
            }
            else
            {
                this.types[0] = type;
            }
        }

        public bool ContainsKey(int key)
        {
            return this.types.ContainsKey(key);
        }

        public bool TryGetType(int key, out Type value)
        {
            return this.types.TryGetValue(key, out value);
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

            Type type;
            if (this.types.TryGetValue(0, out type))
            {
                if (this.staticMethods == null)
                {
                    this.staticMethods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
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
                        throw new ScriptExecutionException(string.Format("More than 1 method named \"{0}\" exists for type \"{1}\". Unable to determine which method to invoke.", binder.Name, type));
                    }
                    else
                    {
                        throw new ScriptExecutionException(string.Format("No method named \"{0}\" exists for type \"{1}\".", binder.Name, type));
                    }
                }
            }

            return false;
        }
    }
}
