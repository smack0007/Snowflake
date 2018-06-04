using Xunit;
using System;

namespace Snowflake.Tests
{
    public abstract class LanguageTestFixture
	{
		public void AssertScriptReturnValue<T>(T expectedValue, string script)
		{
			AssertScriptReturnValue<T>((x) => Assert.Equal(expectedValue, x), script);
		}
		
		public void AssertScriptReturnValue<T>(Action<T> assert, string script)
		{
			AssertScriptReturnValue((x) => { }, assert, script);
		}

        public void AssertScriptReturnValue<T>(Action<ScriptEngine> setup, T expectedValue, string script)
		{
			AssertScriptReturnValue<T>(setup, (x) => Assert.Equal(expectedValue, x), script);
		}

        public void AssertScriptReturnValue<T>(Action<ScriptEngine> setup, Action<T> assert, string script)
		{
			ScriptEngine engine = new ScriptEngine();
			setup(engine);

            T result = default;
            object temp = engine.Execute(script);

            if (temp != null)
                result = (T)temp;

            assert(result);
		}

        public void AssertScriptIsException<T>(string script)
            where T : Exception
        {
            AssertScriptIsException<T>((x) => { }, script, (x) => { });
        }

        public void AssertScriptIsException<T>(Action<ScriptEngine> setup, string script)
            where T : Exception
        {
            AssertScriptIsException<T>(setup, script, (x) => { });
        }

        public void AssertScriptIsException<T>(string script, Action<Exception> assert)
            where T : Exception
        {
            AssertScriptIsException<T>((x) => { }, script, assert);
        }

		public void AssertScriptIsException<T>(Action<ScriptEngine> setup, string script, Action<Exception> assert)
			where T : Exception
		{
			ScriptEngine engine = new ScriptEngine();

            setup(engine);

            try
            {
                engine.Execute(script);
            }
            catch (Exception ex)
            {
                Assert.IsType<T>(ex);
                assert(ex);
            }
		}
	}
}
