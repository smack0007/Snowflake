using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Tests
{
	public class GenericsTests : LanguageTestFixture
	{
		[Fact]
		public void Generic_Type_Is_Registered_As_ScriptType()
		{
			ScriptEngine engine = new ScriptEngine();
			engine.RegisterType("Tuple", typeof(Tuple<int>));

			var value = engine.GetGlobalVariable("Tuple");

			Assert.IsType<ScriptType>(value);

			ScriptType type = (ScriptType)value;

			Assert.Equal(typeof(Tuple<int>), type.Type);
		}

		[Fact]
		public void Generic_Type_Can_Be_Constructed()
		{			
			AssertScriptReturnValue(
				(engine) =>
				{
					engine.RegisterType("Tuple", typeof(Tuple<int>));
				},
				new Tuple<int>(42),
				"return new Tuple(42);");
		}

		[Fact]
		public void Generic_Type_Definitions_Can_Be_Upgraded_To_ScriptTypeSets()
		{
			ScriptEngine engine = new ScriptEngine();
			engine.RegisterType("Tuple", typeof(Tuple<>));
			engine.RegisterType("Tuple", typeof(Tuple<,>));

			var value = engine.GetGlobalVariable("Tuple");

			Assert.IsType<ScriptTypeSet>(value);

			ScriptTypeSet typeSet = (ScriptTypeSet)value;

			Assert.Equal(2, typeSet.Count);
		}

		[Fact]
		public void Non_Generic_Type_Definitions_Cannot_Be_Upgraded_To_ScriptTypeSets()
		{
			ScriptEngine engine = new ScriptEngine();
			engine.RegisterType("Tuple", typeof(Tuple<int>));
			Assert.Throws<ScriptExecutionException>(() => engine.RegisterType("Tuple", typeof(Tuple<int, int>)));
		}
	}
}
