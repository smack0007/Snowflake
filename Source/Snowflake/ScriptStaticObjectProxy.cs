using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake
{
    public class ScriptStaticObjectProxy : DynamicObject
    {
        private readonly Type type;
        private readonly MethodInfo[] methods;

        public Type ProxiedType
        {
            get;
            private set;
        }

        public ScriptStaticObjectProxy(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.ProxiedType = type;

            this.methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
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
            IEnumerable<MethodInfo> matchedMethods = this.methods.Where(x => MatchMethod(x, binder.Name, args, true));
            int count = matchedMethods.Count();

            if (count != 1)
            {
                matchedMethods = this.methods.Where(x => MatchMethod(x, binder.Name, args, false));
                count = matchedMethods.Count();
            }

            if (count == 1)
            {
                result = matchedMethods.First().Invoke(null, args);
                return true;
            }
            else
            {
                matchedMethods = this.methods.Where(x => x.Name == binder.Name);

                if (matchedMethods.Count() > 1)
                {
                    throw new ScriptExecutionException(string.Format("More than 1 method named \"{0}\" exists for type \"{1}\". Unable to determine which method to invoke.", binder.Name, this.ProxiedType));
                }
                else
                {
                    throw new ScriptExecutionException(string.Format("No method named \"{0}\" exists for type \"{1}\".", binder.Name, this.ProxiedType));
                }
            }
        }
        
        //public override bool TryGetMember(GetMemberBinder binder, out object result)
        //{
        //    if (binder.N)

        //    return base.TryGetMember(binder, out result);
        //}
    }
}
