using System;
using Snowsoft.SnowflakeScript;

namespace SnowflakeDemo
{
	class Program
	{
		static void Main(string[] args)
		{			
			Random random = new Random();

			ScriptEngine engine = new ScriptEngine();			
			engine.SetGlobalFunction<object>("print", (x) => Console.WriteLine(x));
			engine.SetGlobalFunction<int>("GetNumber", () => random.Next());
			engine.SetGlobalFunction<int, int, int>("add", (x, y) => x + y);
			var result = engine.ExecuteFile("SnowflakeDemo.sfs");

			//Console.WriteLine("Result is: {0} ({1})", result, result.GetType());

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
