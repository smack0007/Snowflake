using System;
using Snowsoft.SnowflakeScript;

namespace SnowflakeDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			ScriptEngine engine = new ScriptEngine();
			engine.LoadFromFile("SnowflakeDemo.sfs");
			engine.Run();

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
