using Xunit;
using Snowflake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			AssertScriptReturnValue(assert, (x) => { }, script);
		}

		public void AssertScriptReturnValue<T>(T expectedValue, Action<ScriptEngine> setup, string script)
		{
			AssertScriptReturnValue<T>((x) => Assert.Equal(expectedValue, x), setup, script);
		}

		public void AssertScriptReturnValue<T>(Action<T> assert, Action<ScriptEngine> setup, string script)
		{
			ScriptEngine engine = new ScriptEngine();
			setup(engine);
			Console.WriteLine(engine.GenerateCode(script, "Script1"));
			var result = (T)engine.Execute(script);
			assert(result);
		}

        public void AssertScriptIsException<T>(string script)
            where T : Exception
        {
            AssertScriptIsException<T>(script, (x) => { });
        }

		public void AssertScriptIsException<T>(string script, Action<Exception> assert)
			where T : Exception
		{
			ScriptEngine engine = new ScriptEngine();
            
            try
            {
                Console.WriteLine(engine.GenerateCode(script, "Script1"));
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
