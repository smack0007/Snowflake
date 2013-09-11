using System;
using Snowsoft.SnowflakeScript;

namespace SnowflakeDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			ScriptEngine engine = new ScriptEngine();
			var result = engine.ExecuteFile("SnowflakeDemo.sfs");

			Console.WriteLine("Result is: {0} ({1})", result, result.GetType());

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
