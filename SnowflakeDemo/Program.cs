using System;
using Snowsoft.SnowflakeScript;

namespace SnowflakeDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			Script script = Script.FromFile("SnowflakeDemo.sfs");
			script.Execute();

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
