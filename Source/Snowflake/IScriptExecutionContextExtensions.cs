using System;

namespace Snowflake
{
    public static class IScriptExecutionContextExtensions
    {
        public static void SetGlobalFunction(this IScriptExecutionContext context, string name, Action value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1>(this IScriptExecutionContext context, string name, Action<T1> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2>(this IScriptExecutionContext context, string name, Action<T1, T2> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3>(this IScriptExecutionContext context, string name, Action<T1, T2, T3> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4>(this IScriptExecutionContext context, string name, Action<T1, T2, T3, T4> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5>(this IScriptExecutionContext context, string name, Action<T1, T2, T3, T4, T5> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6>(this IScriptExecutionContext context, string name, Action<T1, T2, T3, T4, T5, T6> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7>(this IScriptExecutionContext context, string name, Action<T1, T2, T3, T4, T5, T6, T7> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8>(this IScriptExecutionContext context, string name, Action<T1, T2, T3, T4, T5, T6, T7, T8> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IScriptExecutionContext context, string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IScriptExecutionContext context, string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IScriptExecutionContext context, string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IScriptExecutionContext context, string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this IScriptExecutionContext context, string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this IScriptExecutionContext context, string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this IScriptExecutionContext context, string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this IScriptExecutionContext context, string name, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<TResult>(this IScriptExecutionContext context, string name, Func<TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, TResult>(this IScriptExecutionContext context, string name, Func<T1, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, T3, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, T3, T4, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, T3, T4, T5, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, T3, T4, T5, T6, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, T3, T4, T5, T6, T7, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this IScriptExecutionContext context, string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> value)
        {
            context.SetGlobalVariable(name, value);
        }

        public static void SetGlobalStaticObject(this IScriptExecutionContext context, string name, Type type)
        {
            context.SetGlobalVariable(name, new ScriptStaticObjectProxy(type));
        }
    }
}
