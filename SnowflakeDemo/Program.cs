using System;
using Snowsoft.SnowflakeScript;

namespace SnowflakeDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			ScriptEngine engine = new ScriptEngine();
			engine.SetGlobalVariable("print", new Action<object>((x) => Console.WriteLine(x)));
			engine.SetGlobalVariable("add", new Func<int, int, int>((x, y) => x + y));
			var result = engine.ExecuteFile("SnowflakeDemo.sfs");

			//Console.WriteLine("Result is: {0} ({1})", result, result.GetType());

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
