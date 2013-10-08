using NUnit.Framework;
using Snowsoft.SnowflakeScript.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests.Execution
{
	[TestFixture]
	public class ScriptBooleanTests
	{
		[Test]
		public void Inverse_Of_True_Is_False()
		{
			Assert.AreSame(ScriptBoolean.False, ScriptBoolean.True.Inverse());
		}

		[Test]
		public void Inverse_Of_False_Is_True()
		{
			Assert.AreSame(ScriptBoolean.True, ScriptBoolean.False.Inverse());
		}
	}
}
